using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using HC2.Arcanastudio.Net.Models;

namespace HC2.Arcanastudio.Net.Parsers
{
    internal class TimePublishedParser : IPayloadParser
    {
        #region Implementation of IPayloadParser

        public List<object> Parse(JsonElement payload)
        {
            var jsonlocationlist = payload.EnumerateArray().FirstOrDefault().GetProperty("TimeInfo");

            var list = JsonSerializer.Deserialize<IEnumerable<TimeInfo>>(jsonlocationlist.GetRawText());

            return list.ToList<object>();
        }

        #endregion
    }
}
