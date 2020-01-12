using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using HC2.Arcanastudio.Net.Client;
using HC2.Arcanastudio.Net.Client.Results;
using HC2.Arcanastudio.Net.Observable;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace ClientTests
{
    [TestClass]
    public class ConnectionDeconnectionTests
    {
        private Mock<INativeMqttClient> _nativeMqttClient;

        private NikoResponseObservable _nikoResponseObservable;

        [TestInitialize]
        public void Initialize()
        {
            _nativeMqttClient = new Mock<INativeMqttClient>();
            _nikoResponseObservable = new NikoResponseObservable();
            _nativeMqttClient.SetupGet(d => d.ResponseObservable).Returns(_nikoResponseObservable);
        }

        [TestMethod]
        public async Task Connect_Success()
        {
            var host = "MyHost";
            var token = "MyToken";

            var client = new HC2Client(_nativeMqttClient.Object,host, token);

            _nativeMqttClient.Setup(d => d.Connect(It.IsAny<string>(), It.IsAny<string>(),It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ConnectionResult(ConnectResultCode.Success), TimeSpan.FromSeconds(1));
            _nativeMqttClient.Setup(d => d.Subscribe()).ReturnsAsync(new SubscribeResult(SubscribeResultCode.Success),
                TimeSpan.FromSeconds(0.5));

            var response = await client.Connect(new MessageObserver(e => {}));

            response.IsSuccess.Should().BeTrue();

            _nativeMqttClient.Verify(d => d.Connect(host,token, It.IsAny<CancellationToken>()), Times.Once);
        }

        [TestMethod]
        public async Task Connect_Fail()
        {
            var host = "MyHost";
            var token = "MyToken";

            var client = new HC2Client(_nativeMqttClient.Object, host, token);

            _nativeMqttClient.Setup(d => d.Connect(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ConnectionResult(ConnectResultCode.NotAuthorized,"Error"), TimeSpan.FromSeconds(1));

            var response = await client.Connect(new MessageObserver(e => {}));

            response.IsSuccess.Should().BeFalse();
            _nativeMqttClient.Verify(d => d.Connect(host, token, It.IsAny<CancellationToken>()), Times.Once);
        }

        [TestMethod]
        public async Task Connect_Exception()
        {
            var host = "MyHost";
            var token = "MyToken";
            var exceptionmessage = "This is an exception";

            var client = new HC2Client(_nativeMqttClient.Object, host, token);

            _nativeMqttClient.Setup(d => d.Connect(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception(exceptionmessage));

            Exception ex = null;
            try
            {
                var response = await client.Connect(new MessageObserver(e => {}));
            }
            catch (Exception e)
            {
                ex = e;
            }

            Assert.IsNotNull(ex);
            Assert.AreEqual(exceptionmessage,ex.Message);
            _nativeMqttClient.Verify(d => d.Connect(host, token, It.IsAny<CancellationToken>()), Times.Once);
        }

        [TestMethod]
        public async Task Disconnect_Success()
        {
            var eventraised = false;
            var host = "MyHost";
            var token = "MyToken";
            var autoresetevent = new AutoResetEvent(false);
            _nativeMqttClient.Setup(d => d.Disconnect()).Returns(Task.CompletedTask).Raises(d => d.Disconnected+= null,EventArgs.Empty);

            var client = new HC2Client(_nativeMqttClient.Object, host, token);
            client.Disconnected += (sender, args) =>
            {
                eventraised = true;
                autoresetevent.Set();
            };

            await client.Disconnect();

             autoresetevent.WaitOne(TimeSpan.FromSeconds(2)).Should().BeTrue();
             eventraised.Should().Be(true);
            _nativeMqttClient.Verify(d => d.Disconnect(), Times.Once);
        }
    }
}
