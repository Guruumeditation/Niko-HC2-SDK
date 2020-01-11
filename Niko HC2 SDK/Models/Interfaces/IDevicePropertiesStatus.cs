using System.Collections.Generic;

namespace HC2.Arcanastudio.Net.Models.Interfaces
{
    public interface IDevicePropertiesStatus
    {
        string Id { get; }
        List<IPropertyStatus> Properties { get; }
    }
}