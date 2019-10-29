using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using HC2.Arcanastudio.Net.Models;

namespace HC2.Arcanastudio.Net.Parsers
{
    internal class DeviceDisplayNameChangedParser : IPayloadParser
    {
        #region Implementation of IPayloadParser

        public List<object> Parse(JsonElement payload)
        {
            var devicesidt = payload.EnumerateArray().First().GetProperty("Devices").EnumerateArray();

            var list = new List<object>();

            foreach (var item in devicesidt)
            {
                list.Add(new DeviceDisplayName(item.GetProperty("Uuid").GetString(),item.GetProperty("DisplayName").GetString()));
            }

            return list;
        }

        #endregion
    }
}
