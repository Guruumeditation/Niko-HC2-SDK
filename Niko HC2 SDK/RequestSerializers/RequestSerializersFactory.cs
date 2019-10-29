using System.Collections.Generic;

namespace HC2.Arcanastudio.Net.RequestSerializers
{
    internal class RequestSerializersFactory
    {
        private static readonly Dictionary<string, IRequestSerializer> _parsers = new Dictionary<string, IRequestSerializer>();

        static RequestSerializersFactory()
        {
            _parsers.Add(Constants.Messages.DevicesList, new EmptyPayloadSerializer());
            _parsers.Add(Constants.Messages.DevicesControl, new DeviceCommandSerializer());
            _parsers.Add(Constants.Messages.LocationsList, new EmptyPayloadSerializer());
            _parsers.Add(Constants.Messages.LocationsListItems, new LocationsListItemsSerializer());
            _parsers.Add(Constants.Messages.SysteminfoPublish, new EmptyPayloadSerializer());
            _parsers.Add(Constants.Messages.NotificationsList, new EmptyPayloadSerializer());
            _parsers.Add(Constants.Messages.NotificationsUpdate, new NotificationsUpdateSerializer());

        }

        public static IRequestSerializer GetParser(string parser)
        {
            return _parsers[parser];
        }
    }
}
