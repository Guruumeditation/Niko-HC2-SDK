using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using HC2.Arcanastudio.Net.Models;
using HC2.Arcanastudio.Net.Models.Interfaces;

namespace HC2.Arcanastudio.Net.Parsers
{
    internal class DevicesStatusParser : IPayloadParser
    {
        #region Implementation of IPayloadParser

        public List<object> Parse(JsonElement payload)
        {
            var devicelist = new List<DevicePropertiesStatus>();

            var jsondevicelist = payload.EnumerateArray().FirstOrDefault().GetProperty("Devices");
            foreach (var device in jsondevicelist.EnumerateArray())
            {
                devicelist.Add(ParseDevice(device));
            }

            return devicelist.ToList<object>();
        }

        private DevicePropertiesStatus ParseDevice(JsonElement element)
        {
            var propertydefinitions = ParseProperties(element.GetProperty("Properties"));

            var devicepropertydefinition = new DevicePropertiesStatus(element.GetProperty("Uuid").GetString(), propertydefinitions);

            return devicepropertydefinition;
        }

        private List<IPropertyStatus> ParseProperties(JsonElement parameters)
        {
            var list = new List<IPropertyStatus>();

            foreach (var element in parameters.EnumerateArray())
            {
                var obj = element.EnumerateObject().First();
                list.Add(new PropertyStatus(obj.Name,obj.Value.ToString()));
            }

            return  list;
        }

        #endregion
    }
}
