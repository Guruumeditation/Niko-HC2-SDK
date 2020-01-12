using System.Collections.Generic;
using HC2.Arcanastudio.Net.Models.Interfaces;

namespace HC2.Arcanastudio.Net.Models
{
    internal class DevicePropertiesStatus : IDevicePropertiesStatus
    {
        public string Id { get; }

        public List<IPropertyStatus> Properties { get; }

        public DevicePropertiesStatus(string id, List<IPropertyStatus> properties)
        {
            Id = id;
            Properties = properties;
        }
    }
}
