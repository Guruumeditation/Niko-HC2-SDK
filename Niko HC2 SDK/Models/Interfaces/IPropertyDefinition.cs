using System.Collections.Generic;

namespace HC2.Arcanastudio.Net.Models.Interfaces
{
    public interface IPropertyDefinition
    {
        string Description { get; }
        bool HasStatus { get; }
        bool CanControl { get; }
        PropertyType ValueType { get; }
        IRange Range { get; }
        List<string> Choices { get; }
    }

    public enum PropertyType
    {
        Bool,
        Choice,
        Range
    }
}
