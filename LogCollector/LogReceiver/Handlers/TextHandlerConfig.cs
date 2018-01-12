using System;
using System.Collections.Generic;
using System.Text;

namespace ITI.Log.Handlers
{
    public class TextHandlerConfig : ILogHandlerConfig
    {
        public string Path { get; set; }

        public ILogHandlerConfig Clone()
        {
            return new TextHandlerConfig() { Path = Path };
        }
    }
}
