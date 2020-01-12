using System;
using System.Collections.Generic;

namespace HC2.Arcanastudio.Net.Models.Interfaces
{
    public interface ISystemInfo
    {
        DateTime LastConfigDate { get; }
        double WaterPrice { get; }
        string Currency { get; set; }
        string Language { get; set; }
        string Units { set; }
        UnitType SystemUnit { get; }
        double ElectricityPrice { get; }
        double GasPrice { get; }
        List<KeyValuePair<string, string>> Version { get; set; }
    }
}