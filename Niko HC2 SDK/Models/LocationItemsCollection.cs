using System.Collections.Generic;
using System.Text.Json.Serialization;
using HC2.Arcanastudio.Net.Models.Interfaces;

namespace HC2.Arcanastudio.Net.Models
{
    internal class LocationItemsCollection : ILocationItemsCollection
    {
        [JsonPropertyName("Uuid")]
        public string Id { get; set; }
        [JsonIgnore]
        public List<ILocationItem> Items { get; set; }

        public LocationItemsCollection()
        {
            Items = new List<ILocationItem>();
        }
    }
}
