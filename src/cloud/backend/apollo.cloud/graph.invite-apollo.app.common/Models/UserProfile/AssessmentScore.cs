﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using Invite.Apollo.App.Graph.Common.Models.Assessment;

namespace Invite.Apollo.App.Graph.Common.Models.UserProfile
{
    [DataContract]
    public class AssessmentScore : BaseItem
    {
       
        /// <summary>
        /// Represents FrontEndId
        /// </summary>
        [DataMember(Order = 5, IsRequired = true)]
        [ForeignKey(nameof(UserProfileItem))]
        public long UserId { get; set; }

        /// <summary>
        /// Represents the ClientId
        /// </summary>
        [DataMember(Order = 6, IsRequired = false)]
        [ForeignKey(nameof(AssessmentItem))]
        public long AssessmentId { get; set; }

        [DataMember(Order = 8, IsRequired = true)]
        public string Value { get; set; }

        /// <summary>
        /// Overall Percentage
        /// </summary>
        [DataMember(Order = 9)]
        public decimal PercentageScore { get; set; }

        

    }
}
