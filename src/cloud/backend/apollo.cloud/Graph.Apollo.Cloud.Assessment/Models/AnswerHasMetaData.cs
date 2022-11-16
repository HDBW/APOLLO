using System.ComponentModel.DataAnnotations;

namespace Invite.Apollo.App.Graph.Assessment.Models
{
    public class AnswerHasMetaData
    {
        [Required]
        public long AnswerId { get; set; }
        public Answer Answer { get; set; }

        [Required]
        public long MetaDataId { get; set; }
        public MetaData MetaData { get; set; }
        
        
    }
}
