using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using HC2.Arcanastudio.Net;
using HC2.Arcanastudio.Net.Client;
using HC2.Arcanastudio.Net.Client.Messages;
using HC2.Arcanastudio.Net.Models;
using HC2.Arcanastudio.Net.Observable;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace ClientTests
{
    [TestClass]
    public class ErrorTests
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

        [TestCategory(" ErrorReceived")]
        [TestMethod]
        public async Task ErrorReceived()
        {
            Message message = null;
            var autoresetevent = new AutoResetEvent(false);
            var json = await File.ReadAllTextAsync(@"Data\Error.json");
            var document = JsonDocument.Parse(json);

            await _client.Subscribe(new MessageObserver(m =>
            {
                message = m as Message;
                autoresetevent.Set();
            }));

            _nikoResponseObservable.MessageReceived(new NikoMessage { MessageType = NikoMessageType.Err, Method = Constants.Messages.LocationsList, Params = document.RootElement });

            autoresetevent.WaitOne(TimeSpan.FromSeconds(2)).Should().BeTrue();
            message.Should().NotBeNull();
            message.MessageType.Should().Be(Constants.Messages.LocationsList);
            message.IsError.Should().BeTrue();
            message.Data.Should().BeNull();
            message.Error.Should().NotBeNull();
            message.Error.ErrCode.Should().Be("UNKNOWN_METHOD");
            message.Error.ErrMessage.Should().Be("Method 'unknown' not supported for topic 'hobby/control/location/cmd'");
       
        }

        #endregion
    }
}
