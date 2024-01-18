// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System;

namespace Invite.Apollo.App.Graph.Common.Models.UserProfile
{
    /// <summary>
    /// This is the License a User has.
    /// This information is relevant for User Profile classification.
    /// This information is not PII relevant.
    /// </summary>
    public class License
    {
        /// <summary>
        /// License as of defined in the BA Dataset for Machine Learning.
        /// See Sample Data for more Information.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The Date the License was granted.
        /// </summary>
        public DateTime? Granted { get; set; }

        /// <summary>
        /// The Date the License expires.
        /// </summary>
        public DateTime? Expires { get; set; }
    }
}
