using System.Collections.Generic;

namespace HC2.Arcanastudio.Net.Models.Interfaces
{
    public interface IDevicePropertyDefinitions
    {
        string Id { get; }
        List<IPropertyDefinition> PropertyDefinitions { get; }
    }
}