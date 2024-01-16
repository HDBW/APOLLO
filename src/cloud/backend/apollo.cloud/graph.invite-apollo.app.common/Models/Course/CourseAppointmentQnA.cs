using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace Invite.Apollo.App.Graph.Common.Models.Course
{
    public class CourseAppointmentQnA : BaseItem
    {
        [ForeignKey(nameof(CourseAppointment))]
        [DataMember(Order = 5,IsRequired = true)]
        public long CourseAppointmentId { get; set; }

        [ForeignKey(nameof(QnAItem))]
        [DataMember(Order = 6, IsRequired = true)]
        public long QnAId { get; set; }
    }
}
