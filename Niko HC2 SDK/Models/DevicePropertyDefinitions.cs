using System.Collections.Generic;
using HC2.Arcanastudio.Net.Models.Interfaces;

namespace HC2.Arcanastudio.Net.Models
{
    internal class DevicePropertyDefinitions : IDevicePropertyDefinitions
    {
        public string Id { get; }

        public List<IPropertyDefinition> PropertyDefinitions { get; }

        public DevicePropertyDefinitions(string id, List<IPropertyDefinition> propertyDefinitions)
        {
            Id = id;
            PropertyDefinitions = propertyDefinitions;
        }
    }
}
