﻿using System.Collections.Generic;

namespace HC2.Arcanastudio.Net.Models.Interfaces
{
    public interface IDeviceCommand
    {
        string DeviceId { get; }
        List<PropertyStatus> Commands { get; }
    }
}