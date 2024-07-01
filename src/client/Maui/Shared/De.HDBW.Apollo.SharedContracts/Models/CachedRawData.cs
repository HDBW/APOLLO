// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using Invite.Apollo.App.Graph.Common.Models;

namespace De.HDBW.Apollo.SharedContracts.Models
{
    public class CachedRawData : BaseItem
    {
        public string? SessionId { get; set; }

        public string? AssesmentId { get; set; }

        public string? ModuleId { get; set; }

        public string? Data { get; set; }
    }
}
