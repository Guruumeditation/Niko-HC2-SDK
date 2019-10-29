using System;

namespace HC2.Arcanastudio.Net.Models.Interfaces
{
    public interface ITimeInfo
    {
        string GMTOffset { get; set; }
        string Timezone { get; set; }
        DateTime UtcTime { get; }
        bool IsDst { get; }
    }
}