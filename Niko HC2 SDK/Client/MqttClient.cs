using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net.Security;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using HC2.Arcanastudio.Net.Client.Messages;
using HC2.Arcanastudio.Net.Client.Results;
using HC2.Arcanastudio.Net.Extensions;
using HC2.Arcanastudio.Net.Log;
using HC2.Arcanastudio.Net.Observable;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Options;
using MQTTnet.Protocol;

namespace HC2.Arcanastudio.Net.Client
{
    internal interface INativeMqttClient
    {
        Task<ConnectionResult> Connect(string hostname, string token, CancellationToken canceltoken);
        Task Disconnect();
        Task<PublishResult> PublishMessage(Request request, CancellationToken canceltoken);
        Task<SubscribeResult> Subscribe();
        NikoResponseObservable ResponseObservable { get; }
        bool IsConnected { get; }
    }
    [ExcludeFromCodeCoverage]
    internal class MqttClient : INativeMqttClient
    {
        private IMqttClient _mqttClient;

        public NikoResponseObservable ResponseObservable { get; }
        public bool IsConnected => _mqttClient.IsConnected;

        public MqttClient()
        {
            ResponseObservable = new NikoResponseObservable();
        }


        public Task<ConnectionResult> Connect(string hostname, string token, CancellationToken canceltoken = default)
        {
            var tcs = new TaskCompletionSource<ConnectionResult>();

            var factory = new MqttFactory();
            _mqttClient = factory.CreateMqttClient();

            var options = new MqttClientOptionsBuilder()
                .WithClientId(Guid.NewGuid().ToString())
                .WithTcpServer(hostname, 8884)
                .WithCredentials("hobby", token)
                .WithTls(new MqttClientOptionsBuilderTlsParameters
                {
                    UseTls = true,
                    SslProtocol = SslProtocols.Tls12,
                    CertificateValidationCallback =
                        (X509Certificate x, X509Chain y, SslPolicyErrors z, IMqttClientOptions o) => true
                })
                .WithCleanSession()
                .Build();

            _mqttClient.UseConnectedHandler(d =>
            {
                try
                {
                    tcs.TrySetResult(
                        new ConnectionResult((ConnectResultCode)d.AuthenticateResult.ResultCode));
                }
                catch (Exception e)
                {
                    tcs.TrySetException(e);
                }

            });


            _mqttClient.UseApplicationMessageReceivedHandler(d =>
            {
                Logger.WriteInfo($"Message received {d.ApplicationMessage.Topic}");
                var payload = System.Text.Encoding.Default.GetString(d.ApplicationMessage.Payload);
                Logger.WriteReceivedRaw($"Message received { payload }");
                try
                {
                    var msgtype = d.ApplicationMessage.Topic.Split('/').Last();

                    INikoMessage response = null;
                    var doc = JsonDocument.Parse(payload);

                    switch (msgtype)
                    {
                        case "cmd":
                            break;
                        case "evt":
                            var evtparams = JsonDocument.Parse(doc.RootElement.GetProperty("Params").GetRawText()).RootElement;
                            response = new NikoMessage { MessageType  = NikoMessageType.Evt, Method = doc.RootElement.GetProperty("Method").GetString(), Params = evtparams, CorrelationData = Guid.Empty };
                            break;
                        case "rsp":
                            var rspparams = JsonDocument.Parse(doc.RootElement.GetProperty("Params").GetRawText()).RootElement;
                            response = new NikoMessage { MessageType = NikoMessageType.Rsp,  Method = doc.RootElement.GetProperty("Method").GetString(), Params = rspparams, CorrelationData = Guid.Empty};
                            break;
                        case "err":
                            response =new NikoMessage {MessageType = NikoMessageType.Err, Method = doc.RootElement.GetProperty("Method").GetString(), Params = doc };
                            break;
                    }

                   
                    ResponseObservable.MessageReceived(response);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }

            });

            _mqttClient.UseDisconnectedHandler(d => { });


            _mqttClient.ConnectAsync(options, canceltoken);

            return tcs.Task;
        }

        public Task<SubscribeResult> Subscribe()
        {
            var tcs = new TaskCompletionSource<SubscribeResult>();

            var topics = new List<TopicFilter>
            {
                new TopicFilterBuilder().WithTopic("hobby/control/+/rsp").WithQualityOfServiceLevel(MqttQualityOfServiceLevel.AtLeastOnce).Build(),
                new TopicFilterBuilder().WithTopic("hobby/control/+/evt").WithQualityOfServiceLevel(MqttQualityOfServiceLevel.AtLeastOnce).Build(),
                new TopicFilterBuilder().WithTopic("hobby/control/+/err").WithQualityOfServiceLevel(MqttQualityOfServiceLevel.AtLeastOnce).Build()
            };

            _mqttClient.SubscribeAsync(topics.ToArray()).ContinueWith(t =>
            {
                try
                {
                    var failed = t.Result.Items.FirstOrDefault(d => !d.ResultCode.IsSubscriptionSuccess());

                    tcs.TrySetResult(failed == null
                        ? new SubscribeResult(SubscribeResultCode.Success)
                        : new SubscribeResult((SubscribeResultCode) failed.ResultCode,
                            $"Error while subscribing for topic {failed.TopicFilter.Topic}."));
                }
                catch (Exception e)
                {
                    tcs.TrySetException(e);
                }
            });
            return tcs.Task;
        }

        public Task Disconnect()
        {
            return _mqttClient.IsConnected ? _mqttClient.DisconnectAsync() : Task.CompletedTask;
        }

        public Task<PublishResult> PublishMessage(Request request, CancellationToken canceltoken = default)
        {
            var tcs = new TaskCompletionSource<PublishResult>(TaskCreationOptions.RunContinuationsAsynchronously);

            var nativemessage = new MqttApplicationMessageBuilder()
                .WithTopic($"hobby/control/{request.TopicArea.ToLower()}/cmd")
                .WithPayload(request.Payload)
                .WithQualityOfServiceLevel(MqttQualityOfServiceLevel.ExactlyOnce)
                .WithRetainFlag(false)
                .WithCorrelationData(request.Id.ToByteArray())
                .Build();

            _mqttClient.PublishAsync(nativemessage, canceltoken).ContinueWith(d =>
            {
                try
                {
                    if (d.IsFaulted)
                        tcs.TrySetException(d.Exception.InnerException);
                    else if (d.IsCanceled)
                        tcs.TrySetCanceled(canceltoken);
                    else
                    {
                        Logger.WriteInfo($"Publish result : {d.Result.ReasonCode} / {d.Result.ReasonString}");
                        tcs.TrySetResult(new PublishResult((PublishResultCode) d.Result.ReasonCode));
                    }
                }
                catch (Exception e)
                {
                    tcs.TrySetException(e);
                }
            }, canceltoken);

            return tcs.Task;
        }
    }
}
