using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace Invite.Apollo.App.Graph.Common.Models.Assessment
{
    /// <summary>
    /// Defines the possible Scoring Possibilities and Outcomes
    /// </summary>
    [DataContract]
    public class AssessmentScoringItem : IEntity, IBackendEntity
    {
        #region Implementation of IEntity

        [DataMember(Order = 1, IsRequired = false)]
        public long Id { get; set; }
        [DataMember(Order = 2, IsRequired = true)]
        public long Ticks { get; set; }

        #endregion

        #region Implementation of IBackendEntity

        [DataMember(Order = 3, IsRequired = true)]
        public long BackendId { get; set; }

        [DataMember(Order = 4, IsRequired = true)]
        public Uri Schema { get; set; }

        #endregion

        [ForeignKey(nameof(AssessmentItem))]
        [DataMember(Order = 5, IsRequired = true)]
        public long AssessmentBackendId { get; set; }

        [ForeignKey(nameof(QuestionItem))]
        [DataMember(Order = 6, IsRequired = true)]
        public long QuestionBackendId { get; set; }

        [ForeignKey(nameof(AnswerItem))]
        [DataMember(Order = 7, IsRequired = true)]
        public long AnswerBackendId { get; set; }

        /// <summary>
        /// Scoring Option as bitmask
        /// </summary>
        [DataMember(Order =8, IsRequired = true)]
        public int ScoringOption { get; set; }

        [DataMember(Order = 9,IsRequired = true)]
        public string OccupationResult { get; set; } = string.Empty;

        [DataMember(Order = 10,IsRequired = true)]
        public string SkillResult { get; set; } = string.Empty!;
    }
}
