using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;

namespace ITI.Log
{

    public class LogReceiver
    {
        readonly BlockingCollection<LogMessage> _queue;
        readonly string _internalLogPath;
        readonly object _startLock;
        static readonly LogMessage _endMessage = new LogMessage( "Pouf" );
        Thread _thread;
        IReadOnlyList<ILogHandler> _handlers;


        public LogReceiver( string internalLogPath )
        {
            _internalLogPath = CheckValidInternalLogPath( internalLogPath);
            _queue = new BlockingCollection<LogMessage>();
            _startLock = new object();
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

        ILogHandler FromConfig( ILogHandlerConfig config )
        {
            // 2 cents trick (and one line).
            return (ILogHandler)Activator.CreateInstance(
                                            Type.GetType( config.GetType()
                                                   .AssemblyQualifiedName
                                                   .Replace( "Config,", "," ), throwOnError: true ),
                                            config.Clone() );
        }

        public void Start()
        {
            lock( _startLock )
            {
                if( _thread == null )
                {
                    _thread = new Thread( DispatchMessages );
                    _thread.Name = "LogReceiver thread.";
                    _thread.IsBackground = true;
                    _thread.Start();
                }
            }
        }

        public bool IsRunning => _thread != null;

        public void Configure( LogReceiverConfig config, bool waitForApplication = false )
        {
            _handlers = config.Configs
                              .Select( c => FromConfig( c ) )
                              .ToList();

        }

        void DispatchMessages()
        {
            for( ; ; )
            {
                var m = _queue.Take();
                if( m == _endMessage ) break;
                foreach( var h in _handlers )
                {
                    h.Handle( m );
                }
            }
        }

        public void Stop()
        {
            SendLog( _endMessage );
        }

        public void SendLog(LogMessage m)
        {
            _queue.Add(m);
        }

        public void SendLog(string text) => SendLog(new LogMessage(text));

    }
}
