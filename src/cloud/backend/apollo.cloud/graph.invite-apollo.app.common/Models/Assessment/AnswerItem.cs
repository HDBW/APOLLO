using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Runtime.Serialization;
using Invite.Apollo.App.Graph.Common.Models.Course.Assessment.Enums;

namespace Invite.Apollo.App.Graph.Common.Models.Assessment
{
    /// <summary>
    /// Represents the Answers to a Question in a Assessment
    /// </summary>
    [DataContract]
    public class AnswerItem : IEntity
    {
        #region client stuff
        /// <summary>
        /// Id of the Answer used for local store on the client
        /// </summary>
        [DataMember(Order = 1, IsRequired = true)]
        [Key]
        public long Id { get; set; }

        /// <summary>
        /// Indicates the latest time 
        /// </summary>
        [DataMember(Order = 2, IsRequired = true)]
        public long Ticks { get; set; }

        #endregion

        [DataMember(Order = 3, IsRequired = true)]
        public long QuestionId { get; set; }
        
        [DataMember(Order = 4)]
        public AnswerType AnswerType { get; set; }

        [DataMember(Order = 5, IsRequired = true)]
        public string Value { get; set; } = null!;
    }

    [DataContract]
    public class AnswersRequest : ICorrelationId
    {
        public AnswersRequest()
        {
            CorrelationId = string.Empty;
        }

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

        [DataMember(Order = 6)]
        public CultureInfo? CultureInfo { get; set; }
    }

    [DataContract]
    public class AnswerResponse : ICorrelationId
    {
        public AnswerResponse()
        {
            Answers = new List<AnswerItem>();
            MetaData = new List<MetaData>();
            AnswerMetaDataRelations = new List<AnswerMetaDataRelation>();
            MetaDataMetaDataRelations = new List<MetaDataMetaDataRelation>();
        }

        [DataMember(Order = 1, IsRequired = true)]
        public string CorrelationId { get; set; } = null!;

        [DataMember(Order = 2)]
        public List<AnswerItem> Answers { get; set; }

        [DataMember(Order = 3)]
        public List<MetaData> MetaData { get; set; }

        [DataMember(Order = 4)]
        public List<AnswerMetaDataRelation> AnswerMetaDataRelations { get; set; }

        [DataMember(Order = 5)]
        public List<MetaDataMetaDataRelation> MetaDataMetaDataRelations { get; set; }
    } 


}
