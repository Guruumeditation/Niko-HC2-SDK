using System.Collections.Generic;
using HC2.Arcanastudio.Net.Models.Interfaces;

namespace HC2.Arcanastudio.Net.Models
{
    internal class PropertyDefinition : IPropertyDefinition
    {
        #region Implementation of IPropertyDefinition

        public string Name { get; set; }
        public string Description { get; set; }
        public bool HasStatus { get; set; }
        public bool CanControl { get; set; }
        public PropertyType ValueType { get; set; }
        public IRange Range { get; set; }
        public List<string> Choices { get; set; }

        #endregion
    }
}
