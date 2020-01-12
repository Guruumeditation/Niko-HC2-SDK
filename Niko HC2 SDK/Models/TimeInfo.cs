using System;
using System.Globalization;
using System.Text.Json.Serialization;
using HC2.Arcanastudio.Net.Models.Interfaces;

namespace HC2.Arcanastudio.Net.Models
{
    internal class TimeInfo : ITimeInfo
    {
        private string _utcTimeRaw;
        private DateTime _utcTime;
        public string GMTOffset { get; set; }

        public string Timezone { get; set; }

        [JsonPropertyName("UTCTime")]
        public string UtcTimeRaw
        {
            get => _utcTimeRaw;
            set
            {
                _utcTimeRaw = value;
                DateTime.TryParseExact(value, "yyyyMMddHHmmss", CultureInfo.InvariantCulture,  DateTimeStyles.AssumeLocal,out _utcTime);
            }
        }

        public DateTime UtcTime => _utcTime;

        [JsonPropertyName("IsDST")]
        public string IsDstRaw { get; set; }

        public bool IsDst => IsDstRaw == "1";
    }
}
