using MQTTModule.Contracts;
using MQTTnet;
using MQTTnet.Server;
using System.Diagnostics;
using System.Net;
using Microsoft.Extensions.Logging;

namespace MQTTModule
{
    public class MQTTServerHandler : IMQTTServer
    {
        public delegate Task<bool> ValidateCredintialsDelegate(string username, string password);

        public event ValidateCredintialsDelegate OnValidateCredintials;

        private readonly ILogger<MQTTServerHandler> _logger;

        private MqttServer _server;
        public MQTTServerHandler(ILogger<MQTTServerHandler> logger)
        {
            _logger = logger;
        }
        public async Task StartServer()
        {
            var ipAddress = IPAddress.Parse("0.0.0.0");

            var optionsBuilder = new MqttServerOptionsBuilder()
                .WithDefaultEndpointBoundIPAddress(ipAddress)
                .WithDefaultEndpointPort(1883)
                .WithDefaultEndpoint();

            _server = new MqttFactory().CreateMqttServer(optionsBuilder.Build());

            _server.ValidatingConnectionAsync += ValidateCredintials;

            await _server.StartAsync();
        }

        public async Task StopServer()
        {
            await _server.StopAsync();
        }
        private async Task ValidateCredintials(ValidatingConnectionEventArgs e)
        {
            string userName = e.UserName;
            string password = e.Password;

            bool? result = OnValidateCredintials?.Invoke(userName,password).Result;

            if (result != null)
            {
                e.ReasonCode = result == true ? MQTTnet.Protocol.MqttConnectReasonCode.Success :
                    MQTTnet.Protocol.MqttConnectReasonCode.BadUserNameOrPassword;
            }
            else
            {
                throw new Exception("Credintial validation is failed, something wrong");
            }
        }
    }
}
