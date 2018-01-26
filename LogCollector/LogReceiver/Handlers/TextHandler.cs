using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ITI.Log.Handlers
{
    internal class TextHandler : ILogHandler
    {
        readonly TextHandlerConfig _config;

        public TextHandler( TextHandlerConfig config )
        {
            _config = config;
        }

        public void Handle( LogMessage m )
        {
            File.AppendAllText( _config.Path, $"{m.LogTime} -{m.Text}" );
        }

        public void Dispose()
        {
        }
    }
}
