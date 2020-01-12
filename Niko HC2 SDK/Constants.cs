using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("ClientTests")]
[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]
namespace HC2.Arcanastudio.Net
{
    public static class Constants
    {
        public static class Messages
        {
            public const string DevicesList = "devices.list";
            public const string DevicesControl = "devices.control";
            public const string LocationsList = "locations.list";
            public const string LocationsListItems = "locations.listitems";
            public const string DevicesAdded = "devices.added";
            public const string DevicesRemoved = "devices.removed";
            public const string DevicesDisplayNameChanged = "devices.displayname_change";
            public const string DevicesChanged = "devices.changed";
            public const string DevicesParamChanged = "devices.param_changed";
            public const string DevicesStatus = "devices.status";
            public const string TimePublished = "time.published";
            public const string SysteminfoPublish = "systeminfo.publish";
            public const string SysteminfoPublished = "systeminfo.published";
            public const string NotificationsList = "notification.list";
            public const string NotificationsRaised = "notifications.raised";
            public const string NotificationsUpdate = " notifications.update";
            public const string Error = " Error";
        }

        public static class Errors
        {
            public const string MessageParsingError = "MessageParsingError";
        }
    }
}
