// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using Invite.Apollo.App.Graph.Common.Models.Assessment;
using Invite.Apollo.App.Graph.Common.Models.Course;
using ProtoBuf;

namespace Invite.Apollo.App.Graph.Common.Models.UserProfile
{
    [DataContract]
    [ProtoContract]
    public class AssessmentCategoryResult : BaseItem
    {

        [ForeignKey(nameof(UserProfile))]
        [DataMember(Order = 5)]
        public long UserProfileId { get; set; }

        [DataMember(Order = 6)]
        [ForeignKey(nameof(AssessmentCategory))]
        public long CategoryId { get; set; }

        //The result a user scored in a Category
        [DataMember(Order = 7)]
        public decimal Result { get; set; }

        [ForeignKey(nameof(CourseItem))]
        [DataMember(Order = 8)]
        public long? CourseId { get; set; }

        [ForeignKey(nameof(AssessmentScore))]
        [DataMember(Order = 9)]
        public long AssessmentScoreId { get; set; }

        [DataMember(Order= 10)]
        public bool NeedsRecommendation { get; set; }
    }
}
