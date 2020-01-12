using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using HC2.Arcanastudio.Net.Client;
using HC2.Arcanastudio.Net.Models;
using HC2.Arcanastudio.Net.Models.Interfaces;
using HC2_Sample_App.UWP.Annotations;

namespace HC2_Sample_App.UWP
{
    public class StatusProperty : INotifyPropertyChanged
    {
        private readonly IDevice _device;
        private bool _isActive;
        public event PropertyChangedEventHandler PropertyChanged;
        public event EventHandler<DeviceCommand> StatusChanged;

        public string DeviceName => _device.Name;

        public string DeviceId => _device.Id;

        public bool IsActive
        {
            get => _isActive;
            set
            {
                if (value == _isActive) return;
                _isActive = value;
                OnPropertyChanged();
                StatusChanged?.Invoke(this, new DeviceCommand(_device.Id, new List<PropertyStatus> { new PropertyStatus(Name, _isActive ? "On" : "Off") }));
            }
        }

        public string Name => "Status";

        public StatusProperty(IDevice device)
        {
            _device = device;
            _isActive = device.Properties.First(d => d.Name.Equals("Status", StringComparison.OrdinalIgnoreCase)).Value.Equals("On", StringComparison.OrdinalIgnoreCase);
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunIdleAsync(d => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName)));
        }
    }
}
