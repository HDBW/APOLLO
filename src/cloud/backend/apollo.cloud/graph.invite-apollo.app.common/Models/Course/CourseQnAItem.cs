using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using ProtoBuf;

namespace Invite.Apollo.App.Graph.Common.Models.Course
{
    public class CourseQnAItem : BaseItem
    {
        
        [ForeignKey(nameof(CourseItem))]
        [DataMember(Order = 4)]
        public long CourseId { get; set; }

        [ForeignKey(nameof(QnAItem))]
        [DataMember(Order = 4)]
        public long QnAId { get; set; }
    }
}
