using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using ProtoBuf;

namespace Invite.Apollo.App.Graph.Common.Models.Course
{
    [DataContract]
    public class CourseMetaData : BaseItem
    {
        [DataMember(Order = 5)]
        [ForeignKey(nameof(MetaDataItem))]
        public long MetaDataId { get; set; }

        [DataMember(Order = 6)]
        [ForeignKey(nameof(CourseAppointment))]
        public long CourseAppointmentId { get; set; }
    }
}
