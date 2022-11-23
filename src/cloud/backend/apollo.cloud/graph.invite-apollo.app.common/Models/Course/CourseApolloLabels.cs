using System.ComponentModel.DataAnnotations.Schema;
using ProtoBuf;

namespace Invite.Apollo.App.Graph.Common.Models.Course
{
    public class CourseApolloLabels : BaseItem
    {
        [ForeignKey(nameof(CourseItem))]
        public long CourseId { get; set; }

        [ForeignKey(nameof(ApolloLabel))]
        public long LabelId { get; set; }
    }
}
