using System.Collections.Generic;

namespace HC2.Arcanastudio.Net.Models.Interfaces
{
    public interface IDevice
    {
        string Name { get; }
        string Technology { get; }
        string Id { get; }
        string Identifier { get; }
        List<IProperty> Properties { get; }
        bool Online { get; }
        string Model { get; }
        List<ITrait> Traits { get; }
        string Type { get; }
        List<IParameter> Parameters { get; }

        void UpdateProperties(List<IPropertyStatus> propertiesstatus);
    }
}
