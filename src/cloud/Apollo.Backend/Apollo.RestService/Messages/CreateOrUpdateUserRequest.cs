// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.


using Apollo.Common.Entities;

namespace Apollo.RestService.Apollo.Common.Messages
{
    // Define a request message for creating or updating a user.
    public class CreateOrUpdateUserRequest
    {
        // Property to hold the user to be created or updated
        public User User { get; internal set; }

        //For CreateOrUpdateUser_CreatesUsersSuccessfully() in UserControllerTests
        public CreateOrUpdateUserRequest(User user)
        {
            User = user;
        }
    }
}
