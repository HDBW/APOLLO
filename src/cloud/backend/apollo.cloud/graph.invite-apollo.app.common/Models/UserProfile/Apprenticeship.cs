// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using Invite.Apollo.App.Graph.Common.Models.Taxonomy;

namespace Invite.Apollo.App.Graph.Common.Models.UserProfile
{
    public class Apprenticeship
    {
        public string? Id { get; set; }

        // Apprenticeship_Kind_filtered.txt
        // Freitext oder Vorschlagsliste
        public Occupation Kind { get; set; }

        public int Years { get; set; }
    }
}
