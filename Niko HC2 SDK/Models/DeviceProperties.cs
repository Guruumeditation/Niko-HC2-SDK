using System.Collections.Generic;
using HC2.Arcanastudio.Net.Models.Interfaces;

namespace HC2.Arcanastudio.Net.Models
{
    internal class DeviceProperties : IDeviceProperties
    {
        public string Id { get; }

        public List<KeyValuePair<string, string>> Properties { get; }

        public DeviceProperties(string id, List<KeyValuePair<string, string>> properties)
        {
            Id = id;
            Properties = properties;
        }
    }
}
