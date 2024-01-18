// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace Invite.Apollo.App.Graph.Common.Models.Course
{
    /// <summary>
    /// Defines a list of Benefits for a specific Course.
    /// </summary>
    [DataContract]
    public class CourseBenefits : BaseItem
    {
        [DataMember(Order = 5, IsRequired = true)]
        [ForeignKey(nameof(CourseItem))]
        public long CourseId { get; set; }

        [DataMember(Order = 6)]
        public string Value { get; set; } = string.Empty;

        [DataMember(Order = 7)]
        public Uri Image { get; set; } = null!;

        //TODO: Implement Qualification

    }
}
