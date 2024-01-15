// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

namespace De.HDBW.Apollo.Client.Messages
{
    public class NetworkStatusChangeMessage
    {
        public NetworkStatusChangeMessage(bool hasNetworkConnection)
        {
            HasNetworkConnection = hasNetworkConnection;
        }

        public bool HasNetworkConnection { get; }
    }
}
