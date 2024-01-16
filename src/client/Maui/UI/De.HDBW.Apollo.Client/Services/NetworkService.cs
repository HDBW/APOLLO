// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using CommunityToolkit.Mvvm.Messaging;
using De.HDBW.Apollo.Client.Contracts;
using De.HDBW.Apollo.Client.Messages;
using Microsoft.Extensions.Logging;

namespace De.HDBW.Apollo.Client.Services
{
    public class NetworkService : INetworkService
    {
        public NetworkService(ILogger<NetworkService> logger, IMessenger messenger)
        {
            Messenger = messenger;
            Logger = logger;

            Connectivity.ConnectivityChanged += HandleConnectivityChanged;
            OnChanged();
        }

        public bool HasNetworkConnection
        {
            get
            {
                return Connectivity.NetworkAccess == NetworkAccess.Internet;
            }
        }

        public bool HasWifi
        {
            get { return Connectivity.ConnectionProfiles?.Contains(ConnectionProfile.WiFi) ?? false; }
        }

        private IMessenger Messenger { get; set; }

        private ILogger Logger { get; set; }

        private void HandleConnectivityChanged(object? sender, ConnectivityChangedEventArgs e)
        {
            OnChanged();
        }

        private void OnChanged()
        {
            Logger?.LogInformation($"Networkstate changed. HasNetworkConnection: {HasNetworkConnection} - HasWifi: {HasWifi} - NetworkAccess: {Connectivity.NetworkAccess} - ConnectionProfiles: {string.Join(",", Connectivity.ConnectionProfiles)}");
            Messenger.Send(new NetworkStatusChangeMessage(HasNetworkConnection));
        }
    }
}
