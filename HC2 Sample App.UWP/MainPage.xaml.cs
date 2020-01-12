using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;
using HC2.Arcanastudio.Net;
using HC2.Arcanastudio.Net.Client;
using HC2.Arcanastudio.Net.Models.Interfaces;
using HC2.Arcanastudio.Net.Observable;

namespace HC2_Sample_App.UWP
{
    public sealed partial class MainPage
    {
        private readonly MessageObserver _messageObserver;
        private HC2Client _client;
        private bool _updating;
        private string _hostname = ""; // Put hostname here
        private string _token = ""; // Put token here

        public List<StatusProperty> Properties { get; set; }

        public MainPage()
        {
            InitializeComponent();

            _messageObserver = new MessageObserver(ParseMessage);
        }

        #region Overrides of Page

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (string.IsNullOrEmpty(_hostname) || string.IsNullOrEmpty(_token))
            {
                var dialog = new MessageDialog("Token or hostanme not set. Set it in MainPage.cs");
                dialog.ShowAsync();
            }
            else
            {
                _client = new HC2Client(_hostname, _token);
                _client.Connect(_messageObserver).ContinueWith(
                    t => { _client.GetDevices(); });
            }

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
                var properties = Properties.Where(d => d.DeviceId == newstatus.Id).ToList();
                foreach (var newstatusProperty in newstatus.Properties)
                {
                    var property = properties.FirstOrDefault(d => d.Name == newstatusProperty.Name);

                    if (property != null)
                        property.IsActive = newstatusProperty.Value.Equals("On", StringComparison.OrdinalIgnoreCase);
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
}
