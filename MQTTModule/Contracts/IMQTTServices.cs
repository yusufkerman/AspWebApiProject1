using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQTTModule.Contracts
{
    public interface IMQTTServices
    {
        public IMQTTServer MQTTServer { get; }
    }
}
