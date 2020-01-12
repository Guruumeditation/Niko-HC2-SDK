using HC2.Arcanastudio.Net.Models.Interfaces;

namespace HC2.Arcanastudio.Net.Models
{
    internal class Parameter : IParameter
    {
        #region Implementation of IParameter

        public string Name { get; set; }
        public string Value { get; set;  }

        #endregion
    }
}
