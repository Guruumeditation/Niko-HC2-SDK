using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using HC2.Arcanastudio.Net.Models;

namespace HC2.Arcanastudio.Net.Parsers
{
    internal class LocationsListParser : IPayloadParser
    {
        #region Implementation of IPayloadParser

        public List<object> Parse(JsonElement payload)
        {
            var jsonlocationlist = payload.EnumerateArray().FirstOrDefault().GetProperty("Locations");

            var list = JsonSerializer.Deserialize<IEnumerable<Location>>(jsonlocationlist.GetRawText());

            return list.ToList<object>();
        }
        #endregion
    }
}
