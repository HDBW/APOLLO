// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using Invite.Apollo.App.Graph.Common.Models.Taxonomy;
using OccupationGrpcService.Protos;
using ProtoBuf;

namespace OccupationGrpcService.Services
{
    [ProtoContract]
    public class OccupationListRequest
    {
        [ProtoMember(1)]
        public Taxonomy Taxonomy { get; set; }

        [ProtoMember(2)]
        public string CorrelationId { get; set; }
    }
}
