using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using HC2.Arcanastudio.Net.Client;

namespace HC2.Arcanastudio.Net.RequestSerializers
{
    internal class DeviceCommandSerializer : IRequestSerializer
    {
        #region Implementation of IPayloadSerializer

        public string Serialize(string method, object payload)
        {
            string s;

            using (var stream = new MemoryStream())
            {
                using (var writer = new Utf8JsonWriter(stream, new JsonWriterOptions { Indented = true }))
                {
                    writer.WriteStartObject();
                    writer.WriteString("Method", method);

                    writer.WriteStartArray("Params");

                    writer.WriteStartObject();
                    writer.WriteStartArray("Devices");

                    if (payload is List<DeviceCommand> commands)
                    {

                        foreach (var devicecommand in commands)
                        {
                            writer.WriteStartObject();
                            writer.WriteString("Uuid", devicecommand.DeviceId);
                            writer.WriteStartArray("Properties");

                            foreach (var command in devicecommand.Commands)
                            {
                                writer.WriteStartObject();
                                writer.WriteString(command.Key, command.Value);
                                writer.WriteEndObject();
                            }

                            writer.WriteEndArray();
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
