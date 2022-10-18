using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace Invite.Apollo.App.Graph.Common.Models.Course
{
    [DataContract]
    public class CourseMetaData : IEntity, IApolloGraphItem
    {
        [DataMember(Order = 1, IsRequired = true)]
        public long Id { get; set; }

        [DataMember(Order = 2, IsRequired = true)]
        public long Ticks { get; set; }

        [DataMember(Order = 3, IsRequired = true)]
        public string BackendId { get; set; } = null!;

        [DataMember(Order = 4)]
        public Uri Schema { get; set; } = null!;

        [DataMember(Order = 5)]
        [ForeignKey(nameof(MetaData))]
        public long MetaDataId { get; set; }

        [DataMember(Order = 6)]
        [ForeignKey(nameof(CourseAppointment))]
        public long CourseAppointmentId { get; set; }
    }
}
