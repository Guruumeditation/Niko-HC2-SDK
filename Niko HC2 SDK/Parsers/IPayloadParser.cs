using System.Collections.Generic;
using System.Text.Json;

namespace HC2.Arcanastudio.Net.Parsers
{
    internal interface IPayloadParser
    {
        List<object> Parse(JsonElement payload);
    }
}
