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

        /// <summary>
        /// Get Occupations that are apprenticeships. Dual and normal apprenticeships.
        /// </summary>
        [ProtoMember(3)] public bool? HasApprenticeShip { get; set; }

        /// <summary>
        /// Get a list of all Occupations that are university occupations.
        /// </summary>
        [ProtoMember(4)] public bool? IsUniversityOccupation { get; set; }

        /// <summary>
        /// Get a list of all Occupations that are university degrees.
        /// </summary>
        [ProtoMember(5)] public bool? IsUniversityDegree { get; set; }

        /// <summary>
        /// Get all Occupations that are apprenticeships including
        /// degrees, military, goverment related, everything.
        /// </summary>
        [ProtoMember(6)] public bool? DkzApprenticeship { get; set; }

        /// <summary>
        /// Get Occupations that are qualified professionals by a training or a degree.
        /// </summary>
        [ProtoMember(7)] public bool? QualifiedProfessional { get; set; }


        [ProtoMember(8)] public string CorrelationId { get; set; }
    }
}
