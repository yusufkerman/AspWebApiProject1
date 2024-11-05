using Microsoft.AspNetCore.Mvc;
using Entities;
using Entities.DataTransferObjects;
using Services.Contracts;

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

            var tokenDto = await _serviceManager
                .AuthenticationService
                .GenerateJwtToken(userInformation.UserName);

            return Ok(tokenDto);
        }
    }
}
