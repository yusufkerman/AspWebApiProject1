using DotNetEnv;
using Microsoft.Extensions.Logging;
using Services.Contracts;

namespace Services
{
    public class ApiKeyValidationManager : IApiKeyValidationService
    {
        private readonly ILogger<ApiKeyValidationManager> _logger;
        public ApiKeyValidationManager(ILogger<ApiKeyValidationManager> logger)
        {
            _logger = logger;

            var envFilePath = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).FullName,
                "Services", "variables.env");
            Env.Load(envFilePath);
        }
        public bool ValidateApiKey(string apiKey)
        {
            var envApiKey = Environment.GetEnvironmentVariable("API_KEY");
            
            if (envApiKey == null)
                throw new NullReferenceException("API anahtarı ortam değişkenleri içerisinde bulunamadı.");

            return envApiKey.Equals(apiKey);
        }
    }
}
