using System.Collections.Generic;

namespace HC2.Arcanastudio.Net.Models.Interfaces
{
    public interface IDeviceParameters
    {
        string Id { get; }
        List<IParameter> Parameters { get; }
    }
}