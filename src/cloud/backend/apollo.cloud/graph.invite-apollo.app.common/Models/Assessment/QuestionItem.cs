using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using Invite.Apollo.App.Graph.Common.Models.Assessment.Enums;
using System.Collections.ObjectModel;

namespace Invite.Apollo.App.Graph.Common.Models.Assessment
{
    [DataContract]
    public class QuestionItem : IEntity
    {
        [DataMember(Order = 1, IsRequired = true)]
        [Key]
        public long Id { get; set; }

        [DataMember(Order = 2, IsRequired = true)]
        [ForeignKey(nameof(AssessmentItem))]
        public long AssessmentId { get; set; }

        [DataMember(Order = 3, IsRequired = true)]
        public long Ticks { get; set; }

        [DataMember(Order = 4)]
        public LayoutType QuestionLayout { get; set; }

        [DataMember(Order = 5)]
        public LayoutType AnswerLayout { get; set; }

        [DataMember(Order = 6)]
        public InteractionType Interaction { get; set; }
    }

    [DataContract]
    public class QuestionRequest : ICorrelationId
    {
        [DataMember(Order=1,IsRequired = true)]
        public string CorrelationId { get; set; } = null!;

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
            QuestionMetadata = new Collection<MetaData>();
            MetaDataMetaDataRelation = new Collection<MetaData>();
        }

        [DataMember(Order=1, IsRequired = true)]
        public string CorrelationId { get; set; } = null!;

        [DataMember(Order = 2)]
        public Collection<QuestionItem> Questions { get; set; }

        [DataMember(Order = 3)]
        public Collection<MetaData> QuestionMetadata { get; set; }

        [DataMember(Order = 4)]
        public Collection<MetaData> MetaDataMetaDataRelation { get; set; }

    }
}


