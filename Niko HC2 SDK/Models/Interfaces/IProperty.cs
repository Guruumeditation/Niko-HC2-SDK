namespace HC2.Arcanastudio.Net.Models.Interfaces
{
    public interface IProperty
    {
        string Name { get; }

        string Value { get; }

        IPropertyDefinition Definition { get; }
    }
}
