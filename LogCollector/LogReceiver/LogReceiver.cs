using System;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;

namespace ITI.Log
{

    interface I<T> where T : I<T>
    {
        T Clone();
    }

    class X : I<X>
    {
        public X Clone()
        {
            throw new NotImplementedException();
        }
    }









    public class LogReceiver
    {
        readonly BlockingCollection<LogMessage> _queue;
        readonly string _internalLogPath;

        public LogReceiver( string internalLogPath )
        {
            _internalLogPath = CheckValidInternalLogPath( internalLogPath);
            _queue = new BlockingCollection<LogMessage>();
        }

        private static string CheckValidInternalLogPath(string internalLogPath)
        {
            internalLogPath = Path.GetFullPath(internalLogPath);
            Directory.CreateDirectory(internalLogPath);
            var testPath = Path.Combine(internalLogPath, "Test.txt");
            File.WriteAllText(testPath, "test");
            File.Delete(testPath);
            return testPath;
        }

        public void Configure( LogReceiverConfig config, bool waitForApplication = false )
        {
            var allHandlers = config.Configs.Select( c => FromConfig( c ) ).ToList();

        }

        ILogHandler FromConfig( ILogHandlerConfig config )
        {
            // 2 cents trick...
            string typeHandlerName = config.GetType()
                                        .AssemblyQualifiedName
                                        .Replace( "Config,", "," );
            Type typeHandler = Type.GetType( typeHandlerName, throwOnError: true );
            object handler = Activator.CreateInstance( typeHandler, config.Clone() );
            return (ILogHandler)handler;
        }

        public void Start()
        {

        }

        public void Stop()
        {
        }

        public void SendLog(LogMessage m)
        {
            _queue.Add(m);
        }

        public void SendLog(string text) => SendLog(new LogMessage(text));

    }
}
