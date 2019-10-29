using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using HC2.Arcanastudio.Net.Client.Messages;
using HC2.Arcanastudio.Net.Client.Results;
using HC2.Arcanastudio.Net.Log;
using HC2.Arcanastudio.Net.Models;
using HC2.Arcanastudio.Net.Models.Interfaces;
using HC2.Arcanastudio.Net.Observable;
using HC2.Arcanastudio.Net.Parsers;
using HC2.Arcanastudio.Net.RequestSerializers;
using Zeroconf;
using Message = HC2.Arcanastudio.Net.Models.Message;

namespace HC2.Arcanastudio.Net.Client
{
    public class HC2Client
    {
        private readonly string _token;

        private readonly string _hostname;

        private readonly INativeMqttClient _mqttclient;

        private readonly MessageObservable _messageObservable = new MessageObservable();

        private IDisposable _unsubscriber;

        public bool IsConnected => _mqttclient.IsConnected;

        public HC2Client(string hostname, string token)
        {
            _token = token ?? throw new ArgumentNullException(nameof(token));
            _hostname = hostname ?? throw new ArgumentNullException(nameof(hostname));

            _mqttclient = new MqttClient();
        }        
        
        internal HC2Client(INativeMqttClient nativemqttclient,string hostname, string token)
        {
            _token = token ?? throw new ArgumentNullException(nameof(token));
            _hostname = hostname ?? throw new ArgumentNullException(nameof(hostname));

            _mqttclient = nativemqttclient;

            var observer = new NikoresponseObserver(ParseMessage);

            _mqttclient.ResponseObservable.Subscribe(observer);
        }

        public Task<ConnectionResult> Connect(CancellationToken canceltoken = default)
        {
            var tcs = new TaskCompletionSource<ConnectionResult>();

            _mqttclient.Connect(_hostname, _token, canceltoken).ContinueWith(t =>
                {
                    try
                    {
                        if (t.IsFaulted)
                            tcs.TrySetException(t.Exception.InnerException);
                        else if (t.IsCanceled)
                            tcs.TrySetCanceled(canceltoken);
                        else
                        {
                            _unsubscriber = _mqttclient.ResponseObservable.Subscribe(new NikoresponseObserver(ParseMessage));
                            tcs.TrySetResult(new ConnectionResult(t.Result.ResultCode));
                        }
                    }
                    catch (Exception e)
                    {
                        tcs.TrySetException(e);
                    }
                }, canceltoken);

             return tcs.Task;
        }

        public Task<SubscribeResult> Subscribe(MessageObserver observer)
        {
            var tcs = new TaskCompletionSource<SubscribeResult>();

            _mqttclient.Subscribe().ContinueWith(t =>
            {
                try
                {
                    _messageObservable.Subscribe(observer);
                    tcs.TrySetResult(t.Result);
                }
                catch (Exception e)
                {
                    tcs.TrySetException(e);
                }
            });

            return tcs.Task;
        }

        public Task<PublishResult> GetDevices(CancellationToken canceltoken = default)
        {
            return SendMessage(Constants.Messages.DevicesList, null, canceltoken);
        }

        public Task<PublishResult> SendCommand(List<DeviceCommand> commands,CancellationToken canceltoken = default)
        {
            return SendMessage(Constants.Messages.DevicesControl, commands, canceltoken);
        }       
        public Task<PublishResult> GetLocations(CancellationToken canceltoken = default)
        {
            return SendMessage(Constants.Messages.LocationsList, null, canceltoken);
        }        
        
        public Task<PublishResult> GetLocationItems(List<string> locationsid,CancellationToken canceltoken = default)
        {
            return SendMessage(Constants.Messages.LocationsListItems, locationsid, canceltoken);
        }

        public Task<PublishResult> GetSystemInfo(CancellationToken canceltoken = default)
        {
            return SendMessage(Constants.Messages.SysteminfoPublish, null, canceltoken);
        }

        public Task<PublishResult> GetNotifications(CancellationToken canceltoken = default)
        {
            return SendMessage(Constants.Messages.NotificationsList, null, canceltoken);
        }

        public Task<PublishResult> UpdateNotifications(List<string> notificationsid, CancellationToken canceltoken = default)
        {
            return SendMessage(Constants.Messages.NotificationsUpdate, notificationsid, canceltoken);
        }

        private Task<PublishResult> SendMessage(string method,object payloadobject, CancellationToken canceltoken = default)
        {
            var tcs = new TaskCompletionSource<PublishResult>(TaskCreationOptions.RunContinuationsAsynchronously);

            var payloadserializer = RequestSerializersFactory.GetParser(method);

            var request = new Request(method,payloadserializer.Serialize(method, payloadobject));

            _mqttclient.PublishMessage(request, canceltoken).ContinueWith(t =>
            {
                try
                {
                    if (t.IsFaulted)
                        tcs.TrySetException(t.Exception.InnerException);
                    else if (t.IsCanceled)
                        tcs.TrySetCanceled(canceltoken);
                    else
                        tcs.TrySetResult(t.Result);
                }
                catch (Exception e)
                {
                    tcs.TrySetException(e);
                }
            }, canceltoken);

            return tcs.Task;
        }

        public Task Disconnect()
        {
            _unsubscriber?.Dispose();
            _unsubscriber = null;
            return _mqttclient.Disconnect();
        }

        #region Parsers

        private void ParseMessage(INikoMessage message)
        {
            switch (message.MessageType)
            {
                case NikoMessageType.Rsp:
                case NikoMessageType.Evt:
                    ParseResponse((NikoMessage)message);
                    break;
                case NikoMessageType.Err:
                    ParseError((NikoMessage)message);
                    break;
            }
           
        }

        private void ParseResponse(NikoMessage message)
        {
            List<object> data;
            var parser = PayloadParserFactory.GetParser(message.Method);
            try
            {
                data = parser.Parse((JsonElement)message.Params);
            }
            catch (Exception e)
            {
                var error = new ResponseError(Constants.Errors.MessageParsingError, e.Message, message.Method);
                _messageObservable.MessageReceived(new Message(message.Method, error));
                return;
            }

            _messageObservable.MessageReceived(new Message(message.Method, data));

            if (Logger.IsLog)
            {
                Logger.WriteInfo($"{data?.Count ?? 0} item(s) received.");
            }
            
            if (Logger.IsLog && data != null)
            {
                Logger.WriteReceived(JsonSerializer.Serialize(data));
            }
        }

        private void ParseError(NikoMessage message)
        {
            var parser = PayloadParserFactory.GetParser(Constants.Messages.Error);
            var errorraw = parser.Parse((JsonElement)message.Params).Cast<IError>().ToList();

            var error = new ResponseError(errorraw.First().ErrCode, errorraw.First().ErrMessage, message.Method);
            _messageObservable.MessageReceived(new Message(message.Method, error));
        }

        #endregion

        public static async Task<string> DiscoverHost(LogConfiguration logconfig = null)
        {
            var domains = await ZeroconfResolver.BrowseDomainsAsync();
            var responses = await ZeroconfResolver.ResolveAsync(domains.Select(g => g.Key));

            if (Logger.IsLog)
            {
                foreach (var response in responses)
                {
                    Logger.WriteInfo(response.ToString());
                }
            }

            return responses.Where(d => d.DisplayName.StartsWith("FP", StringComparison.OrdinalIgnoreCase))
                .Select(d => $"{d.DisplayName}.local").FirstOrDefault();
        }

        #region Log

        public void SetLog(LogConfiguration config)
        {
            Logger.Configuration = config;
        }

        #endregion
    }
}
