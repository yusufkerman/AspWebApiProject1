using AutoMapper;
using Entities.DataTransferObjects;
using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using MQTTModule.Contracts;
using Services.Contracts;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class AuthenticationManager : IAuthenticationService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<AuthenticationManager> _logger;
        private readonly IMapper _mapper;
        private readonly IApiKeyValidationService _apiKeyValidationService;
        private readonly IMQTTServices _mqttServices;
        private readonly UserManager<User> _userManager;

        private User? _user;

        public AuthenticationManager(IConfiguration configuration,
            IMapper mapper,
            UserManager<User> userManager, IApiKeyValidationService apiKeyValidationService,
            IMQTTServices mqttServices,ILogger<AuthenticationManager> logger)
        {
            _configuration = configuration;
            _mapper = mapper;
            _userManager = userManager;
            _apiKeyValidationService = apiKeyValidationService;
            _mqttServices = mqttServices;
            _logger = logger;
        }
        public void InitializeMqttSubscription()
        {
            //MQTT Server'a abone olunulur (bağımlılık tersine çevrilir)
            _mqttServices.MQTTServer.OnValidateCredintials += ValidateJwtTokenAndApiKeys;
        }
        public async Task<IdentityResult> RegisterUserAsync(UserRegistirationInformationDto userInformation)
        {
            var user = _mapper.Map<User>(userInformation);
            var result = await _userManager.CreateAsync(user,userInformation.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRolesAsync(user, userInformation.Roles);
            }

            return result;
        }

        public async Task<string> GenerateJwtToken(string username)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var secretKey = jwtSettings["SecretKey"];
            var issuer = jwtSettings["Issuer"];
            var audience = jwtSettings["Audience"];

            var claims = await GetClaims(_user);

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(Convert.ToDouble(jwtSettings["ExpirationMinutes"])),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        public bool ValidateJwtToken(string token)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var secretKey = jwtSettings["SecretKey"];
            var issuer = jwtSettings["Issuer"];
            var audience = jwtSettings["Audience"];

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(secretKey);

            try
            {
                // Token'ı doğrulamak için validation parametrelerini ayarlıyoruz
                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidIssuer = issuer,
                    ValidAudience = audience,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                };

                // Token'ı doğrulama ve çözümleme
                var principal = tokenHandler.ValidateToken(token, validationParameters, out var validatedToken);

                // Token başarılı bir şekilde doğrulandıysa
                return validatedToken is JwtSecurityToken;
            }
            catch (SecurityTokenException)
            {
                //token geçerli değil
                return false;
            }
        }
        public string GetUsernameFromJwtToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var jwtToken = tokenHandler.ReadJwtToken(token);

            var username = jwtToken.Claims
                .FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;

            return username;
        }
        public async Task<bool> ValidateUserAsync(UserAuthenticationInformationDto userInformation)
        {
            _user = await _userManager.FindByNameAsync(userInformation.UserName);
            var result = (_user != null &&
                await _userManager.CheckPasswordAsync(_user, userInformation.Password));

            return result;
        }
        private async Task<List<Claim>> GetClaims(User user)
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, user.UserName)
            };

            var roles = await _userManager.GetRolesAsync(user);

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            return claims;
        }
        private async Task<bool> ValidateJwtTokenAndApiKeys(string token, string apiKey)
        {
            bool tokenIsValid = ValidateJwtToken(token);  
            bool apiKeyIsValid = _apiKeyValidationService.ValidateApiKey(apiKey);

            return await Task.FromResult(tokenIsValid && apiKeyIsValid);  // Asenkron olarak dönüyoruz
        }
    }
}
