using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using HC2.Arcanastudio.Net.Models;

namespace HC2.Arcanastudio.Net.Parsers
{
    internal class NotificationListParser : IPayloadParser
    {
        #region Implementation of IPayloadParser

        public List<object> Parse(JsonElement payload)
        {
            var notificationlist = payload.EnumerateArray().FirstOrDefault().GetProperty("Notifications");

            var notifications = JsonSerializer.Deserialize<IEnumerable<Notification>>(notificationlist.GetRawText());

            return notifications.ToList<object>();
        }

        #endregion
    }
}
