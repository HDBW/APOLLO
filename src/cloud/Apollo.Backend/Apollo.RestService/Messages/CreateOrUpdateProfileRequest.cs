// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.


using Apollo.Common.Entities;

namespace Apollo.RestService.Apollo.Common.Messages
{
    // Define a request message for creating or updating a profile.
    public class CreateOrUpdateProfileRequest
    {
        // Property to hold the profile to be created or updated
        public Profile Profile { get; internal set; }

        //For CreateOrUpdateProfile_CreatesProfilessSuccessfully() in ProfileControllerTests
        public CreateOrUpdateProfileRequest(Profile profile)
        {
            Profile = profile;
        }
    }
}
