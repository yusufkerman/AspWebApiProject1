﻿using Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class ServiceManager : IServiceManager
    {
        public IAuthenticationService AuthenticationService => throw new NotImplementedException();
    }
}
