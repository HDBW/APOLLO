// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Collections.Generic;

namespace Invite.Apollo.App.Graph.Common.Models.UserProfile
{
    public class Knowledge
    {
        // Knowledge_Advanced_filtered.txt
        // Freitext
        public List<string> Advanced { get; set; }

        // Knowledge_Basic_filtered.txt
        // Freitext
        public List<string> Basic { get; set; }

        // Knowledge_Expert_filtered.txt
        // Freitext
        public List<string> Expert { get; set; }
    }
}
