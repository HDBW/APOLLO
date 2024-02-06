// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using Invite.Apollo.App.Graph.Common.Models.Taxonomy;
using ProtoBuf;

namespace OccupationGrpcService.Protos
{
    [ProtoContract]
    public class OccupationTerm
    {
        [ProtoMember(1)]
        public string? Id { get; set; }

        [ProtoMember(2)]
        public string Title { get; set; }

        [ProtoMember(3)]
        public string? OccupationId { get; set; }

        [ProtoMember(4)] public Taxonomy TaxonomyType { get; set; }

        [ProtoMember(5)] public string? CultureName { get; set; }
    }
}
