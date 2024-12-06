using Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class ServiceManager : IServiceManager
    {
        public IAuthenticationService AuthenticationService => _authenticationService;
        public IUserService UserService => _userService;
        public IApiKeyValidationService ApiKeyValidationService => _apiKeyValidationService;


        private readonly IAuthenticationService _authenticationService;
        private readonly IUserService _userService;
        private readonly IApiKeyValidationService _apiKeyValidationService;

        public ServiceManager(IAuthenticationService authenticationService,
            IUserService userService, IApiKeyValidationService apiKeyValidationService)
        {
            _authenticationService = authenticationService;
            _userService = userService;
            _apiKeyValidationService = apiKeyValidationService;
        }
    }
}
