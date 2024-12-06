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

        private readonly IAuthenticationService _authenticationService;
        private readonly IUserService _userService;

        public ServiceManager(IAuthenticationService authenticationService,
            IUserService userService)
        {
            _authenticationService = authenticationService;
            _userService = userService;
        }
    }
}
