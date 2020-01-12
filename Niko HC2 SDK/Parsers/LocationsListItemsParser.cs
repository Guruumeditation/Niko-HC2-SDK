using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using HC2.Arcanastudio.Net.Models;

namespace HC2.Arcanastudio.Net.Parsers
{
    internal class LocationsListItemsParser : IPayloadParser
    {
        #region Implementation of IPayloadParser

        public List<object> Parse(JsonElement payload)
        {
            var jsonlocationlist = payload.EnumerateArray().FirstOrDefault().GetProperty("Locations");

            var list = new List<LocationItemsCollection>();

            foreach (var jsonlocation in jsonlocationlist.EnumerateArray())
            {
                var location = JsonSerializer.Deserialize<LocationItemsCollection>(jsonlocation.GetRawText());

                location.Items.AddRange(JsonSerializer.Deserialize<IEnumerable<LocationItem>>(jsonlocation.GetProperty("Items").GetRawText()));

                list.Add(location);
            }

            return list.ToList<object>();
        }

        #endregion
    }
}
