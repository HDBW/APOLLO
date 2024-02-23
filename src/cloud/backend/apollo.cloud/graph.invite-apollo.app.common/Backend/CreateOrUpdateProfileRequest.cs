// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using Invite.Apollo.App.Graph.Common.Models.UserProfile;

namespace Invite.Apollo.App.Graph.Common.Backend.Api
{
    /// <summary>
    /// Defines the request for creating or updating a profile.
    /// </summary>
    public class CreateOrUpdateProfileRequest
    {
        /// <summary>
        /// The Id of the user to whom the profile belongs.
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        ///  Property to hold the profile to be created or updated
        /// </summary>
        public Profile Profile { get; set; }

    }
}
