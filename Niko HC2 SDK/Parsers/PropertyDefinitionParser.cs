using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using HC2.Arcanastudio.Net.Models;
using HC2.Arcanastudio.Net.Models.Interfaces;

namespace HC2.Arcanastudio.Net.Parsers
{
    internal class PropertyDefinitionParser : IPayloadParser
    {
        #region Implementation of IPayloadParser

        public List<object> Parse(JsonElement payload)
        {
            var list = new List<IPropertyDefinition>();

            foreach (var propertydefinition in payload.EnumerateArray())
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

            return list.ToList<object>();
        }

        #endregion
    }
}
