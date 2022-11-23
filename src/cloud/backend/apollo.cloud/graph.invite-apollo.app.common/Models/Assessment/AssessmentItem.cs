using System;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using Invite.Apollo.App.Graph.Common.Models.Assessment.Enums;
using ProtoBuf;
using ProtoBuf.Grpc;

namespace Invite.Apollo.App.Graph.Common.Models.Assessment
{
    [DataContract]
    //TODO: Rename
    public class AssessmentItem : BaseItem
    {
        public AssessmentItem()
        {
        }

        [DataMember(Order = 5)]
        public string Title { get; set; } = string.Empty;

        [DataMember(Order = 6)]
        public string Profession { get; set; } = string.Empty;

        [DataMember(Order = 7)]
        public string Kldb { get; set; } = string.Empty;

        [DataMember(Order = 8)]
        public string EscoOccupationId { get; set; } = string.Empty;

        [DataMember(Order = 9)]
        public AssessmentType AssessmentType { get; set; }

        [DataMember(Order = 10)]
        public string Publisher { get; set; }

        // Will be auto calculated in the future
        [DataMember(Order = 11)]
        public string Duration { get; set; }

        [DataMember(Order = 12)]
        public string Description { get; set; } = string.Empty;

        [DataMember(Order = 13)]
        public string Disclaimer { get; set; }
    }

    [DataContract]
    public class AssessmentRequest : ICorrelationId
    {
        [DataMember(Order = 1, IsRequired = true)]
        public string CorrelationId { get; set; } = null!;

        [DataMember(Order = 2)]
        public string? EscoOccupationId { get; set; }

        [DataMember(Order = 3)]
        public long? Ticks { get; set; }
    }

    [DataContract]
    public class AssessmentResponse : ICorrelationId
    {
        public AssessmentResponse()
        {
            Assessments = new Collection<AssessmentItem>();
        }

        [DataMember(Order = 1, IsRequired = true)]
        public string CorrelationId { get; set; } = string.Empty;

        [DataMember(Order = 2)]
        public Collection<AssessmentItem> Assessments { get; set; }
    }

}
