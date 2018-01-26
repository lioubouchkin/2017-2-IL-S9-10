using System;

namespace ITI.Log
{
    public interface ILogHandler : IDisposable
    {
        void Handle( LogMessage m );
    }
}
