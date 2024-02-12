// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Collections.Generic;
using Invite.Apollo.App.Graph.Common.Models.Lists;

namespace Invite.Apollo.App.Graph.Common.Models.UserProfile
{
    public class Mobility
    {
        // Mobility_WillingToTravel_filtered.txt
        // Auswahlliste
        public ApolloListItem? WillingToTravel { get; set; }

        // Mobility_DriverLicenses_filtered.txt
        // Mehrfachselection
        public List<ApolloListItem> DriverLicenses { get; set; } = new List<ApolloListItem>();

        public bool HasVehicle { get; set; }
    }
}
