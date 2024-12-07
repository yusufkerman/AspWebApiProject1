using MQTTModule.Contracts;

namespace MQTTModule
{
    public class MQTTServiceManager : IMQTTServices
    {
        public IMQTTServer MQTTServer => _mqttServer;

        private IMQTTServer _mqttServer;

        public MQTTServiceManager(IMQTTServer mqttServer)
        {
            _mqttServer = mqttServer;
        }
    }
}
