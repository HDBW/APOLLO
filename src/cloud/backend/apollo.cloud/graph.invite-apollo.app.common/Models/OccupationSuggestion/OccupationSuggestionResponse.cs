// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Collections.Generic;
using ProtoBuf;

namespace OccupationGrpcService.Protos
{
    [ProtoContract]
    public class OccupationSuggestionResponse
    {
        [ProtoMember(1)]
        public List<OccupationTerm?> OccupationSuggestions { get; set; } = new List<OccupationTerm?>();

        [ProtoMember(2)]
        public string CorrelationId { get; set; }
    }
}
