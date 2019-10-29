using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using HC2.Arcanastudio.Net;
using HC2.Arcanastudio.Net.Client;
using HC2.Arcanastudio.Net.Client.Messages;
using HC2.Arcanastudio.Net.Models;
using HC2.Arcanastudio.Net.Models.Interfaces;
using HC2.Arcanastudio.Net.Observable;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace ClientTests
{
    [TestClass]
    public class ResponseTests
    {
        private Mock<INativeMqttClient> _nativeMqttClient;

        private NikoResponseObservable _nikoResponseObservable;

        private HC2Client _client;

        [TestInitialize]
        public void Initialize()
        {
            _nativeMqttClient = new Mock<INativeMqttClient>();
            _nikoResponseObservable = new NikoResponseObservable();
            _nativeMqttClient.SetupGet(d => d.ResponseObservable).Returns(_nikoResponseObservable);

            var host = "MyHost";
            var token = "MyToken";

            _client = new HC2Client(_nativeMqttClient.Object, host, token);
        }

        #region DeviceList

        [TestCategory("DeviceList")]
        [TestMethod]
        public async Task DeviceList_Success()
        {
            Message message = null;
            var autoresetevent = new AutoResetEvent(false);
            var json = await File.ReadAllTextAsync(@"Data\DeviceList.json");
            var document = JsonDocument.Parse(json);

            await _client.Subscribe(new MessageObserver(m =>
            {
                message = m as Message;
                autoresetevent.Set();
            }));            
            
            _nikoResponseObservable.MessageReceived(new NikoMessage { MessageType = NikoMessageType.Rsp, Method = Constants.Messages.DevicesList, Params = document.RootElement});

            autoresetevent.WaitOne(TimeSpan.FromSeconds(2)).Should().BeTrue();
            message.Should().NotBeNull();
            message.MessageType.Should().Be(Constants.Messages.DevicesList);
            message.IsError.Should().BeFalse();
            message.Data.Should().AllBeAssignableTo<IDevice>();
            message.Data.Count.Should().Be(1);
            var device = (IDevice) message.Data[0];
            device.Identifier.Should().Be("a4fafca1-bde4-4ad7-94f9-292c60c26bf7");
            device.Parameters.Count.Should().Be(3);
            device.Properties.Count.Should().Be(3);
            device.Properties.First().Definition.ValueType.Should().Be(PropertyType.Range);
            device.Properties.First().Definition.Range.Should().NotBeNull();
            device.Properties.First().Definition.Range.End.Should().Be(100);
        }


        [TestMethod]
        [TestCategory("DeviceList")]
        public async Task DeviceList_Fail()
        {
            Message message = null;
            var autoresetevent = new AutoResetEvent(false);

            var document = JsonDocument.Parse("{}");

            await _client.Subscribe(new MessageObserver(m =>
            {
                message = m as Message;
                autoresetevent.Set();
            }));

            _nikoResponseObservable.MessageReceived(new NikoMessage { MessageType = NikoMessageType.Rsp, Method = Constants.Messages.DevicesList, Params = document.RootElement });

            autoresetevent.WaitOne(TimeSpan.FromSeconds(2)).Should().BeTrue();
            message.Should().NotBeNull();
            message.MessageType.Should().Be(Constants.Messages.DevicesList);
            message.IsError.Should().BeTrue();
            message.Error.Should().NotBeNull();
            message.Error.ErrCode.Should().Be(Constants.Errors.MessageParsingError);
        }

        #endregion

        #region LocationsList

        [TestCategory("LocationsList")]
        [TestMethod]
        public async Task LocationsList_Success()
        {
            Message message = null;
            var autoresetevent = new AutoResetEvent(false);
            var json = await File.ReadAllTextAsync(@"Data\LocationList.json");
            var document = JsonDocument.Parse(json);

            await _client.Subscribe(new MessageObserver(m =>
            {
                message = m as Message;
                autoresetevent.Set();
            }));

            _nikoResponseObservable.MessageReceived(new NikoMessage { MessageType = NikoMessageType.Rsp, Method = Constants.Messages.LocationsList, Params = document.RootElement });

            autoresetevent.WaitOne(TimeSpan.FromSeconds(2)).Should().BeTrue();
            message.Should().NotBeNull();
            message.MessageType.Should().Be(Constants.Messages.LocationsList);
            message.IsError.Should().BeFalse();
            message.Data.Should().AllBeAssignableTo<Location>();
            message.Data.Count.Should().Be(2);
            var locations = message.Data.Cast<Location>().ToList();
            locations[0].Id.Should().Be("b4e948b8-6378-498f-961c-b7c285c9f5b8");
        }


        [TestMethod]
        [TestCategory("LocationsList")]
        public async Task LocationsList_Fail()
        {
            Message message = null;
            var autoresetevent = new AutoResetEvent(false);

            var document = JsonDocument.Parse("{}");

            await _client.Subscribe(new MessageObserver(m =>
            {
                message = m as Message;
                autoresetevent.Set();
            }));

            _nikoResponseObservable.MessageReceived(new NikoMessage { MessageType = NikoMessageType.Rsp, Method = Constants.Messages.LocationsList, Params = document.RootElement });

            autoresetevent.WaitOne(TimeSpan.FromSeconds(2)).Should().BeTrue();
            message.Should().NotBeNull();
            message.MessageType.Should().Be(Constants.Messages.LocationsList);
            message.IsError.Should().BeTrue();
            message.Error.Should().NotBeNull();
            message.Error.ErrCode.Should().Be(Constants.Errors.MessageParsingError);
        }

        #endregion

        #region LocationsListItems

        [TestCategory("LocationsListItems")]
        [TestMethod]
        public async Task LocationsListItems_Success()
        {
            Message message = null;
            var autoresetevent = new AutoResetEvent(false);
            var json = await File.ReadAllTextAsync(@"Data\LocationListItems.json");
            var document = JsonDocument.Parse(json);

            await _client.Subscribe(new MessageObserver(m =>
            {
                message = m as Message;
                autoresetevent.Set();
            }));

            _nikoResponseObservable.MessageReceived(new NikoMessage { MessageType = NikoMessageType.Rsp, Method = Constants.Messages.LocationsListItems, Params = document.RootElement });

            autoresetevent.WaitOne(TimeSpan.FromSeconds(2)).Should().BeTrue();
            message.Should().NotBeNull();
            message.MessageType.Should().Be(Constants.Messages.LocationsListItems);
            message.IsError.Should().BeFalse();
            message.Data.Should().AllBeAssignableTo<ILocationItemsCollection>();
            message.Data.Count.Should().Be(2);
            var locations = message.Data.Cast<ILocationItemsCollection>().ToList();
            locations[0].Items.Count.Should().Be(0);
            locations[1].Items[1].Id.Should().Be("21a967a1-676d-487b-b8d4-9736ef16d450");
        }


        [TestMethod]
        [TestCategory("LocationsListItems")]
        public async Task LocationsListItems_Fail()
        {
            Message message = null;
            var autoresetevent = new AutoResetEvent(false);

            var document = JsonDocument.Parse("{}");

            await _client.Subscribe(new MessageObserver(m =>
            {
                message = m as Message;
                autoresetevent.Set();
            }));

            _nikoResponseObservable.MessageReceived(new NikoMessage { MessageType = NikoMessageType.Rsp, Method = Constants.Messages.LocationsListItems, Params = document.RootElement });

            autoresetevent.WaitOne(TimeSpan.FromSeconds(2)).Should().BeTrue();
            message.Should().NotBeNull();
            message.MessageType.Should().Be(Constants.Messages.LocationsListItems);
            message.IsError.Should().BeTrue();
            message.Error.Should().NotBeNull();
            message.Error.ErrCode.Should().Be(Constants.Errors.MessageParsingError);
        }

        #endregion

        #region SysteminfoPublish

        [TestCategory("SysteminfoPublish")]
        [TestMethod]
        public async Task SysteminfoPublish_Success()
        {
            Message message = null;
            var autoresetevent = new AutoResetEvent(false);
            var json = await File.ReadAllTextAsync(@"Data\SystemInfoPublish.json");
            var document = JsonDocument.Parse(json);

            await _client.Subscribe(new MessageObserver(m =>
            {
                message = m as Message;
                autoresetevent.Set();
            }));

            _nikoResponseObservable.MessageReceived(new NikoMessage { MessageType = NikoMessageType.Rsp, Method = Constants.Messages.SysteminfoPublish, Params = document.RootElement });

            autoresetevent.WaitOne(TimeSpan.FromSeconds(2)).Should().BeTrue();
            message.Should().NotBeNull();
            message.MessageType.Should().Be(Constants.Messages.SysteminfoPublish);
            message.IsError.Should().BeFalse();
            message.Data.Should().AllBeAssignableTo<SystemInfo>();
            message.Data.Count.Should().Be(1);
            var systeminfos = message.Data.Cast<SystemInfo>().ToList();
            systeminfos[0].ElectricityPrice.Should().Be(0);
            systeminfos[0].Currency.Should().Be("EUR");
            systeminfos[0].Version[1].Value.Should().Be("2019.1-20190118105507");
        }


        [TestMethod]
        [TestCategory("SysteminfoPublish")]
        public async Task SysteminfoPublish_Fail()
        {
            Message message = null;
            var autoresetevent = new AutoResetEvent(false);

            var document = JsonDocument.Parse("{}");

            await _client.Subscribe(new MessageObserver(m =>
            {
                message = m as Message;
                autoresetevent.Set();
            }));

            _nikoResponseObservable.MessageReceived(new NikoMessage { MessageType = NikoMessageType.Rsp, Method = Constants.Messages.SysteminfoPublish, Params = document.RootElement });

            autoresetevent.WaitOne(TimeSpan.FromSeconds(2)).Should().BeTrue();
            message.Should().NotBeNull();
            message.MessageType.Should().Be(Constants.Messages.SysteminfoPublish);
            message.IsError.Should().BeTrue();
            message.Error.Should().NotBeNull();
            message.Error.ErrCode.Should().Be(Constants.Errors.MessageParsingError);
        }

        #endregion

        #region NotificationsList

        [TestCategory("NotificationsList")]
        [TestMethod]
        public async Task NotificationsList_Success()
        {
            Message message = null;
            var autoresetevent = new AutoResetEvent(false);
            var json = await File.ReadAllTextAsync(@"Data\NotificationsList.json");
            var document = JsonDocument.Parse(json);

            await _client.Subscribe(new MessageObserver(m =>
            {
                message = m as Message;
                autoresetevent.Set();
            }));

            _nikoResponseObservable.MessageReceived(new NikoMessage { MessageType = NikoMessageType.Rsp, Method = Constants.Messages.NotificationsList, Params = document.RootElement });

            autoresetevent.WaitOne(TimeSpan.FromSeconds(2)).Should().BeTrue();
            message.Should().NotBeNull();
            message.MessageType.Should().Be(Constants.Messages.NotificationsList);
            message.IsError.Should().BeFalse();
            message.Data.Should().AllBeAssignableTo<Notification>();
            message.Data.Count.Should().Be(1);
            var notifications = message.Data.Cast<Notification>().ToList();
            notifications[0].Id.Should().Be("a5f576c6-7f4a-4541-bc39-28f617cff435");
            notifications[0].NotificationStatus.Should().Be(StatusEnum.New);
        }


        [TestMethod]
        [TestCategory("NotificationsList")]
        public async Task NotificationsList_Fail()
        {
            Message message = null;
            var autoresetevent = new AutoResetEvent(false);

            var document = JsonDocument.Parse("{}");

            await _client.Subscribe(new MessageObserver(m =>
            {
                message = m as Message;
                autoresetevent.Set();
            }));

            _nikoResponseObservable.MessageReceived(new NikoMessage { MessageType = NikoMessageType.Rsp, Method = Constants.Messages.NotificationsList, Params = document.RootElement });

            autoresetevent.WaitOne(TimeSpan.FromSeconds(2)).Should().BeTrue();
            message.Should().NotBeNull();
            message.MessageType.Should().Be(Constants.Messages.NotificationsList);
            message.IsError.Should().BeTrue();
            message.Error.Should().NotBeNull();
            message.Error.ErrCode.Should().Be(Constants.Errors.MessageParsingError);
        }

        #endregion

        #region NotificationsUpdate

        [TestCategory("NotificationsUpdate")]
        [TestMethod]
        public async Task NotificationsUpdate_Success()
        {
            Message message = null;
            var autoresetevent = new AutoResetEvent(false);
            var json = await File.ReadAllTextAsync(@"Data\NotificationsUpdate.json");
            var document = JsonDocument.Parse(json);

            await _client.Subscribe(new MessageObserver(m =>
            {
                message = m as Message;
                autoresetevent.Set();
            }));

            _nikoResponseObservable.MessageReceived(new NikoMessage { MessageType = NikoMessageType.Rsp, Method = Constants.Messages.NotificationsUpdate, Params = document.RootElement });

            autoresetevent.WaitOne(TimeSpan.FromSeconds(2)).Should().BeTrue();
            message.Should().NotBeNull();
            message.MessageType.Should().Be(Constants.Messages.NotificationsUpdate);
            message.IsError.Should().BeFalse();
            message.Data.Should().AllBeAssignableTo<Notification>();
            message.Data.Count.Should().Be(1);
            var notifications = message.Data.Cast<Notification>().ToList();
            notifications[0].Id.Should().Be("a5f576c6-7f4a-4541-bc39-28f617cff435");
            notifications[0].NotificationStatus.Should().Be(StatusEnum.Read);
        }


        [TestMethod]
        [TestCategory("NotificationsUpdate")]
        public async Task NotificationsUpdate_Fail()
        {
            Message message = null;
            var autoresetevent = new AutoResetEvent(false);

            var document = JsonDocument.Parse("{}");

            await _client.Subscribe(new MessageObserver(m =>
            {
                message = m as Message;
                autoresetevent.Set();
            }));

            _nikoResponseObservable.MessageReceived(new NikoMessage { MessageType = NikoMessageType.Rsp, Method = Constants.Messages.NotificationsUpdate, Params = document.RootElement });

            autoresetevent.WaitOne(TimeSpan.FromSeconds(2)).Should().BeTrue();
            message.Should().NotBeNull();
            message.MessageType.Should().Be(Constants.Messages.NotificationsUpdate);
            message.IsError.Should().BeTrue();
            message.Error.Should().NotBeNull();
            message.Error.ErrCode.Should().Be(Constants.Errors.MessageParsingError);
        }

        #endregion

        #region NotificationsRaised

        [TestCategory("NotificationsRaised")]
        [TestMethod]
        public async Task NotificationsRaised_Success()
        {
            Message message = null;
            var autoresetevent = new AutoResetEvent(false);
            var json = await File.ReadAllTextAsync(@"Data\NotificationsRaised.json");
            var document = JsonDocument.Parse(json);

            await _client.Subscribe(new MessageObserver(m =>
            {
                message = m as Message;
                autoresetevent.Set();
            }));

            _nikoResponseObservable.MessageReceived(new NikoMessage { MessageType = NikoMessageType.Rsp, Method = Constants.Messages.NotificationsRaised, Params = document.RootElement });

            autoresetevent.WaitOne(TimeSpan.FromSeconds(2)).Should().BeTrue();
            message.Should().NotBeNull();
            message.MessageType.Should().Be(Constants.Messages.NotificationsRaised);
            message.IsError.Should().BeFalse();
            message.Data.Should().AllBeAssignableTo<Notification>();
            message.Data.Count.Should().Be(1);
            var notifications = message.Data.Cast<Notification>().ToList();
            notifications[0].Id.Should().Be("a5f576c6-7f4a-4541-bc39-28f617cff435");
            notifications[0].NotificationStatus.Should().Be(StatusEnum.New);
        }


        [TestMethod]
        [TestCategory("NotificationsRaised")]
        public async Task NotificationsRaised_Fail()
        {
            Message message = null;
            var autoresetevent = new AutoResetEvent(false);

            var document = JsonDocument.Parse("{}");

            await _client.Subscribe(new MessageObserver(m =>
            {
                message = m as Message;
                autoresetevent.Set();
            }));

            _nikoResponseObservable.MessageReceived(new NikoMessage { MessageType = NikoMessageType.Rsp, Method = Constants.Messages.NotificationsRaised, Params = document.RootElement });

            autoresetevent.WaitOne(TimeSpan.FromSeconds(2)).Should().BeTrue();
            message.Should().NotBeNull();
            message.MessageType.Should().Be(Constants.Messages.NotificationsRaised);
            message.IsError.Should().BeTrue();
            message.Error.Should().NotBeNull();
            message.Error.ErrCode.Should().Be(Constants.Errors.MessageParsingError);
        }

        #endregion
    }
}
