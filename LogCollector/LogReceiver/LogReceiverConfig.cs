using System;
using System.Collections.Generic;
using System.Text;

namespace ITI.Log
{
    public class LogReceiverConfig
    {
        public List<ILogHandlerConfig> Configs { get; } = new List<ILogHandlerConfig>();

    }
}
