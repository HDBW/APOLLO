using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Text;
using Invite.Apollo.App.Graph.Common.Models.Assessment;

namespace Invite.Apollo.App.Graph.Common.Models.UserProfile
{
    public class AnswerItemResult : IEntity, IBackendEntity
    {
        #region Implementation of IEntity

        [Key]
        [DataMember(Order = 1)]
        public long Id { get; set; }

        [DataMember(Order = 2)]
        public long Ticks { get; set; }

        #endregion

        #region Implementation of IBackendEntity

        [DataMember()]
        public long BackendId { get; set; }

        [DataMember()]
        public Uri Schema { get; set; }

        #endregion

        [DataMember()]
        [ForeignKey(nameof(QuestionItem))]
        public long QuestionItemId { get; set; }

        [DataMember()]
        [ForeignKey(nameof(AssessmentItem))]
        public long AssessmentItemId { get; set; }

        [DataMember()]
        [ForeignKey(nameof(AnswerItem))]
        public long AnswerItemId { get; set; }

        [DataMember()]
        public string Value { get; set; }
    }
}
