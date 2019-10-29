using System.IO;
using System.Text;
using System.Text.Json;

namespace HC2.Arcanastudio.Net.RequestSerializers
{
    internal class EmptyPayloadSerializer : IRequestSerializer
    {
        #region Implementation of IPayloadSerializer

        public string Serialize(string method, object payload)
        {
            string s;

            using (var stream = new MemoryStream())
            {
                using (var writer = new Utf8JsonWriter(stream, new JsonWriterOptions {Indented = true}))
                {
                    writer.WriteStartObject();
                    writer.WriteString("Method", method);

                    writer.WriteEndObject();

                    writer.Flush();

                    s = Encoding.UTF8.GetString(stream.ToArray());
                }
            }

            return s;
        }

        #endregion
    }
}
