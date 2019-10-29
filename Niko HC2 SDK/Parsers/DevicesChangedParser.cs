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
                var tt = propertydefinition.EnumerateObject().ToList();
                pd.Name = tt[0].Name;

                foreach (var pro in tt[0].Value.EnumerateObject())
                {
                    switch (pro.Name)
                    {
                        case "HasStatus":
                            pd.HasStatus = bool.Parse(pro.Value.ToString());
                            break;
                        case "CanControl":
                            pd.CanControl = bool.Parse(pro.Value.ToString());
                            break;
                        case "Description":
                            pd.Description = pro.Value.ToString();
                            if (pd.Description.StartsWith("Range"))
                            {
                                pd.ValueType = PropertyType.Range;
                                pd.Range = Range.FromString(pd.Description);
                            }

                            if (pd.Description.StartsWith("Choice"))
                            {
                                pd.ValueType = PropertyType.Choice;
                                var s = pd.Description.Split('(', ')');

                                pd.Choices = s[1].Split(',').ToList();
                            }

                            if (pd.Description.StartsWith("Boolean"))
                            {
                                pd.ValueType = PropertyType.Bool;
                            }


                            break;
                    }
                }

                list.Add(pd);
            }

            return list;
        }

        #endregion
    }
}
