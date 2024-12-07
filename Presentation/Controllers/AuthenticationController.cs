using Microsoft.AspNetCore.Mvc;
using Entities;
using Entities.DataTransferObjects;
using Services.Contracts;
using Microsoft.AspNetCore.Identity;
using DotNetEnv;
using Presentation.ActionFilters;

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
        [ServiceFilter(typeof(ApiKeyFilter))]
        public async Task<IActionResult> RegisterUser(
            [FromBody] UserRegistirationInformationDto userInformation)
        {

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
        [ServiceFilter(typeof(ApiKeyFilter))]
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
