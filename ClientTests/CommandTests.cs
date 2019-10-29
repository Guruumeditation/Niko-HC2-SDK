using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using HC2.Arcanastudio.Net.Client;
using HC2.Arcanastudio.Net.Client.Results;
using HC2.Arcanastudio.Net.Models;
using HC2.Arcanastudio.Net.Observable;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace ClientTests
{
    [TestClass]
    public class CommandTests
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

        #region Devices

        [TestCategory("GetDevices")]
        [TestMethod]
        public async Task GetDevices_Success()
        {
            var payload = "{\r\n  \"Method\": \"devices.list\"\r\n}";

            _nativeMqttClient.Setup(d => d.PublishMessage(It.IsAny<Request>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new PublishResult(PublishResultCode.Success), TimeSpan.FromSeconds(1));

            var response = await _client.GetDevices();

            response.IsSuccess.Should().BeTrue();
            _nativeMqttClient.Verify(d => d.PublishMessage(It.Is<Request>(d => d.Payload == payload), It.IsAny<CancellationToken>()), Times.Once);
        }


        [TestMethod]
        [TestCategory("GetDevices")]
        public async Task GetDevices_Fail()
        {
            var payload = "{\r\n  \"Method\": \"devices.list\"\r\n}";

            _nativeMqttClient.Setup(d => d.PublishMessage(It.IsAny<Request>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new PublishResult(PublishResultCode.UnspecifiedError), TimeSpan.FromSeconds(1));

            var response = await _client.GetDevices();

            response.IsSuccess.Should().BeFalse();
            _nativeMqttClient.Verify(d => d.PublishMessage(It.Is<Request>(d => d.Payload == payload), It.IsAny<CancellationToken>()), Times.Once);
        }

        #endregion

        #region SendCommand

        [TestCategory("SendCommand")]
        [TestMethod]
        public async Task SendCommand_Success()
        {
            var payload = "{\r\n  \"Method\": \"devices.control\",\r\n  \"Params\": [\r\n    {\r\n      \"Devices\": [\r\n        {\r\n          \"Uuid\": \"02544b91-ea50-4241-885c-e5002abbe0ea\",\r\n          \"Properties\": [\r\n            {\r\n              \"Status\": \"Off\"\r\n            }\r\n          ]\r\n        }\r\n      ]\r\n    }\r\n  ]\r\n}";

            _nativeMqttClient.Setup(d => d.PublishMessage(It.IsAny<Request>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new PublishResult(PublishResultCode.Success), TimeSpan.FromSeconds(1));

            var response = await _client.SendCommand(new List<DeviceCommand> { new DeviceCommand("02544b91-ea50-4241-885c-e5002abbe0ea", new Dictionary<string, string> { { "Status", "Off" } }) });

            response.IsSuccess.Should().BeTrue();
            _nativeMqttClient.Verify(d => d.PublishMessage(It.Is<Request>(d => d.Payload == payload), It.IsAny<CancellationToken>()), Times.Once);
        }


        [TestMethod]
        [TestCategory("SendCommand")]
        public async Task SendCommand_Fail()
        {
            var payload = "{\r\n  \"Method\": \"devices.control\",\r\n  \"Params\": [\r\n    {\r\n      \"Devices\": [\r\n        {\r\n          \"Uuid\": \"02544b91-ea50-4241-885c-e5002abbe0ea\",\r\n          \"Properties\": [\r\n            {\r\n              \"Status\": \"Off\"\r\n            }\r\n          ]\r\n        }\r\n      ]\r\n    }\r\n  ]\r\n}";

            _nativeMqttClient.Setup(d => d.PublishMessage(It.IsAny<Request>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new PublishResult(PublishResultCode.UnspecifiedError), TimeSpan.FromSeconds(1));

            var response = await _client.SendCommand(new List<DeviceCommand> { new DeviceCommand("02544b91-ea50-4241-885c-e5002abbe0ea", new Dictionary<string, string> { { "Status", "Off" } }) });


            response.IsSuccess.Should().BeFalse();
            _nativeMqttClient.Verify(d => d.PublishMessage(It.Is<Request>(d => d.Payload == payload), It.IsAny<CancellationToken>()), Times.Once);
        }

        #endregion

        #region ListLocations

        [TestCategory("ListLocations")]
        [TestMethod]
        public async Task ListLocations_Success()
        {
            var payload = "{\r\n  \"Method\": \"locations.list\"\r\n}";

            _nativeMqttClient.Setup(d => d.PublishMessage(It.IsAny<Request>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new PublishResult(PublishResultCode.Success), TimeSpan.FromSeconds(1));

            var response = await _client.GetLocations();

            response.IsSuccess.Should().BeTrue();
            _nativeMqttClient.Verify(d => d.PublishMessage(It.Is<Request>(d => d.Payload == payload), It.IsAny<CancellationToken>()), Times.Once);
        }


        [TestMethod]
        [TestCategory("ListLocations")]
        public async Task ListLocations_Fail()
        {
            var payload = "{\r\n  \"Method\": \"locations.list\"\r\n}";

            _nativeMqttClient.Setup(d => d.PublishMessage(It.IsAny<Request>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new PublishResult(PublishResultCode.UnspecifiedError), TimeSpan.FromSeconds(1));

            var response = await _client.GetLocations();

            response.IsSuccess.Should().BeFalse();
            _nativeMqttClient.Verify(d => d.PublishMessage(It.Is<Request>(d => d.Payload == payload), It.IsAny<CancellationToken>()), Times.Once);
        }

        #endregion

        #region ListLocationsItems

        [TestCategory("ListLocationsItems")]
        [TestMethod]
        public async Task ListLocationsItems_Success()
        {
            var payload = "{\r\n  \"Method\": \"locations.listitems\",\r\n  \"Params\": [\r\n    {\r\n      \"Locations\": [\r\n        {\r\n          \"Uuid\": \"b4e948b8-6378-498f-961c-b7c285c9f5b8\"\r\n        },\r\n        {\r\n          \"Uuid\": \"7f62f934-83d3-4c66-b4bd-df7065cb1c6a\"\r\n        }\r\n      ]\r\n    }";

            _nativeMqttClient.Setup(d => d.PublishMessage(It.IsAny<Request>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new PublishResult(PublishResultCode.Success), TimeSpan.FromSeconds(1));

            var response = await _client.GetLocationItems(new List<string>{ "b4e948b8-6378-498f-961c-b7c285c9f5b8" , "7f62f934-83d3-4c66-b4bd-df7065cb1c6a" });

            response.IsSuccess.Should().BeTrue();
            _nativeMqttClient.Verify(d => d.PublishMessage(It.Is<Request>(d => d.Payload == payload), It.IsAny<CancellationToken>()), Times.Once);
        }


        [TestMethod]
        [TestCategory("ListLocationsItems")]
        public async Task ListLocationsItems_Fail()
        {
            var payload = "{\r\n  \"Method\": \"locations.listitems\",\r\n  \"Params\": [\r\n    {\r\n      \"Locations\": [\r\n        {\r\n          \"Uuid\": \"b4e948b8-6378-498f-961c-b7c285c9f5b8\"\r\n        },\r\n        {\r\n          \"Uuid\": \"7f62f934-83d3-4c66-b4bd-df7065cb1c6a\"\r\n        }\r\n      ]\r\n    }";

            _nativeMqttClient.Setup(d => d.PublishMessage(It.IsAny<Request>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new PublishResult(PublishResultCode.UnspecifiedError), TimeSpan.FromSeconds(1));

            var response = await _client.GetLocationItems(new List<string> { "b4e948b8-6378-498f-961c-b7c285c9f5b8", "7f62f934-83d3-4c66-b4bd-df7065cb1c6a" });

            response.IsSuccess.Should().BeFalse();
            _nativeMqttClient.Verify(d => d.PublishMessage(It.Is<Request>(d => d.Payload == payload), It.IsAny<CancellationToken>()), Times.Once);
        }

        #endregion

        #region SystemInfo

        [TestCategory("SystemInfo")]
        [TestMethod]
        public async Task SystemInfo_Success()
        {
            var payload = "{\r\n  \"Method\": \"systeminfo.publish\"\r\n}";

            _nativeMqttClient.Setup(d => d.PublishMessage(It.IsAny<Request>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new PublishResult(PublishResultCode.Success), TimeSpan.FromSeconds(1));

            var response = await _client.GetSystemInfo();

            response.IsSuccess.Should().BeTrue();
            _nativeMqttClient.Verify(d => d.PublishMessage(It.Is<Request>(d => d.Payload == payload), It.IsAny<CancellationToken>()), Times.Once);
        }


        [TestMethod]
        [TestCategory("SystemInfo")]
        public async Task SystemInfo_Fail()
        {
            var payload = "{\r\n  \"Method\": \"systeminfo.publish\"\r\n}";

            _nativeMqttClient.Setup(d => d.PublishMessage(It.IsAny<Request>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new PublishResult(PublishResultCode.UnspecifiedError), TimeSpan.FromSeconds(1));

            var response = await _client.GetSystemInfo();

            response.IsSuccess.Should().BeFalse();
            _nativeMqttClient.Verify(d => d.PublishMessage(It.Is<Request>(d => d.Payload == payload), It.IsAny<CancellationToken>()), Times.Once);
        }

        #endregion

        #region NotificationList

        [TestCategory("NotificationList")]
        [TestMethod]
        public async Task NotificationList_Success()
        {
            var payload = "{\r\n  \"Method\": \"notification.list\"\r\n}";

            _nativeMqttClient.Setup(d => d.PublishMessage(It.IsAny<Request>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new PublishResult(PublishResultCode.Success), TimeSpan.FromSeconds(1));

            var response = await _client.GetNotifications();

            response.IsSuccess.Should().BeTrue();
            _nativeMqttClient.Verify(d => d.PublishMessage(It.Is<Request>(d => d.Payload == payload), It.IsAny<CancellationToken>()), Times.Once);
        }


        [TestMethod]
        [TestCategory("NotificationList")]
        public async Task NotificationList_Fail()
        {
            var payload = "{\r\n  \"Method\": \"notification.list\"\r\n}";

            _nativeMqttClient.Setup(d => d.PublishMessage(It.IsAny<Request>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new PublishResult(PublishResultCode.UnspecifiedError), TimeSpan.FromSeconds(1));

            var response = await _client.GetNotifications();

            response.IsSuccess.Should().BeFalse();
            _nativeMqttClient.Verify(d => d.PublishMessage(It.Is<Request>(d => d.Payload == payload), It.IsAny<CancellationToken>()), Times.Once);
        }

        #endregion

        #region NotificationUpdate

        [TestCategory("NotificationUpdate")]
        [TestMethod]
        public async Task NotificationUpdate_Success()
        {
            var payload = "{\r\n  \"Method\": \" notifications.update\",\r\n  \"Params\": [\r\n    {\r\n      \"Notifications\": [\r\n        {\r\n          \"Uuid\": \"a5f576c6-7f4a-4541-bc39-28f617cff435\",\r\n          \"Status\": \"read\"\r\n        },\r\n        {\r\n          \"Uuid\": \"a5f576c6-7f4a-4541-bc39-28f617cff435\",\r\n          \"Status\": \"read\"\r\n        }\r\n      ]\r\n    }\r\n  ]\r\n}";

            var s = "";
            _nativeMqttClient.Setup(d => d.PublishMessage(It.IsAny<Request>(), It.IsAny<CancellationToken>()))
                .Callback<Request, CancellationToken>((a, b) => s = a.Payload).ReturnsAsync(new PublishResult(PublishResultCode.Success), TimeSpan.FromSeconds(1));

            var response = await _client.UpdateNotifications(new List<string>{ "a5f576c6-7f4a-4541-bc39-28f617cff435", "a5f576c6-7f4a-4541-bc39-28f617cff435" });

            response.IsSuccess.Should().BeTrue();
            _nativeMqttClient.Verify(d => d.PublishMessage(It.Is<Request>(d => d.Payload == payload), It.IsAny<CancellationToken>()), Times.Once);
        }


        [TestMethod]
        [TestCategory("NotificationUpdate")]
        public async Task NotificationUpdate_Fail()
        {
            var payload = "{\r\n  \"Method\": \" notifications.update\",\r\n  \"Params\": [\r\n    {\r\n      \"Notifications\": [\r\n        {\r\n          \"Uuid\": \"a5f576c6-7f4a-4541-bc39-28f617cff435\",\r\n          \"Status\": \"read\"\r\n        },\r\n        {\r\n          \"Uuid\": \"a5f576c6-7f4a-4541-bc39-28f617cff435\",\r\n          \"Status\": \"read\"\r\n        }\r\n      ]\r\n    }\r\n  ]\r\n}";

            _nativeMqttClient.Setup(d => d.PublishMessage(It.IsAny<Request>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new PublishResult(PublishResultCode.UnspecifiedError), TimeSpan.FromSeconds(1));

            var response = await _client.UpdateNotifications(new List<string> { "a5f576c6-7f4a-4541-bc39-28f617cff435", "a5f576c6-7f4a-4541-bc39-28f617cff435" });


            response.IsSuccess.Should().BeFalse();
            _nativeMqttClient.Verify(d => d.PublishMessage(It.Is<Request>(d => d.Payload == payload), It.IsAny<CancellationToken>()), Times.Once);
        }

        #endregion
    }
}
