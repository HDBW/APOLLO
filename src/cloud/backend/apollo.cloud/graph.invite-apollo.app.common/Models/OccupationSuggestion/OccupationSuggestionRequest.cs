// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using ProtoBuf;

namespace OccupationGrpcService.Protos
{
    [ProtoContract]
    public class OccupationSuggestionRequest
    {
        [ProtoMember(1)]
        public string? Input { get; set; }

        [ProtoMember(2)]
        public string? CultureName { get; set; }

        [ProtoMember(3)] public bool? HasApprenticeShip { get; set; }

        [ProtoMember(4)] public bool? IsUniversityOccupation { get; set; }

        [ProtoMember(5)] public bool? IsUniversityDegree { get; set; }

        [ProtoMember(6)] public string CorrelationId { get; set; }
    }
}
