using System;

namespace HC2.Arcanastudio.Net.Models.Interfaces
{
    public interface INotification
    {
        string Id { get; set; }
        DateTime Time { get; }
        NotificationTypeEnum NotificationType { get; set; }
        StatusEnum NotificationStatus { get; set; }
        string Text { get; set; }
    }
}