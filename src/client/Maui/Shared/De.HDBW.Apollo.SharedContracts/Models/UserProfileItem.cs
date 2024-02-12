// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using Invite.Apollo.App.Graph.Common.Models;

namespace De.HDBW.Apollo.SharedContracts.Models
{
    public class UserProfileItem : IEntity
    {
        public long Id { get; set; }

        public long Ticks { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string? Image { get; set; }

        public string? Goal { get; set; }
    }
}
