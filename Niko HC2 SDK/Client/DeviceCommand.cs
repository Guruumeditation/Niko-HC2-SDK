using System.Collections.Generic;
using HC2.Arcanastudio.Net.Models;
using HC2.Arcanastudio.Net.Models.Interfaces;

namespace HC2.Arcanastudio.Net.Client
{
    public class DeviceCommand : IDeviceCommand
    {
        public string DeviceId { get; }

        public List<PropertyStatus> Commands { get; }

        public DeviceCommand(string deviceId, List<PropertyStatus> commands)
        {
            DeviceId = deviceId;
            Commands = commands;
        }
    }
}
