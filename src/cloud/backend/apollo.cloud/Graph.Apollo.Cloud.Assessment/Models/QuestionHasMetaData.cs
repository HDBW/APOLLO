using System.ComponentModel.DataAnnotations;

namespace Invite.Apollo.App.Graph.Assessment.Models
{
    public class QuestionHasMetaData : BaseItem
    {
        [Required]
        public long QuestionId { get; set; }
        public Question Question { get; set; }

        [Required]
        public long MetaDataId { get; set; }
        public MetaData MetaData { get; set; }
    }
}
