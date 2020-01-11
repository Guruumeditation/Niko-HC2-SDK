using System;
using HC2.Arcanastudio.Net.Models.Interfaces;

namespace HC2.Arcanastudio.Net.Models
{
    public class PropertyStatus : IPropertyStatus
    {
        #region Implementation of IPropertyStatus

        public string Name { get; }
        public string Value { get; }

        #endregion

        public PropertyStatus(string name, string value)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Value = value;
        }
    }
}
