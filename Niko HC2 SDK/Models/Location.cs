using System.Text.Json.Serialization;
using HC2.Arcanastudio.Net.Models.Interfaces;

namespace HC2.Arcanastudio.Net.Models
{
    internal class Location : ILocation
    {
        public string Icon { get; set; }

        public string Name { get; set; }

        [JsonPropertyName("Index")]
        public string IndexRaw { get; set; }
        [JsonIgnore]
        public int Index => int.Parse(IndexRaw);

        [JsonPropertyName("Uuid")]
        public string Id { get; set; }
	}
}
