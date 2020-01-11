using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;
using HC2.Arcanastudio.Net;
using HC2.Arcanastudio.Net.Client;
using HC2.Arcanastudio.Net.Models;
using HC2.Arcanastudio.Net.Models.Interfaces;
using HC2.Arcanastudio.Net.Observable;
using HC2_Sample_App.UWP.Annotations;

namespace HC2_Sample_App.UWP
{
    public sealed partial class MainPage
    {
        private readonly MessageObserver _messageObserver;
        private readonly HC2Client _client;
        private bool _updating;

        public List<StatusProperty> Properties { get; set; }

        public MainPage()
        {
            InitializeComponent();

            _messageObserver = new MessageObserver(ParseMessage);
            _client = new HC2Client("FP00112A22318E.local", "eyJhbGciOiJSUzUxMiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJob2JieSIsImlhdCI6MTU3MjM2MTk5NCwiZXhwIjoxNjAzOTg0Mzk0LCJyb2xlIjpbImhvYmJ5Il0sImF1ZCI6IkZQMDAxMTJBMjIzMThFIiwiaXNzIjoibmhjLWNvcmUiLCJqdGkiOiIyOGE2NDg4OC00MTk1LTRiOGUtYmZjZi00ODk0ZmMyMjkyODUifQ.K3Hqd90UQGnucFzsSWcjsHHyl6-LsJNlkJ3NnKNEG_gD9J1eIYjZes1J5BMzrumf8yVkHS_G43ONbdTepaqkcrMWw3vES554MdnGTBQb75PP8iK5sH4pRNDAhdvAJN8c5Fc8Gy0N1BzZ2_W5jQrp1zVIVkCd5_szQFa5HoSYtVHadWJCtUGL6mdtIl2ZE6C8hpC1wy0eEye01y0jYpWZ2wV0FhmPCCe_8YawD66oNiv_i9sZ6H8mnVaRMBEk1WkbE7fYEcdjCyZQAwpPJLTsRMrX8hCakbBdMCSBhzW4jdDDu7DReB_E9jfbNQhC7Qyo4VSgo2xcbSNaAxAEl-n8PA");

        }

        #region Overrides of Page

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            _client.Connect(_messageObserver).ContinueWith(
                t => { _client.GetDevices(); });
            base.OnNavigatedTo(e);
        }

        #endregion

        private void ParseMessage(IMessage message)
        {
            switch (message.MessageType)
            {
                case Constants.Messages.DevicesList:
                    // take only the status property (if it exists)
                    var devices = message.Data.Cast<IDevice>().Where(d => d.Properties.Any(e => e.Name.Equals("Status")) && d.Type == "action").ToList();

                    Properties = devices.Select(ParseDevice).ToList();
                    Dispatcher.RunIdleAsync((d) =>
                    {
                        ActionGridView.ItemsSource = Properties;
                        progress1.IsActive = false;
                        ActionGridView.Visibility = Visibility.Visible;
                    });

                    break;
                case Constants.Messages.DevicesStatus:
                    var devicesproperties = message.Data.Cast<IDevicePropertiesStatus>().ToList();
                    UpdateStatus(devicesproperties);

                    break;
            }
        }

        private void UpdateStatus(List<IDevicePropertiesStatus> newstatuses)
        {
            if (Properties == null)
                return;
            _updating = true;
            foreach (var newstatus in newstatuses)
            {
                var properties = Properties.Where(d => d.DeviceId == newstatus.Id);
                foreach (var newstatusProperty in newstatus.Properties)
                {
                    var property = properties.FirstOrDefault(d => d.Name == newstatusProperty.Name);

                    if (property != null)
                        property.IsActive = newstatusProperty.Value.Equals("On", StringComparison.OrdinalIgnoreCase);

                    Debug.Write($"ID {newstatus.Id}, name {newstatusProperty.Name} value {newstatusProperty.Value}");
                }
            }
            _updating = false;
        }

        private StatusProperty ParseDevice(IDevice device)
        {
            var property = new StatusProperty(device);

            property.StatusChanged += Property_StatusChanged;

            return property;
        }

        private void Property_StatusChanged(object sender, DeviceCommand e)
        {
            if (!_updating)
                _client.SendCommand(new List<DeviceCommand> {e});
        }
    }

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
                StatusChanged?.Invoke(this, new DeviceCommand(_device.Id,new List<PropertyStatus>{ new PropertyStatus(Name,_isActive ? "On" : "Off")}));
            }
        }

        public string Name => "Status";

        public StatusProperty(IDevice device)
        {
            _device = device;
            _isActive = device.Properties.First(d=> d.Name.Equals("Status",StringComparison.OrdinalIgnoreCase)).Value.Equals("On", StringComparison.OrdinalIgnoreCase);
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunIdleAsync(d => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName)));
        }
    }
}
