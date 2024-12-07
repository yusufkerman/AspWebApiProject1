using Entities.DataTransferObjects;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Contracts
{
    public interface IAuthenticationService
    {
        void InitializeMqttSubscription();
        Task<IdentityResult> RegisterUserAsync(UserRegistirationInformationDto userInformation);
        Task<string> GenerateJwtToken(string username);
        bool ValidateJwtToken(string token);
        string GetUsernameFromJwtToken(string token);
        Task<bool> ValidateUserAsync(UserAuthenticationInformationDto userInformation);
    }
}
