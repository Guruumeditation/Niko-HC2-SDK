using HC2.Arcanastudio.Net.Models.Interfaces;

namespace HC2.Arcanastudio.Net.Models
{
    internal class Property : IProperty
    {
        #region Implementation of IProperty

        public string Name { get; set; }
        public string Value { get; set; }
        public IPropertyDefinition Definition { get; set; }

        #endregion
    }
}
