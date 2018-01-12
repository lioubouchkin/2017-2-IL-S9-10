using FluentAssertions;
using NUnit.Framework;
using System;
using System.IO;
using System.Runtime.CompilerServices;

namespace ITI.Log.Tests
{
    [TestFixture]
    public class LogReceiverSimpeTests
    {
        static string GetThisFile( [CallerFilePath]string p = null ) => p;
        static string InternalLogPath => Path.Combine( Path.GetDirectoryName( GetThisFile() ), "InternalLogPath" );

        [Test]
        public void sending_logs_to_a_text_file()
        {
            LogReceiver r = new LogReceiver( InternalLogPath );
            r.Invoking( sut => sut.Start() ).ShouldNotThrow();
        }

    }
}
