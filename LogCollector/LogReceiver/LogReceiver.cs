using System;
using System.Collections.Concurrent;
using System.IO;

namespace ITI.Log
{
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
