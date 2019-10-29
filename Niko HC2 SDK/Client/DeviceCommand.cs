using System.Collections.Generic;
using HC2.Arcanastudio.Net.Models.Interfaces;

namespace HC2.Arcanastudio.Net.Client
{
    public class DeviceCommand : IDeviceCommand
    {
        public string DeviceId { get; }

        public Dictionary<string,string> Commands { get; }

        public DeviceCommand(string deviceId, Dictionary<string, string> commands)
        {
            DeviceId = deviceId;
            Commands = commands;
        }
    }
}
