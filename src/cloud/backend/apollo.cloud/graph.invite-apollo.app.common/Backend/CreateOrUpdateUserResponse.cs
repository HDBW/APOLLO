// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Text;
using Invite.Apollo.App.Graph.Common.Models.UserProfile;

namespace Invite.Apollo.App.Graph.Common.Backend.Api
{
    public class CreateOrUpdateUserResponse
    {
        // Property to store the result of the create/update operation.
        public User? Result { get; set; }
    }
}
