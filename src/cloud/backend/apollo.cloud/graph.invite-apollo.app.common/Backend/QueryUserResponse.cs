// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Collections.Generic;
using Invite.Apollo.App.Graph.Common.Models.UserProfile;

namespace Invite.Apollo.App.Graph.Common.Backend.Api
{
    public class QueryUserResponse
    {
        public List<User> Users { get; set; }
    }
}
