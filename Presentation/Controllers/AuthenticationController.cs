using Microsoft.AspNetCore.Mvc;
using Entities;
using Entities.DataTransferObjects;
using Services.Contracts;
using Microsoft.AspNetCore.Identity;
using DotNetEnv;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/authentication")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;
        public AuthenticationController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        [HttpPost]
        public async Task<IActionResult> RegisterUser(
            [FromBody] UserRegistirationInformationDto userInformation)
        {
            //Request header kısmında api key gönderilmiş mi kontrol eder.
            if (!Request.Headers.TryGetValue("X-API-KEY", out var apiKey))
            {
                return Unauthorized("API key is missing"); // 401
            }

            //Gelen api key için doğrulama yapar.
            var isValidApiKey = _serviceManager.ApiKeyValidationService.ValidateApiKey(apiKey);

            //Eğer doğru api key değil ise 401 döner.
            if (!isValidApiKey)
            {
                return Unauthorized("Invalid API key"); // 401
            }

            var result = await _serviceManager
                .AuthenticationService
                .RegisterUser(userInformation);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.TryAddModelError(error.Code, error.Description);
                }
                return BadRequest(ModelState);
            }

            return StatusCode(201);
        }
        [HttpPost("login")]
        public async Task<IActionResult> Authenticate(
            [FromBody] UserAuthenticationInformationDto userInformation)
        {

            if (!await _serviceManager.AuthenticationService.ValidateUser(userInformation))
            {
                return Unauthorized(); //401
            }

            // Kullanıcıyı bul ve IP adresini güncelle
            string ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();
            await _serviceManager.UserService.UpdateUserIpAdress(ipAddress,userInformation.UserName);

            var tokenDto = await _serviceManager
                .AuthenticationService
                .GenerateJwtToken(userInformation.UserName);



            return Ok(tokenDto);
        }
    }
}
