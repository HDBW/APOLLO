using System.ComponentModel.DataAnnotations;

namespace Invite.Apollo.App.Graph.Assessment.Models
{
    public class CategoryRecomendation : BaseItem
    {

        [Required]
        public long CategoryId { get; set; }
        public Category Category { get; set; }

        [Required]
        public long CourseId { get; set; }
    }
}
