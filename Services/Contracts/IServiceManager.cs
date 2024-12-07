using MQTTModule.Contracts;


namespace Services.Contracts
{
    public interface IServiceManager
    {
        IAuthenticationService AuthenticationService { get; }
        IUserService UserService { get; }
        IApiKeyValidationService ApiKeyValidationService { get; }
        IMQTTServices MQTTServices { get; }
    }
}
