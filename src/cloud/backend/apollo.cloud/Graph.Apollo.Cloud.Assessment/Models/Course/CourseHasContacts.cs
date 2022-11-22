using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Invite.Apollo.App.Graph.Assessment.Models.Course
{
    public class CourseHasContacts : BaseItem
    {
        [Required]
        public long CourseId { get; set; }
        [DeleteBehavior(DeleteBehavior.NoAction)]
        [ForeignKey("CourseId")]
        public Course Course { get; set; }

        [Required]
        public long ContactId { get; set; }
        [DeleteBehavior(DeleteBehavior.NoAction)]
        [ForeignKey("ContactId")]
        public Contact Contact { get; set; }
    }
}
