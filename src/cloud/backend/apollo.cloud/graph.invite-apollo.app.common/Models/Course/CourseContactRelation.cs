// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace Invite.Apollo.App.Graph.Common.Models.Course
{
    [DataContract]
    public class CourseContactRelation : BaseItem
    {
        [DataMember(Order = 1, IsRequired = true)]
        [ForeignKey(nameof(CourseItem))]
        public long CourseId { get; set; }

        [DataMember(Order = 2, IsRequired = true)]
        [ForeignKey(nameof(CourseContact))]
        public long CourseContactId { get; set; }
    }
}
