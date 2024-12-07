using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MQTTModule.MQTTServerHandler;

namespace MQTTModule.Contracts
{
    public interface IMQTTServer
    {
        public Task StartServer();
        public Task StopServer();

        public event ValidateCredintialsDelegate OnValidateCredintials;
    }
}
