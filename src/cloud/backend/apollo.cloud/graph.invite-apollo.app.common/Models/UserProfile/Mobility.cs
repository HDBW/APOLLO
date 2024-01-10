using System.Collections.Generic;
using Invite.Apollo.App.Graph.Common.Models.UserProfile.Enums;

namespace Invite.Apollo.App.Graph.Common.Models.UserProfile
{
    public class Mobility
    {
        // Mobility_WillingToTravel_filtered.txt
        // Auswahlliste
        public Willing? WillingToTravel { get; set; }

        // Mobility_DriverLicenses_filtered.txt
        // Mehrfachselection
        public List<DriversLicense> DriverLicenses { get; set; }

        public bool HasVehicle { get; set; }
    }
}
