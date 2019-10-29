using HC2.Arcanastudio.Net.Models.Interfaces;

namespace HC2.Arcanastudio.Net.Models
{
    internal class Trait : ITrait
    {
        #region Implementation of ITrait

        public string Name { get; set; }
        public string Value { get; set; }

        #endregion
    }
}
