// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System;

namespace Invite.Apollo.App.Graph.Common.Models.UserProfile
{
    public class WebReference
    {
        public string? Id { get; set; }

        // WebReference_Link_filtered.txt
        // Freitext
        public Uri Url { get; set; }

        // WebReference_Title_filtered.txt
        // Freitext
        public string Title { get; set; }
    }
}
