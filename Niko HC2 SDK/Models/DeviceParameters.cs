using System.Collections.Generic;
using HC2.Arcanastudio.Net.Models.Interfaces;

namespace HC2.Arcanastudio.Net.Models
{
    internal class DeviceParameters : IDeviceParameters
    {
        public string Id { get; }

        public List<IParameter> Parameters { get; }

        public DeviceParameters(string id, List<IParameter> parameters)
        {
            Id = id;
            Parameters = parameters;
        }
    }
}
