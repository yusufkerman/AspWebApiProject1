﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Contracts
{
    public interface IUserService
    {
        public Task UpdateUserIpAdress(string ipAddress,string userName);
    }
}
