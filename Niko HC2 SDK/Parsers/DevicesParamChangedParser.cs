using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using HC2.Arcanastudio.Net.Models;
using HC2.Arcanastudio.Net.Models.Interfaces;

namespace HC2.Arcanastudio.Net.Parsers
{
    internal class DevicesParamChangedParser : IPayloadParser
    {
        #region Implementation of IPayloadParser

        public List<object> Parse(JsonElement payload)
        {
            var devicelist = new List<DeviceParameters>();

            var jsondevicelist = payload.EnumerateArray().FirstOrDefault().GetProperty("Devices");
            foreach (var device in jsondevicelist.EnumerateArray())
            {
                devicelist.Add(ParseDevice(device));
            }

            return devicelist.ToList<object>();
        }

        private DeviceParameters ParseDevice(JsonElement element)
        {
            var propertydefinitions = ParseParameter(element.GetProperty("Parameters"));

            var devicepropertydefinition = new DeviceParameters(element.GetProperty("Uuid").GetString(), propertydefinitions);

            return devicepropertydefinition;
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

        #endregion
    }
}
