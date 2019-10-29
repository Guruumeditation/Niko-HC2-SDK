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
    public class EventTests
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
        #region DevicesAdded

        [TestCategory(" DevicesAdded")]
        [TestMethod]
        public async Task DevicesAdded_Success()
        {
            Message message = null;
            var autoresetevent = new AutoResetEvent(false);
            var json = await File.ReadAllTextAsync(@"Data\DeviceAdded.json");
            var document = JsonDocument.Parse(json);

            await _client.Subscribe(new MessageObserver(m =>
            {
                message = m as Message;
                autoresetevent.Set();
            }));

            _nikoResponseObservable.MessageReceived(new NikoMessage { MessageType = NikoMessageType.Evt, Method = Constants.Messages.DevicesAdded, Params = document.RootElement });

            autoresetevent.WaitOne(TimeSpan.FromSeconds(2)).Should().BeTrue();
            message.Should().NotBeNull();
            message.MessageType.Should().Be(Constants.Messages.DevicesAdded);
            message.IsError.Should().BeFalse();
            message.Data.Should().AllBeAssignableTo<IDevice>();
            message.Data.Count.Should().Be(2);
            var devices = message.Data.Cast<IDevice>().ToList();
            devices[0].Id.Should().Be("b6a06a67-ce6f-42e2-933b-c67227996f46");
        }


        [TestMethod]
        [TestCategory(" DevicesAdded")]
        public async Task DevicesAdded_Fail()
        {
            Message message = null;
            var autoresetevent = new AutoResetEvent(false);

            var document = JsonDocument.Parse("{}");

            await _client.Subscribe(new MessageObserver(m =>
            {
                message = m as Message;
                autoresetevent.Set();
            }));

            _nikoResponseObservable.MessageReceived(new NikoMessage { MessageType = NikoMessageType.Evt, Method = Constants.Messages.DevicesRemoved, Params = document.RootElement });

            autoresetevent.WaitOne(TimeSpan.FromSeconds(2)).Should().BeTrue();
            message.Should().NotBeNull();
            message.MessageType.Should().Be(Constants.Messages.DevicesRemoved);
            message.IsError.Should().BeTrue();
            message.Error.Should().NotBeNull();
            message.Error.ErrCode.Should().Be(Constants.Errors.MessageParsingError);
        }

        #endregion

        #region DeviceRemoved

        [TestCategory(" DeviceRemoved")]
        [TestMethod]
        public async Task DeviceRemoved_Success()
        {
            Message message = null;
            var autoresetevent = new AutoResetEvent(false);
            var json = await File.ReadAllTextAsync(@"Data\DeviceRemoved.json");
            var document = JsonDocument.Parse(json);

            await _client.Subscribe(new MessageObserver(m =>
            {
                message = m as Message;
                autoresetevent.Set();
            }));

            _nikoResponseObservable.MessageReceived(new NikoMessage { MessageType = NikoMessageType.Evt, Method = Constants.Messages.DevicesRemoved, Params = document.RootElement });

            autoresetevent.WaitOne(TimeSpan.FromSeconds(2)).Should().BeTrue();
            message.Should().NotBeNull();
            message.MessageType.Should().Be(Constants.Messages.DevicesRemoved);
            message.IsError.Should().BeFalse();
            message.Data.Should().AllBeAssignableTo<string>();
            message.Data.Count.Should().Be(2);
            var devices = message.Data.Cast<string>().ToList();
            devices[0].Should().Be("ab2e315e-a6df-4cc8-9518-5fa2a48226f5");
            devices[1].Should().Be("ae56f142-03ca-4de6-8547-282489a615ca");
        }


        [TestMethod]
        [TestCategory(" DeviceRemoved")]
        public async Task DeviceRemoved_Fail()
        {
            Message message = null;
            var autoresetevent = new AutoResetEvent(false);

            var document = JsonDocument.Parse("{}");

            await _client.Subscribe(new MessageObserver(m =>
            {
                message = m as Message;
                autoresetevent.Set();
            }));

            _nikoResponseObservable.MessageReceived(new NikoMessage { MessageType = NikoMessageType.Evt, Method = Constants.Messages.DevicesRemoved, Params = document.RootElement });

            autoresetevent.WaitOne(TimeSpan.FromSeconds(2)).Should().BeTrue();
            message.Should().NotBeNull();
            message.MessageType.Should().Be(Constants.Messages.DevicesRemoved);
            message.IsError.Should().BeTrue();
            message.Error.Should().NotBeNull();
            message.Error.ErrCode.Should().Be(Constants.Errors.MessageParsingError);
        }

        #endregion

        #region DevicesParamChanged

        [TestCategory(" DevicesParamChanged")]
        [TestMethod]
        public async Task DevicesParamChanged_Success()
        {
            Message message = null;
            var autoresetevent = new AutoResetEvent(false);
            var json = await File.ReadAllTextAsync(@"Data\DevicesParamChanged.json");
            var document = JsonDocument.Parse(json);

            await _client.Subscribe(new MessageObserver(m =>
            {
                message = m as Message;
                autoresetevent.Set();
            }));

            _nikoResponseObservable.MessageReceived(new NikoMessage { MessageType = NikoMessageType.Evt, Method = Constants.Messages.DevicesParamChanged, Params = document.RootElement });

            autoresetevent.WaitOne(TimeSpan.FromSeconds(2)).Should().BeTrue();
            message.Should().NotBeNull();
            message.MessageType.Should().Be(Constants.Messages.DevicesParamChanged);
            message.IsError.Should().BeFalse();
            message.Data.Should().AllBeAssignableTo<IDeviceParameters>();
            message.Data.Count.Should().Be(3);
            var devices = message.Data.Cast<IDeviceParameters>().ToList();
            devices[0].Id.Should().Be("ab2e315e-a6df-4cc8-9518-5fa2a48226f5");
            devices[0].Parameters.Count.Should().Be(3);
            devices[1].Parameters.Count.Should().Be(5);
            devices[2].Parameters.Count.Should().Be(3);
        }


        [TestMethod]
        [TestCategory(" DevicesParamChanged")]
        public async Task DevicesParamChanged_Fail()
        {
            Message message = null;
            var autoresetevent = new AutoResetEvent(false);

            var document = JsonDocument.Parse("{}");

            await _client.Subscribe(new MessageObserver(m =>
            {
                message = m as Message;
                autoresetevent.Set();
            }));

            _nikoResponseObservable.MessageReceived(new NikoMessage { MessageType = NikoMessageType.Evt, Method = Constants.Messages.DevicesParamChanged, Params = document.RootElement });

            autoresetevent.WaitOne(TimeSpan.FromSeconds(2)).Should().BeTrue();
            message.Should().NotBeNull();
            message.MessageType.Should().Be(Constants.Messages.DevicesParamChanged);
            message.IsError.Should().BeTrue();
            message.Error.Should().NotBeNull();
            message.Error.ErrCode.Should().Be(Constants.Errors.MessageParsingError);
        }

        #endregion

        #region DeviceDisplayNameChanged

        [TestCategory(" DeviceDisplayNameChanged")]
        [TestMethod]
        public async Task DeviceDisplayNameChanged_Success()
        {
            Message message = null;
            var autoresetevent = new AutoResetEvent(false);
            var json = await File.ReadAllTextAsync(@"Data\DeviceDisplayNameChanged.json");
            var document = JsonDocument.Parse(json);

            await _client.Subscribe(new MessageObserver(m =>
            {
                message = m as Message;
                autoresetevent.Set();
            }));

            _nikoResponseObservable.MessageReceived(new NikoMessage { MessageType = NikoMessageType.Evt, Method = Constants.Messages.DevicesDisplayNameChanged, Params = document.RootElement });

            autoresetevent.WaitOne(TimeSpan.FromSeconds(2)).Should().BeTrue();
            message.Should().NotBeNull();
            message.MessageType.Should().Be(Constants.Messages.DevicesDisplayNameChanged);
            message.IsError.Should().BeFalse();
            message.Data.Should().AllBeAssignableTo<IDeviceDisplayName>();
            message.Data.Count.Should().Be(1);
            var devices = message.Data.Cast<IDeviceDisplayName>().ToList();
            devices[0].Id.Should().Be("ae56f142-03ca-4de6-8547-282489a615ca");
            devices[0].Name.Should().Be("Single push button");
        }


        [TestMethod]
        [TestCategory(" DeviceDisplayNameChanged")]
        public async Task DeviceDisplayNameChanged_Fail()
        {
            Message message = null;
            var autoresetevent = new AutoResetEvent(false);

            var document = JsonDocument.Parse("{}");

            await _client.Subscribe(new MessageObserver(m =>
            {
                message = m as Message;
                autoresetevent.Set();
            }));

            _nikoResponseObservable.MessageReceived(new NikoMessage { MessageType = NikoMessageType.Evt, Method = Constants.Messages.DevicesDisplayNameChanged, Params = document.RootElement });

            autoresetevent.WaitOne(TimeSpan.FromSeconds(2)).Should().BeTrue();
            message.Should().NotBeNull();
            message.MessageType.Should().Be(Constants.Messages.DevicesDisplayNameChanged);
            message.IsError.Should().BeTrue();
            message.Error.Should().NotBeNull();
            message.Error.ErrCode.Should().Be(Constants.Errors.MessageParsingError);
        }

        #endregion

        #region DeviceChanged

        [TestCategory(" DeviceChanged")]
        [TestMethod]
        public async Task DeviceChanged_Success()
        {
            Message message = null;
            var autoresetevent = new AutoResetEvent(false);
            var json = await File.ReadAllTextAsync(@"Data\DeviceChanged.json");
            var document = JsonDocument.Parse(json);

            await _client.Subscribe(new MessageObserver(m =>
            {
                message = m as Message;
                autoresetevent.Set();
            }));

            _nikoResponseObservable.MessageReceived(new NikoMessage { MessageType = NikoMessageType.Evt, Method = Constants.Messages.DevicesChanged, Params = document.RootElement });

            autoresetevent.WaitOne(TimeSpan.FromSeconds(2)).Should().BeTrue();
            message.Should().NotBeNull();
            message.MessageType.Should().Be(Constants.Messages.DevicesChanged);
            message.IsError.Should().BeFalse();
            message.Data.Should().AllBeAssignableTo<IDevicePropertyDefinitions>();
            message.Data.Count.Should().Be(2);
            var devices = message.Data.Cast<IDevicePropertyDefinitions>().ToList();
            devices[0].PropertyDefinitions.Count.Should().Be(1);
            devices[1].PropertyDefinitions.Count.Should().Be(1);
            devices[1].Id.Should().Be("ab2e315e-a6df-4cc8-9518-5fa2a48226f5");
        }


        [TestMethod]
        [TestCategory(" DeviceChanged")]
        public async Task DeviceChanged_Fail()
        {
            Message message = null;
            var autoresetevent = new AutoResetEvent(false);

            var document = JsonDocument.Parse("{}");

            await _client.Subscribe(new MessageObserver(m =>
            {
                message = m as Message;
                autoresetevent.Set();
            }));

            _nikoResponseObservable.MessageReceived(new NikoMessage { MessageType = NikoMessageType.Evt, Method = Constants.Messages.DevicesChanged, Params = document.RootElement });

            autoresetevent.WaitOne(TimeSpan.FromSeconds(2)).Should().BeTrue();
            message.Should().NotBeNull();
            message.MessageType.Should().Be(Constants.Messages.DevicesChanged);
            message.IsError.Should().BeTrue();
            message.Error.Should().NotBeNull();
            message.Error.ErrCode.Should().Be(Constants.Errors.MessageParsingError);
        }

        #endregion

        #region DevicesStatus

        [TestCategory(" DevicesStatus")]
        [TestMethod]
        public async Task DevicesStatus_Success()
        {
            Message message = null;
            var autoresetevent = new AutoResetEvent(false);
            var json = await File.ReadAllTextAsync(@"Data\DevicesStatus.json");
            var document = JsonDocument.Parse(json);

            await _client.Subscribe(new MessageObserver(m =>
            {
                message = m as Message;
                autoresetevent.Set();
            }));

            _nikoResponseObservable.MessageReceived(new NikoMessage { MessageType = NikoMessageType.Evt, Method = Constants.Messages.DevicesStatus, Params = document.RootElement });

            autoresetevent.WaitOne(TimeSpan.FromSeconds(2)).Should().BeTrue();
            message.Should().NotBeNull();
            message.MessageType.Should().Be(Constants.Messages.DevicesStatus);
            message.IsError.Should().BeFalse();
            message.Data.Should().AllBeAssignableTo<IDeviceProperties>();
            message.Data.Count.Should().Be(2);
            var devices = message.Data.Cast<IDeviceProperties>().ToList();
            devices[0].Properties.Count.Should().Be(1);
            devices[1].Properties.Count.Should().Be(2);
            devices[1].Id.Should().Be("21a967a1-676d-487b-b8d4-9736ef16d450");
        }


        [TestMethod]
        [TestCategory(" DevicesStatus")]
        public async Task DevicesStatus_Fail()
        {
            Message message = null;
            var autoresetevent = new AutoResetEvent(false);

            var document = JsonDocument.Parse("{}");

            await _client.Subscribe(new MessageObserver(m =>
            {
                message = m as Message;
                autoresetevent.Set();
            }));

            _nikoResponseObservable.MessageReceived(new NikoMessage { MessageType = NikoMessageType.Evt, Method = Constants.Messages.DevicesStatus, Params = document.RootElement });

            autoresetevent.WaitOne(TimeSpan.FromSeconds(2)).Should().BeTrue();
            message.Should().NotBeNull();
            message.MessageType.Should().Be(Constants.Messages.DevicesStatus);
            message.IsError.Should().BeTrue();
            message.Error.Should().NotBeNull();
            message.Error.ErrCode.Should().Be(Constants.Errors.MessageParsingError);
        }

        #endregion

        #region DevicesStatus

        [TestCategory("TimePublished")]
        [TestMethod]
        public async Task TimePublished_Success()
        {
            Message message = null;
            var autoresetevent = new AutoResetEvent(false);
            var json = await File.ReadAllTextAsync(@"Data\TimePublished.json");
            var document = JsonDocument.Parse(json);

            await _client.Subscribe(new MessageObserver(m =>
            {
                message = m as Message;
                autoresetevent.Set();
            }));

            _nikoResponseObservable.MessageReceived(new NikoMessage { MessageType = NikoMessageType.Evt, Method = Constants.Messages.TimePublished, Params = document.RootElement });

            autoresetevent.WaitOne(TimeSpan.FromSeconds(2)).Should().BeTrue();
            message.Should().NotBeNull();
            message.MessageType.Should().Be(Constants.Messages.TimePublished);
            message.IsError.Should().BeFalse();
            message.Data.Should().AllBeAssignableTo<ITimeInfo>();
            message.Data.Count.Should().Be(1);
            var devices = message.Data.Cast<ITimeInfo>().ToList();
            devices[0].Timezone.Should().Be("Europe/Brussels");
        }


        [TestMethod]
        [TestCategory(" TimePublished")]
        public async Task TimePublished_Fail()
        {
            Message message = null;
            var autoresetevent = new AutoResetEvent(false);

            var document = JsonDocument.Parse("{}");

            await _client.Subscribe(new MessageObserver(m =>
            {
                message = m as Message;
                autoresetevent.Set();
            }));

            _nikoResponseObservable.MessageReceived(new NikoMessage { MessageType = NikoMessageType.Evt, Method = Constants.Messages.DevicesStatus, Params = document.RootElement });

            autoresetevent.WaitOne(TimeSpan.FromSeconds(2)).Should().BeTrue();
            message.Should().NotBeNull();
            message.MessageType.Should().Be(Constants.Messages.DevicesStatus);
            message.IsError.Should().BeTrue();
            message.Error.Should().NotBeNull();
            message.Error.ErrCode.Should().Be(Constants.Errors.MessageParsingError);
        }

        #endregion

        #region SysteminfoPublish

        [TestCategory("SysteminfoPublished")]
        [TestMethod]
        public async Task SysteminfoPublished_Success()
        {
            Message message = null;
            var autoresetevent = new AutoResetEvent(false);
            var json = await File.ReadAllTextAsync(@"Data\SystemInfoPublished.json");
            var document = JsonDocument.Parse(json);

            await _client.Subscribe(new MessageObserver(m =>
            {
                message = m as Message;
                autoresetevent.Set();
            }));

            _nikoResponseObservable.MessageReceived(new NikoMessage { MessageType = NikoMessageType.Evt, Method = Constants.Messages.SysteminfoPublished, Params = document.RootElement });

            autoresetevent.WaitOne(TimeSpan.FromSeconds(2)).Should().BeTrue();
            message.Should().NotBeNull();
            message.MessageType.Should().Be(Constants.Messages.SysteminfoPublished);
            message.IsError.Should().BeFalse();
            message.Data.Should().AllBeAssignableTo<ISystemInfo>();
            message.Data.Count.Should().Be(1);
            var systeminfos = message.Data.Cast<ISystemInfo>().ToList();
            systeminfos[0].ElectricityPrice.Should().Be(0);
            systeminfos[0].Currency.Should().Be("EUR");
            systeminfos[0].Version[1].Value.Should().Be("2019.1-20190118105507");
        }


        [TestMethod]
        [TestCategory("SysteminfoPublish")]
        public async Task SysteminfoPublished_Fail()
        {
            Message message = null;
            var autoresetevent = new AutoResetEvent(false);

            var document = JsonDocument.Parse("{}");

            await _client.Subscribe(new MessageObserver(m =>
            {
                message = m as Message;
                autoresetevent.Set();
            }));

            _nikoResponseObservable.MessageReceived(new NikoMessage { MessageType = NikoMessageType.Evt, Method = Constants.Messages.SysteminfoPublished, Params = document.RootElement });

            autoresetevent.WaitOne(TimeSpan.FromSeconds(2)).Should().BeTrue();
            message.Should().NotBeNull();
            message.MessageType.Should().Be(Constants.Messages.SysteminfoPublished);
            message.IsError.Should().BeTrue();
            message.Error.Should().NotBeNull();
            message.Error.ErrCode.Should().Be(Constants.Errors.MessageParsingError);
        }

        #endregion
    }
}
