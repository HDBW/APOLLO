﻿// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using Invite.Apollo.App.Graph.Common.Models.Assessment;
using ProtoBuf;

namespace Invite.Apollo.App.Graph.Common.Models.UserProfile
{
    [DataContract]
    [ProtoContract]
    public class AnswerItemResult : BaseItem
    {

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
