using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using Invite.Apollo.App.Graph.Common.Models.Course.Assessment.Enums;

namespace Invite.Apollo.App.Graph.Common.Models.Assessment
{
    [DataContract]
    public class AnswerItem : IEntity
    {
        [DataMember(Order = 1, IsRequired = true)]
        [Key]
        public long Id { get; set; }

        [DataMember(Order = 2, IsRequired = true)]
        public long QuestionId { get; set; }

        [DataMember(Order = 3, IsRequired = true)]
        public long Ticks { get; set; }

        [DataMember(Order = 4)]
        public AnswerType AnswerType { get; set; }

        [DataMember(Order = 5, IsRequired = true)]
        public string Value { get; set; }
    }

    [DataContract]
    public class AnswersRequest : ICorrelationId
    {
        [DataMember(Order = 1,IsRequired = true)]
        public string CorrelationId { get; set; }

        [DataMember(Order=2)]
        public long? AnswerId { get; set; }

        [DataMember(Order = 3)]
        public long? Ticks { get; set; }

        [DataMember(Order = 4)]
        [ForeignKey(nameof(QuestionItem))]
        public long? QuestionId { get; set; }

        [DataMember(Order = 5)]
        [ForeignKey(nameof(AssessmentItem))]
        public long? AssessmentId { get; set; }
    }

    [DataContract]
    public class AnswerResponse : ICorrelationId
    {
        [DataMember(Order = 1,IsRequired = true)]
        public string CorrelationId { get; set; }

        [DataMember(Order = 2)]
        public List<AnswerItem> Answers { get; set; }

        [DataMember(Order = 3)]
        public List<AnswerMetaDataRelation> AnswerMetaDataRelations { get; set; }

        [DataMember(Order = 4)]
        public List<MetaDataMetaDataRelation> MetaDataMetaDataRelations { get; set; }
    } 


}
