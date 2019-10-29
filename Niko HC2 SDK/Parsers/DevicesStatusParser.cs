using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using HC2.Arcanastudio.Net.Models;

namespace HC2.Arcanastudio.Net.Parsers
{
    internal class DevicesStatusParser : IPayloadParser
    {
        #region Implementation of IPayloadParser

        public List<object> Parse(JsonElement payload)
        {
            var devicelist = new List<DeviceProperties>();

            var jsondevicelist = payload.EnumerateArray().FirstOrDefault().GetProperty("Devices");
            foreach (var device in jsondevicelist.EnumerateArray())
            {
                devicelist.Add(ParseDevice(device));
            }

            return devicelist.ToList<object>();
        }

        private DeviceProperties ParseDevice(JsonElement element)
        {
            var propertydefinitions = ParseProperties(element.GetProperty("Properties"));

            var devicepropertydefinition = new DeviceProperties(element.GetProperty("Uuid").GetString(), propertydefinitions);

            return devicepropertydefinition;
        }

        private List<KeyValuePair<string,string>> ParseProperties(JsonElement parameters)
        {
            var reee = JsonSerializer.Deserialize<IEnumerable<Dictionary<string, string>>>(parameters.GetRawText());

            return  reee.SelectMany(d => d).ToList();
        }

        #endregion
    }
}
