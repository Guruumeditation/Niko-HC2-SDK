using System;
using System.Globalization;
using System.Text.Json.Serialization;
using HC2.Arcanastudio.Net.Models.Interfaces;

namespace HC2.Arcanastudio.Net.Models
{
    internal class Notification : INotification
    {
        private DateTime _utcTime;

        [JsonPropertyName("Uuid")]
        public string Id { get; set; }

        [JsonPropertyName("TimeOccurred")]
        public string TimeOccurred
        {
            set => DateTime.TryParseExact(value, "yyyyMMddHHmmss", CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal, out _utcTime);
        }

        public DateTime Time => _utcTime;

        public string Type
        {
            set => NotificationType = (NotificationType) Enum.Parse(typeof(NotificationType), value, true);
        }

        public string Status
        {
            set => NotificationStatus = (Status)Enum.Parse(typeof(Status), value, true);
        }

        public NotificationType NotificationType { get; set; }
        public Status NotificationStatus { get; set; }

        public string Text { get; set; }
    }

    public enum NotificationType
    {
        Alarm,
        Notification
    }

    public enum Status
    {
        New,
        Read
    }
}
