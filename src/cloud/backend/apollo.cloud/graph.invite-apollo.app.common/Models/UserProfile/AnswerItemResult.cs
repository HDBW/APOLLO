﻿using System;
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

        [DataMember(Order = 3)]
        public long BackendId { get; set; }

        [DataMember(Order = 4)]
        public Uri Schema { get; set; }

        #endregion

        //TODO: Mapping Table User -> to create several other workloads
        [DataMember(Order = 5)]
        [ForeignKey(nameof(UserProfile))]
        public long UserProfileId { get; set; }

        [DataMember(Order = 6)]
        [ForeignKey(nameof(AssessmentItem))]
        public long AssessmentItemId { get; set; }

        [DataMember(Order = 7)]
        [ForeignKey(nameof(QuestionItem))]
        public long QuestionItemId { get; set; }

        [DataMember(Order = 8)]
        [ForeignKey(nameof(AnswerItem))]
        public long AnswerItemId { get; set; }

        [DataMember(Order = 9)]
        public string Value { get; set; }

        [DataMember(Order = 10)]
        public TimeSpan TimeUserSpent { get; set; }
    }
}
