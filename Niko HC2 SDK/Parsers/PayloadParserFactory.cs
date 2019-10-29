using System.Collections.Generic;

namespace HC2.Arcanastudio.Net.Parsers
{
    internal class PayloadParserFactory
    {
        private static readonly Dictionary<string,IPayloadParser> _parsers = new Dictionary<string, IPayloadParser>();

        static PayloadParserFactory()
        {
            _parsers.Add(Constants.Messages.DevicesList,new DeviceListParser());
            _parsers.Add(Constants.Messages.DevicesAdded,new DeviceAddedParser());
            _parsers.Add(Constants.Messages.DevicesRemoved,new DeviceRemovedParser());
            _parsers.Add(Constants.Messages.DevicesDisplayNameChanged,new DeviceDisplayNameChangedParser());
            _parsers.Add(Constants.Messages.DevicesChanged,new DevicesChangedParser());
            _parsers.Add(Constants.Messages.DevicesParamChanged,new DevicesParamChangedParser());
            _parsers.Add(Constants.Messages.DevicesStatus,new DevicesStatusParser());

            _parsers.Add(Constants.Messages.LocationsList,new LocationsListParser());
            _parsers.Add(Constants.Messages.LocationsListItems, new LocationsListItemsParser());

            _parsers.Add(Constants.Messages.TimePublished,new TimePublishedParser());

            _parsers.Add(Constants.Messages.SysteminfoPublish,new SystemInfoParser());
            _parsers.Add(Constants.Messages.SysteminfoPublished, new SystemInfoParser());

            _parsers.Add(Constants.Messages.NotificationsList,new NotificationListParser());
            _parsers.Add(Constants.Messages.NotificationsUpdate, new NotificationListParser());
            _parsers.Add(Constants.Messages.NotificationsRaised,new NotificationsRaisedParser());

            _parsers.Add(Constants.Messages.Error,new ErrorParser());
        }

        public static IPayloadParser GetParser(string parser)
        {
            return _parsers[parser];
        }
    }
}
