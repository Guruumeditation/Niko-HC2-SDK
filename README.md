
# NikoSDK

<a target="_blank" href="https://opensource.org/licenses/MIT"><img src="https://img.shields.io/badge/license-MIT-blue.svg" alt="License" /></a>
<a target="_blank" href="https://twitter.com/guruumeditation"><img src="https://img.shields.io/twitter/follow/guruumeditation.svg?style=social" /></a>

Niko Home Control 2 SDK for .NET Standard 2.0

## Status

| Target | Branch | Status | SonarCloud | Package version |
|--------------|------------- | --------- | --------| --------|
| Release | master | <a target="_blank" href="https://equinoxe.visualstudio.com/Niko%20Home%20Control%202%20SDK/_build?definitionId=41"><img src="https://equinoxe.visualstudio.com/Niko%20Home%20Control%202%20SDK/_apis/build/status/Master-Publish?branchName=master" alt="Build status" /></a> | <a target="_blank" href="https://sonarcloud.io/dashboard?id=Guruumeditation_Niko-HC2-SDK"><img src="https://sonarcloud.io/api/project_badges/measure?project=Guruumeditation_Niko-HC2-SDK&metric=alert_status" alt="Nuget package" /></a> | <a target="_blank" href="https://www.nuget.org/packages/Net.ArcanaStudio.NikoHC2SDK/"><img src="https://img.shields.io/nuget/v/Net.ArcanaStudio.NikoHC2SDK.svg" alt="Nuget package" /></a> |
| Preview | develop | <a target="_blank" href="https://equinoxe.visualstudio.com/Niko%20Home%20Control%202%20SDK/_build?definitionId=39"><img src="https://equinoxe.visualstudio.com/Niko%20Home%20Control%202%20SDK/_apis/build/status/Develop-Publish?branchName=development" alt="Build status" /></a>| <a target="_blank" href="https://sonarcloud.io/dashboard?id=Guruumeditation_Niko-HC2-SDK"><img src="https://sonarcloud.io/api/project_badges/measure?project=Guruumeditation_Niko-HC2-SDK&metric=alert_status" alt="Nuget package" /></a> | <a target="_blank" href="https://www.nuget.org/packages/Net.ArcanaStudio.NikoHC2SDK/"><img src="https://img.shields.io/nuget/vpre/Net.ArcanaStudio.NikoHC2SDK.svg" alt="Nuget package"/></a> |



## Usage
### Client instantiation

Pass the host name and the Hobby API token (the one generated in your My Niko account) :
```
 var client = new HC2Client(host, token);
```

You can detect the host name if you don't know it :
```
var hostname = HC2Client.DiscoverHost();
```
### Start client


To start the client, use :

```
var result = client.Connect();
```

### Stop client


To stop the client, use :

```
client.Disconnect();
```

### To subscribe to HC2 responses and events


```
client.Subscribe(new MessageObserver((m) => ParseMessage(m))); 

private void MessageReceived(IMessage message)
{
...
}
```

### Query HC2

HC2 is using MQTT as communication. So you send a request, then HC2 will send a response message to the client.
The response message will be an IMessage, that will be parsed in the MessageObserver you pass in the Subscribe method (examples after)
Requests returns a PublishResult, where you can check if the request was successful.

The requests are : 

```
var result = await client.GetDevices();
var result = await client.GetLocations();
var result = await client.GetLocationItems(locationsid); // locationsid is a List<string> of locations ID which you want to get the devices in it
var result = await client.GetSystemInfo();
var result = await client.GetNotifications();
var result = await client.UpdateNotifications(notificationsid); // notificationsid is a List<string> of notifications id you want to set status as read
```

### Execute Command


To execute a command, use :

```
var commands = new List<DeviceCommand>{ new DeviceCommand("xxxx", new Dictionary<string,string>{ {"Brightness","50"}});
var result = await client.SendCommand(commands);
```                       
Where commands is a List<DeviceCommand>.
DeviceCommand needs the Id of the device, and a Key/Value list of device properties name and the value you want to set;
You need to have the device list before (using GetDevices). Then you can check the Properties property, you have there the names of all the properties, their current value, and the property type (bool, choice, range,...) so you'll have all the info on what values are accepted.

### Events

Like said, responses of request and NC2 events are sent to the MessageObserver passed in the Subscribe method.
This is the IMessage interface

```
    public interface IMessage
    {
        string MessageType { get; }
        IError Error { get; }
        bool IsError { get; }
        List<object> Data { get; }
    }
```
So you can see if it is an error message (IsError = true), or a response/event message.
If not en error, you'll get the data sent by HC2 in a List<object>. The type of object depends on the MessageType so you'll have to cast it.

Here is the list :

| MessageType | Data Type | Event or response |
|--------------|------------- | --------- |
| Constants.Messages.DevicesList | List\<Device\> | Response of GetDevices |
| Constants.Messages.DevicesAdded | List\<Device\> | Event if device(s) added |
| Constants.Messages.DevicesRemoved | List\<string\> | Event if device(s) removed (Ids of devices) |
| Constants.Messages.DevicesDisplayNameChanged | List\<DeviceDisplayName\> | Event if device name changed |
| Constants.Messages.DevicesChanged | List\<DevicePropertyDefinitions\> | Event if device property(ies) definition(s) changed |
| Constants.Messages.DevicesParamChanged | List\<DeviceParameters\> | Event if device property(ies) parameter(s) changed  |
| Constants.Messages.DevicesStatus | List\<DeviceProperties\> | Event if device property(ies) value(s) changed. Can be response of a command sent or an event generation physicaly (turning on a light, for instance) |
| Constants.Messages.LocationsList | List\<Location\> | Response of GetLocations |
| Constants.Messages.LocationsListItems | List\<LocationItemsCollection\> | Response of GetLocationItems |
| Constants.Messages.TimePublished | List\<TimeInfo\> | Event sent every 30 seconds by the HC2 |
| Constants.Messages.SysteminfoPublish | List\<SystemInfo\> | Response of GetSystemInfo |
| Constants.Messages.SysteminfoPublished | List\<SystemInfo\> | Event if system info updated |
| Constants.Messages.NotificationsList | List\<Notification\> | Response of GetNotifications |
| Constants.Messages.NotificationsUpdate | List\<Notification\> | Response of UpdateNotifications |
| Constants.Messages.NotificationsRaised | List\<Notification\> | Event when a new notification is raised |

Basic parser implementation : 

```
private void ParseMessage(IMessage message)
{
    if (message.IsError)
    {
       //do something
       return;

    }

    switch (message.MessageType)
    {
        case Constants.Messages.LocationsList:
            var locations = message.Data.Cast<Location>();
            // do something
            break;
        // continue with other message type
    }
}
```

## Limitations

As I don't have alarm, vision or or energy modules, I couldn't test in real conditions.

## History

0.6 (28/12/2019)
- First public beta

## License

```
Copyright 2019 Arcana Studio

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
```
