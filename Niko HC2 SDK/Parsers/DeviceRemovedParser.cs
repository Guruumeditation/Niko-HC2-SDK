using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace HC2.Arcanastudio.Net.Parsers
{
    internal class DeviceRemovedParser : IPayloadParser
    {
        #region Implementation of IPayloadParser

        public List<object> Parse(JsonElement payload)
        {
            var devicesid = payload.EnumerateArray().First().GetProperty("Devices").EnumerateArray();

            var idlist = new List<string>();

            foreach (var item in devicesid)
            {
                idlist.Add(item.GetProperty("Uuid").GetString());
            }

            return idlist.ToList<object>();
        }

        #endregion
    }
}
