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

        private readonly IAuthenticationService _authenticationService;

        public ServiceManager(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }
    }
}
