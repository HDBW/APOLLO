using System;

namespace Invite.Apollo.App.Graph.Common.Models.UserProfile
{
    public class Qualification
    {
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
