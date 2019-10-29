using System.Collections.Generic;

namespace HC2.Arcanastudio.Net.Models.Interfaces
{
    public interface IDeviceProperties
    {
        string Id { get; }
        List<KeyValuePair<string, string>> Properties { get; }
    }
}