using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Text;

namespace Invite.Apollo.App.Graph.Common.Models.Course
{
    [DataContract]
    public class CourseAppointment : IEntity, IApolloGraphItem
    {
        [Key]
        [DataMember(Order = 1,IsRequired = true)]
        public long Id { get; set; }

        [DataMember(Order = 2)]
        public long Ticks { get; set; }

        [DataMember(Order = 3)]
        [ForeignKey(nameof(CourseItem))]
        public long CourseId { get; set; }

        [DataMember(Order = 4,IsRequired = true)]
        public string BackendId { get; set; }

        [DataMember(Order = 5)]
        public Uri Schema { get; set; }
    }
}
