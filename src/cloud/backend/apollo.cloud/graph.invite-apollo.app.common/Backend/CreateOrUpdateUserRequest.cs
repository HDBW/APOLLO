// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Text;
using Invite.Apollo.App.Graph.Common.Models.UserProfile;

namespace Invite.Apollo.App.Graph.Common.Backend.Api
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
