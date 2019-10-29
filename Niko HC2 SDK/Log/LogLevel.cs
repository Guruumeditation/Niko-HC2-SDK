using System;

namespace HC2.Arcanastudio.Net.Log
{
    [Flags]
    public enum LogLevel
    {
        Error = 1,
        ReceivedRaw = 2,
        Received = 4,
        Info = 8,
        All = 15
    }
}
 