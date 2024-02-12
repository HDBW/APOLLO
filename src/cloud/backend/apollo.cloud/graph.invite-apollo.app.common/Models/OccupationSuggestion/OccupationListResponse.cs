// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Collections.Generic;
using Invite.Apollo.App.Graph.Common.Models.Taxonomy;
using ProtoBuf;

namespace OccupationGrpcService.Services
{
    [ProtoContract]
    public class OccupationListResponse
    {
        [ProtoMember(1)]
        public List<Occupation> Occupations { get; set; }

        [ProtoMember(2)]
        public string CorrelationId { get; set; }
    }
}
