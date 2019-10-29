using System;
using System.Text.Json.Serialization;

namespace HC2.Arcanastudio.Net.Client.Messages
{
    internal class NikoMessage : INikoMessage
    {
        [JsonIgnore]
        public Guid CorrelationData { get; set; }

        [JsonIgnore]
        public NikoMessageType MessageType { get; set; }

        public string Method { get; set; }

        public object Params { get; set; }

    }
}
