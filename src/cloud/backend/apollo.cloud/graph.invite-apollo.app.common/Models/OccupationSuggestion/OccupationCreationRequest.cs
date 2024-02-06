// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using Invite.Apollo.App.Graph.Common.Models.Taxonomy;
using ProtoBuf;

namespace OccupationGrpcService.Protos
{
    [ProtoContract]
    public class OccupationCreationRequest
    {
        [ProtoMember(1)]
        public Occupation? Occupation { get; set; }

        [ProtoMember(2)]
        public string CorrelationId { get; set; }
    }
}
