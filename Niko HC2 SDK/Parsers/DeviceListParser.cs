using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using HC2.Arcanastudio.Net.Models;
using HC2.Arcanastudio.Net.Models.Interfaces;

namespace HC2.Arcanastudio.Net.Parsers
{
    internal class DeviceListParser : IPayloadParser
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
            var device = JsonSerializer.Deserialize<Device>(element.GetRawText());

            var propertydefinitions = ParsePropertyDefinitions(element.GetProperty("PropertyDefinitions"));

            var properties = ParseProperty(element.GetProperty("Properties"));

            foreach (var property in properties)
            {
                property.Definition = propertydefinitions[property.Name];
            }

            device.Properties.AddRange(properties);

            device.Parameters.AddRange(ParseParameter(element.GetProperty("Parameters")));

            device.Traits.AddRange(ParseTraits(element.GetProperty("Traits")));

            return device;
        }

        private Dictionary<string, IPropertyDefinition> ParsePropertyDefinitions(JsonElement propertydefinitions)
        {
            var dict = new Dictionary<string, IPropertyDefinition>();

            var parser = new PropertyDefinitionParser();

            var list = parser.Parse(propertydefinitions).Cast<IPropertyDefinition>().ToList();

            foreach (var pd in list)
            {

                dict.Add(pd.Name, pd);
            }

            return dict;
        }

        private List<Property> ParseProperty(JsonElement properties)
        {
            var list = new List<Property>();

            foreach (var property in properties.EnumerateArray())
            {
                var values = property.EnumerateObject().ToList();

                var p = new Property { Name = values[0].Name, Value = values[0].Value.GetString() };

                list.Add(p);
            }
            return list;
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
