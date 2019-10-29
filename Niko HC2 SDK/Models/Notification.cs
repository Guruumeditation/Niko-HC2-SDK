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
            set => NotificationType = (NotificationTypeEnum) Enum.Parse(typeof(NotificationTypeEnum), value, true);
        }

        public string Status
        {
            set => NotificationStatus = (StatusEnum)Enum.Parse(typeof(StatusEnum), value, true);
        }

        public NotificationTypeEnum NotificationType { get; set; }
        public StatusEnum NotificationStatus { get; set; }

        public string Text { get; set; }
    }

    public enum NotificationTypeEnum
    {
        Alarm,
        Notification
    }

    public enum StatusEnum
    {
        New,
        Read
    }
}
