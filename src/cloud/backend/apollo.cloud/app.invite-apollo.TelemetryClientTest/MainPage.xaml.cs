// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
using Microsoft.ApplicationInsights;
using Microsoft.Maui.Animations;

namespace app.invite_apollo.TelemetryClientTest
{

    public partial class MainPage : ContentPage
    {
        int _count = 0;
        private TelemetryClient _telemetryClient = new TelemetryClient();

        public MainPage()
        {
            #region telemetry client
            //Basic Information we collect about the device
            _telemetryClient.InstrumentationKey = Environment.GetEnvironmentVariable("APPLICATIONINSIGHTS_CONNECTION_STRING");
            _telemetryClient.Context.Device.OemName = DeviceInfo.Manufacturer;
            _telemetryClient.Context.Device.Model = DeviceInfo.Current.Model;
            _telemetryClient.Context.Device.Type = DeviceInfo.Current.DeviceType.ToString();
            _telemetryClient.Context.Device.OperatingSystem = DeviceInfo.Current.VersionString;
            //TODO: Add _telemetryClient.Context.Device.Platform = DeviceInfo.Current.Platform;

            //Basic Information we collect about the user
            _telemetryClient.Context.Session.Id = Guid.NewGuid().ToString();
            //TODO: _telemetryClient.Context.User = ; 
            #endregion telemetry client end

            InitializeComponent();
        }

        private async void OnCounterClicked(object sender, EventArgs e)
        {
            //TODO: Implement Telemetry Client Message
            _telemetryClient.TrackTrace($"{_telemetryClient.Context.Session.Id} Clicked on {nameof(sender)}:{sender.ToString()} at: {DateTime.Now.ToString()}");
            _telemetryClient.Flush();

            //await DisplayAlert("Alert", AppInsights, "OK");

            //Suggestion: TrackEvent instead? and metrics?

            _count++;
            
            if (_count == 1)
                CounterBtn.Text = $"Clicked {_count} time";
            else
                CounterBtn.Text = $"Clicked {_count} times";

            SemanticScreenReader.Announce(CounterBtn.Text);
        }


    }
}
