// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

namespace Invite.Apollo.App.Graph.Common.Models.UserProfile
{
    public class UserProfile : IEntity
    {
        public long Id { get; set; }

        public long Ticks { get; set; }

        public string? Goal { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string? Image { get; set; }
    }
}
