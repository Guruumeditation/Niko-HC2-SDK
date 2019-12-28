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
            var list = new List<IPropertyDefinition>();

            foreach (var propertydefinition in propertydefinitions.EnumerateArray())
            {
                var pd = new PropertyDefinition();
                var rawproperty = propertydefinition.EnumerateObject().First();
                pd.Name = rawproperty.Name;

                pd.HasStatus = bool.Parse(rawproperty.Value.GetProperty("HasStatus").GetString());
                pd.CanControl = bool.Parse(rawproperty.Value.GetProperty("CanControl").GetString());
                pd.Description = rawproperty.Value.GetProperty("Description").GetString();

                if (pd.Description.StartsWith("Range"))
                {
                    pd.ValueType = PropertyType.Range;
                    pd.Range = Range.FromString(pd.Description);
                }
                else
                if (pd.Description.StartsWith("Choice"))
                {
                    pd.ValueType = PropertyType.Choice;
                    var s = pd.Description.Split('(', ')');

                    pd.Choices = s[1].Split(',').ToList();
                }
                else
                    pd.ValueType = PropertyType.Bool;

                list.Add(pd);
            }

            return list;
        }

        #endregion
    }
}
