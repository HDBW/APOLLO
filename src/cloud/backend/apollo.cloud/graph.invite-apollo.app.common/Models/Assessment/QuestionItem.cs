// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using Invite.Apollo.App.Graph.Common.Models.Assessment.Enums;
using System.Collections.ObjectModel;
using ProtoBuf;

namespace Invite.Apollo.App.Graph.Common.Models.Assessment
{
    [DataContract]
    [ProtoContract]
    public class QuestionItem : BaseItem
    {


        [DataMember(Order = 5, IsRequired = true)]
        [ProtoMember(1)]
        [ForeignKey(nameof(AssessmentItem))]
        public long AssessmentId { get; set; }

        [DataMember(Order = 6)]
        [ProtoMember(2)]
        public LayoutType QuestionLayout { get; set; }

        [DataMember(Order = 7)]
        [ProtoMember(3)]
        public LayoutType AnswerLayout { get; set; }

        [DataMember(Order = 8)]
        [ProtoMember(4)]
        public InteractionType Interaction { get; set; }

        [DataMember(Order = 9)]
        [ProtoMember(5)]
        [ForeignKey(nameof(AssessmentCategory))]
        public long CategoryId { get; set; }

        [DataMember(Order = 10)]
        [ProtoMember(6)]
        public string ScoringOption { get; set; } = string.Empty;

        [DataMember(Order = 11)]
        [ProtoMember(7)]
        public int Scalar { get; set; }

        [DataMember(Order = 12, IsRequired = true)]
        [ProtoMember(8)]
        public QuestionType QuestionType { get; set; }
    }

    [DataContract]
    public class QuestionRequest : ICorrelationId
    {
        [DataMember(Order=1,IsRequired = true)]
        public string CorrelationId { get; set; } = string.Empty;

        [DataMember(Order = 2)]
        [ForeignKey(nameof(AssessmentItem))]
        public long? AssessmentId { get; set; }

        [DataMember(Order = 3)]
        [ForeignKey(nameof(QuestionItem))]
        public long? QuestionId { get; set; }

        [DataMember(Order = 4)]
        public long? Ticks { get; set; }

        //TODO: stuff
    }

    [DataContract]
    public class QuestionResponse : ICorrelationId
    {
        public QuestionResponse()
        {
            Questions = new Collection<QuestionItem>();
            QuestionMetadata = new Collection<MetaDataItem>();
            MetaDataMetaDataRelation = new Collection<MetaDataItem>();
        }

        [DataMember(Order=1, IsRequired = true)]
        public string CorrelationId { get; set; } = string.Empty;

        [DataMember(Order = 2)]
        public Collection<QuestionItem> Questions { get; set; }

        [DataMember(Order = 3)]
        public Collection<MetaDataItem> QuestionMetadata { get; set; }

        [DataMember(Order = 4)]
        public Collection<MetaDataItem> MetaDataMetaDataRelation { get; set; }

    }
}


