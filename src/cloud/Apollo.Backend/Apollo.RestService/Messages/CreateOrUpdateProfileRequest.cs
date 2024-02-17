// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.


using Apollo.Common.Entities;

namespace Apollo.RestService.Apollo.Common.Messages
{
    /// <summary>
    /// Defines the request for creating or updating a profile.
    /// </summary>
    public class CreateOrUpdateProfileRequest
    {
        /// <summary>
        /// The Id of the user to whom the profile belongs.
        /// </summary>
        public  string UserId { get; set; }

        /// <summary>
        ///  Property to hold the profile to be created or updated
        /// </summary>
        public Profile Profile { get; set; }

    }
}
