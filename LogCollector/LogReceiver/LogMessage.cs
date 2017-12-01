using System;
using System.Collections.Generic;
using System.Text;

namespace ITI.Log
{
    public class LogMessage
    {
        public LogMessage( string text )
        {
            Text = text;
            LogTime = DateTime.UtcNow;
        }

        public string Text { get; }

        public DateTime LogTime { get; }
    }
}
