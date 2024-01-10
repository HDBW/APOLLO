using Invite.Apollo.App.Graph.Common.Models.Taxonomy;

namespace Invite.Apollo.App.Graph.Common.Models.UserProfile
{
    public class Apprenticeship
    {
        // Apprenticeship_Kind_filtered.txt
        // Freitext oder Vorschlagsliste
        public Occupation Kind { get; set; }

        public int Years { get; set; }
    }
}
