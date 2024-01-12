// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

namespace Apollo.Common.Entities
{
    /// <summary>
    /// This is part of the original BA Dataset for Machine Learning.
    /// </summary>
    public class Qualification
    {
        public string Id { get; set; }

        // Qualification_Name_filtered.txt
        // FreiText
        public string Name { get; set; }

        // Qualification_Description_filtered.txt
        // FreiText
        public string? Description { get; set; }

        public DateTime? IssueDate { get; set; }

        public DateTime? ExpirationDate { get; set; }

        public string IssuingAuthority { get; set; }
    }
}
