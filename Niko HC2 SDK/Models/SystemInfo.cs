using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;
using HC2.Arcanastudio.Net.Models.Interfaces;

namespace HC2.Arcanastudio.Net.Models
{
    internal class SystemInfo : ISystemInfo
    {
        private string _lastConfig;

        private DateTime _lastDate;
        private double _waterTariff;
        private double _electricityTariff;
        private double _gasTariff;

        private UnitType _unit;

        [JsonPropertyName("LastConfig")]
        public string LastConfigRaw
        {
            get => _lastConfig;
            set
            {
                _lastConfig = value;
                DateTime.TryParseExact(value, "yyyyMMddHHmmss", CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal, out _lastDate);
            }
        }

        public DateTime LastConfigDate => _lastDate;

        [JsonPropertyName("WaterTariff")]
        public string WaterTariffRaw
        {
            set => double.TryParse(value, out _waterTariff);
        }

        public double WaterPrice => _waterTariff;

        public string Currency { get; set; }
        public string Language { get; set; }

        public string Units
        {
            set => UnitType.TryParse(value, out _unit);
        }

        public UnitType SystemUnit => _unit;

        public string ElectricityTariff
        {
            set => double.TryParse(value, out _electricityTariff);
        }

        public double ElectricityPrice => _electricityTariff;

        public string GasTariff
        {
            set => double.TryParse(value, out _gasTariff);
        }

        public double GasPrice => _gasTariff;

        public JsonElement SWversions
        {
            get;
            set;
        }

        [JsonIgnore]
        public List<KeyValuePair<string,string>> Version { get; set; }
    }

    public enum UnitType
    {
        Metric
    }
}
