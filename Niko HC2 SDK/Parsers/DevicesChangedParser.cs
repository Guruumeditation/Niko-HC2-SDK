using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using HC2.Arcanastudio.Net.Models;
using HC2.Arcanastudio.Net.Models.Interfaces;

namespace HC2.Arcanastudio.Net.Parsers
{
    internal class DevicesChangedParser : IPayloadParser
    {
        #region Implementation of IPayloadParser

        public List<object> Parse(JsonElement payload)
        {
            var devicelist = new List<DevicePropertyDefinitions>();

            var jsondevicelist = payload.EnumerateArray().FirstOrDefault().GetProperty("Devices");
            foreach (var device in jsondevicelist.EnumerateArray())
            {
                devicelist.Add(ParseDevice(device));
            }

            return devicelist.ToList<object>();
        }

        private DevicePropertyDefinitions ParseDevice(JsonElement element)
        {
            var propertydefinitions = ParsePropertyDefinitions(element.GetProperty("PropertyDefinitions"));

            var devicepropertydefinition = new DevicePropertyDefinitions(element.GetProperty("Uuid").GetString(), propertydefinitions);

       

            return devicepropertydefinition;
        }

        private List<IPropertyDefinition> ParsePropertyDefinitions(JsonElement propertydefinitions)
        {
            var parser = new PropertyDefinitionParser();

            var list = parser.Parse(propertydefinitions);

            return list.Cast<IPropertyDefinition>().ToList();
        }

        #endregion
    }
}
