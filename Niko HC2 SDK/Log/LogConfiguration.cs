using System;
using LogLevel = HC2.Arcanastudio.Net.Log.LogLevel;

namespace HC2.Arcanastudio.Net.Log
{
    public class LogConfiguration
    {
        internal bool IsWriteToConsole { get; private set; }
        internal bool IsWriteToFile { get; private set; }
        internal string FileName { get; private set; }
        internal LogLevel LogLevel { get; private set; } = LogLevel.Error;

        public LogConfiguration SetLevel(LogLevel level)
        {
            LogLevel = level;
            return this;
        }

        public LogConfiguration WriteToConsole()
        {
            IsWriteToConsole = true;
            return this;
        }

        public LogConfiguration WriteToFile(string filename)
        {
            FileName = filename ?? throw new ArgumentNullException(nameof(filename));
            IsWriteToFile = true;
            return this;
        }
    }
}
