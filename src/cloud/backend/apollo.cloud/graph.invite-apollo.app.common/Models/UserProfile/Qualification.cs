// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System;

namespace Invite.Apollo.App.Graph.Common.Models.UserProfile
{
    public class Qualification
    {
        public string? Id { get; set; }

        // Qualification_Name_filtered.txt
        // FreiText
        public string Name { get; set; }

        // Qualification_Description_filtered.txt
        // FreiText
        public string? Description { get; set; }

        public DateTime? IssueDate { get; set; }

        public DateTime? ExpirationDate { get; set; }
    }
}
