using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Invite.Apollo.App.Graph.Common.Models.Assessment.Enums;
using System.Collections.ObjectModel;
using System;

namespace Invite.Apollo.App.Graph.Common.Models.Assessment
{
    [DataContract]
    public class QuestionItem : IEntity, IBackendEntity
    {
        #region Implementation of IEntity
        [Key]
        [DataMember(Order = 1)]
        public long Id { get; set; }

        [DataMember(Order = 2, IsRequired = true)]
        public long Ticks { get; set; }

        #endregion

        #region Implementation of IBackendEntity
        [DataMember(Order = 3, IsRequired = true)]
        public long BackendId { get; set; }

        [DataMember(Order = 4, IsRequired = true)]
        public Uri Schema { get; set; } = null!;

        #endregion

        [DataMember(Order = 5, IsRequired = true)]
        [ForeignKey(nameof(AssessmentItem))]
        public long AssessmentId { get; set; }

        [DataMember(Order = 6)]
        public LayoutType QuestionLayout { get; set; }

        [DataMember(Order = 7)]
        public LayoutType AnswerLayout { get; set; }

        [DataMember(Order = 8)]
        public InteractionType Interaction { get; set; }
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


