using System.ComponentModel;
using HC2.Arcanastudio.Net.Log;
using MQTTnet.Client.Subscribing;
using Serilog.Events;

namespace HC2.Arcanastudio.Net.Extensions
{
    public static class EnumExtensions
    {
        internal static bool IsSubscriptionSuccess(this MqttClientSubscribeResultCode code)
        {
            return (code == MqttClientSubscribeResultCode.GrantedQoS0) ||
                   (code == MqttClientSubscribeResultCode.GrantedQoS1) ||
                   (code == MqttClientSubscribeResultCode.GrantedQoS2);
        }

        internal static bool CheckLevel(this LogLevel level, LogLevel tocheck)
        {
            return (level & tocheck) == tocheck;
        }
    }
}
