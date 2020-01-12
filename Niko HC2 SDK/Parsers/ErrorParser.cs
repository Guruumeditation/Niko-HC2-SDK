using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using HC2.Arcanastudio.Net.Models;
using HC2.Arcanastudio.Net.Models.Interfaces;

namespace HC2.Arcanastudio.Net.Parsers
{
    internal class ErrorParser : IPayloadParser
    {
        #region Implementation of IPayloadParser

        public List<object> Parse(JsonElement payload)
        {
            var list = new List<IError>();

            var error = new ResponseError(payload.GetProperty("ErrCode").GetString(), payload.GetProperty("ErrMessage").GetString(), "");

            list.Add(error);

            return list.ToList<object>();
        }

        #endregion
    }
}
