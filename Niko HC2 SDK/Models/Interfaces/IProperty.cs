namespace HC2.Arcanastudio.Net.Models.Interfaces
{
    public interface IProperty : IPropertyStatus
    {
        IPropertyDefinition Definition { get; }
    }
}
