// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using ProtoBuf;

namespace OccupationGrpcService.Protos
{
    [ProtoContract]
    public class OccupationRequest
    {
        [ProtoMember(1)]
        [global::System.ComponentModel.DataAnnotations.Required]
        public string Id { get; set; }

        [ProtoMember(2)]
        public string? CultureName { get; set; }

        [ProtoMember(3)]
        public string CorrelationId { get; set; }
    }
}
