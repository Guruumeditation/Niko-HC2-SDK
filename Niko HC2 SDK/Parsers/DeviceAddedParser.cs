using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using HC2.Arcanastudio.Net.Models;
using HC2.Arcanastudio.Net.Models.Interfaces;

namespace HC2.Arcanastudio.Net.Parsers
{
    internal class DeviceAddedParser : IPayloadParser
    {
        #region Implementation of IPayloadParser

        public List<object> Parse(JsonElement payload)
        {
            var devicelist = new List<Device>();

            var jsondevicelist = payload.EnumerateArray().FirstOrDefault().GetProperty("Devices");
            foreach (var device in jsondevicelist.EnumerateArray())
            {
                devicelist.Add(ParseDevice(device));
            }

            return devicelist.ToList<object>();
        }

        private Device ParseDevice(JsonElement element)
        {
            var device  = JsonSerializer.Deserialize<Device>(element.GetRawText());

            device.Parameters.AddRange(ParseParameter(element.GetProperty("Parameters")));

            device.Traits.AddRange(ParseTraits(element.GetProperty("Traits")));

            return device;
        }


        private List<IParameter> ParseParameter(JsonElement parameters)
        {
            var list = new List<IParameter>();

            foreach (var parameter in parameters.EnumerateArray())
            {
                var values = parameter.EnumerateObject().ToList();

                var p = new Parameter { Name = values[0].Name, Value = values[0].Value.GetString() };

                list.Add(p);
            }

            return list;
        }

        private List<ITrait> ParseTraits(JsonElement traits)
        {
            var list = new List<ITrait>();

            foreach (var trait in traits.EnumerateArray())
            {
                var values = trait.EnumerateObject().ToList();

                var p = new Trait { Name = values[0].Name, Value = values[0].Value.GetString() };

                list.Add(p);
            }

            return list;
        }
        #endregion
    }
}
