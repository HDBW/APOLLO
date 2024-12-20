﻿// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using Invite.Apollo.App.Graph.Common.Models.Assessment;
using ProtoBuf;

namespace Invite.Apollo.App.Graph.Common.Models.Course
{
    [DataContract]
    [ProtoContract]
    public class CategoryRecomendationItem : BaseItem
    {
        [DataMember(Order = 5)]
        [ForeignKey(nameof(AssessmentCategory))]
        public long CategoryId { get; set; }

        [DataMember(Order = 6)]
        [ForeignKey(nameof(CourseItem))]
        public long CourseId { get; set; }
    }
}
