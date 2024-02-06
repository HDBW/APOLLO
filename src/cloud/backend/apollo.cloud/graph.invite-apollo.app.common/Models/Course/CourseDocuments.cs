// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace Invite.Apollo.App.Graph.Common.Models.Course
{
    [DataContract]
    public class CourseDocuments : BaseItem
    {
        
        [ForeignKey(nameof(CourseItem))]
        [DataMember(Order = 5,IsRequired = true)]
        public long CourseId { get; set; }

        [ForeignKey(nameof(DocumentItem))]
        [DataMember(Order = 7, IsRequired = true)]
        public long DocumentId { get; set; }
    }
}
