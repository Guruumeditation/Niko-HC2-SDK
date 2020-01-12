using HC2.Arcanastudio.Net.Models.Interfaces;

namespace HC2.Arcanastudio.Net.Models
{
    internal class DeviceDisplayName : IDeviceDisplayName
    {
        public string Id { get; }
        public string Name { get; }

        public DeviceDisplayName(string id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
