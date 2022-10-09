// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Diagnostics;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Maui.Animations;

namespace app.invite_apollo.TelemetryClientTest
{

    public partial class MainPage : ContentPage
    {
        int _count = 0;
        private TelemetryClient _telemetryClient;

        public MainPage()
        {
            TelemetryConfiguration configuration = TelemetryConfiguration.CreateDefault();
            _telemetryClient = new TelemetryClient(configuration);
            #region telemetry client
            //Basic Information we collect about the device
            _telemetryClient.InstrumentationKey = Environment.GetEnvironmentVariable("INSTRUMENTATIONKEY");
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
            NetworkAccess accessType = Connectivity.Current.NetworkAccess;

            if (accessType == NetworkAccess.Internet)
            {
                //TODO: Implement Telemetry Client Message
                _telemetryClient.TrackTrace($"{_telemetryClient.Context.Session.Id} Clicked on {nameof(sender)}:{sender.ToString()} at: {DateTime.Now.ToString()}");
                _telemetryClient.Flush();
            }
            
            //await DisplayAlert("Alert", Environment.GetEnvironmentVariable("INSTRUMENTATIONKEY"), "OK");

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
