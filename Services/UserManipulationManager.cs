using Entities.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class UserManipulationManager : IUserService
    {
        private readonly UserManager<User> _userManager;

        public UserManipulationManager(UserManager<User> userManager)
        {
            _userManager = userManager;   
        }

        public async Task UpdateUserIpAdress(string ipAddress,string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);

            if (user != null)
            {
                // Kullanıcının IP adresini güncelle
                user.IpAdress = ipAddress;

                // Veritabanında güncelleme işlemi
                await _userManager.UpdateAsync(user);
            }
            else
            {
                throw new Exception("User not found");
            }
        }
    }
}
