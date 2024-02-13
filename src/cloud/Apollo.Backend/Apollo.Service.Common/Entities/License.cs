// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

namespace Apollo.Common.Entities
{
    /// <summary>
    /// Allowed to be extended by user.
    /// </summary>
    public class License : ExtendableApolloListItem
    {
        /// <summary>
        /// The id of the license in Apollo
        /// </summary>
        //public string? Id { get; set; }

        // License_Name_filtered.txt
        // Freitext
        //public string Name { get; set; }

        public DateTime? Granted { get; set; }

        public DateTime? Expires { get; set; }

        public string? IssuingAuthority { get; set; }
    }
}
