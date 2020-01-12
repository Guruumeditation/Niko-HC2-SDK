using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;

namespace HC2.Arcanastudio.Net.RequestSerializers
{
    internal class NotificationsUpdateSerializer : IRequestSerializer
    {
        #region Implementation of IPayloadSerializer

        public string Serialize(string method, object payload)
        {
            string s = null;

            using (var stream = new MemoryStream())
            {
                using (var writer = new Utf8JsonWriter(stream, new JsonWriterOptions { Indented = true }))
                {
                    writer.WriteStartObject();
                    writer.WriteString("Method", method);

                    writer.WriteStartArray("Params");

                    writer.WriteStartObject();
                    writer.WriteStartArray("Notifications");
                    if (payload is List<string> idlist)
                    {

                        foreach (var id in idlist)
                        {
                            writer.WriteStartObject();
                            writer.WriteString("Uuid", id);
                            writer.WriteString("Status", "read");
                            writer.WriteEndObject();
                        }
                    }

                    writer.WriteEndArray();
                    writer.WriteEndObject();

                    writer.WriteEndArray();
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
