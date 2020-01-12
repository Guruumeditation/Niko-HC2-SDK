using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using HC2.Arcanastudio.Net.Models;

namespace HC2.Arcanastudio.Net.Parsers
{
    internal class SystemInfoParser : IPayloadParser
    {
        #region Implementation of IPayloadParser

        public List<object> Parse(JsonElement payload)
        {
            var jsonlocationlist = payload.EnumerateArray().FirstOrDefault().GetProperty("SystemInfo");

           var list = JsonSerializer.Deserialize<IEnumerable<SystemInfo>>(jsonlocationlist.GetRawText()).ToList();

           var sysinfo = list.First();

            sysinfo.Version = JsonSerializer.Deserialize<IEnumerable<Dictionary<string, string>>>(sysinfo.SWversions.GetRawText()).SelectMany(d => d).ToList();

            return list.ToList<object>();
        }

        #endregion
    }
}
