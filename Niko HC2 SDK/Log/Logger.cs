using System;
using HC2.Arcanastudio.Net.Extensions;
using Serilog;
using Serilog.Events;

namespace HC2.Arcanastudio.Net.Log
{
    internal static class Logger
    {
        private static LogConfiguration _logConfiguration;
        public static LogConfiguration Configuration
        {
            set
            {
                _logConfiguration = value;

                var serilogconfig = new LoggerConfiguration();
                if (_logConfiguration.IsWriteToConsole)
                    serilogconfig = serilogconfig.WriteTo.Console();

                if (_logConfiguration.IsWriteToFile)
                    serilogconfig = serilogconfig.WriteTo.File(_logConfiguration.FileName);

                serilogconfig = serilogconfig.MinimumLevel.Debug();

                Serilog.Log.Logger = serilogconfig.CreateLogger();
            }
        }

        public static bool IsLog => _logConfiguration != null;

        public static void Write(string message)
        {
            Serilog.Log.Write(LogEventLevel.Debug, message);
        }

        public static void WriteError(string message)
        {
           if (_logConfiguration.LogLevel.CheckLevel(LogLevel.Error))
               Write(message);
        }

        public static void WriteReceivedRaw(string message)
        {
            if (_logConfiguration.LogLevel.CheckLevel(LogLevel.ReceivedRaw))
                Write(message);
        }

        public static void WriteInfo(string message)
        {
            if (_logConfiguration.LogLevel.CheckLevel(LogLevel.Info))
                Write(message);
        }

        public static void WriteReceived(string message)
        {
            if (_logConfiguration.LogLevel.CheckLevel(LogLevel.Received))
                Write(message);
        }

        public static void WriteAll(string message)
        {
            if (_logConfiguration.LogLevel.CheckLevel(LogLevel.All))
                Write(message);
        }

        public static void Write(Exception exception, string message = null)
        {
            Serilog.Log.Write(LogEventLevel.Debug, exception, message);
        }

        public static void WriteError(Exception exception, string message = null)
        {
            if (_logConfiguration.LogLevel.CheckLevel(LogLevel.Error))
                Write(exception, message);
        }

        public static void WriteReceivedRaw(Exception exception, string message = null)
        {
            if (_logConfiguration.LogLevel.CheckLevel(LogLevel.ReceivedRaw))
                Write(exception, message);
        }

        public static void WriteInfo(Exception exception, string message = null)
        {
            if (_logConfiguration.LogLevel.CheckLevel(LogLevel.Info))
                Write(exception, message);
        }

        public static void WriteReceived(Exception exception, string message = null)
        {
            if (_logConfiguration.LogLevel.CheckLevel(LogLevel.Received))
                Write(exception, message);
        }

        public static void WriteAll(Exception exception, string message = null)
        {
            if (_logConfiguration.LogLevel.CheckLevel(LogLevel.All))
                Write(exception, message);
        }
    }
}
