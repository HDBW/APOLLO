using System.ComponentModel.DataAnnotations;
using Invite.Apollo.App.Graph.Common.Models.Assessment.Enums;

namespace Invite.Apollo.App.Graph.Assessment.Models
{
    public class Question : BaseItem
    {
        
        [Required]
        public string ExternalId { get; set; } = string.Empty;

        [Required]
        public long AssessmentId { get; set; }

        [Required]
        public Assessment Assessment { get; set; }

        [Required]
        public long CategoryId { get; set; }

        [Required]
        public Category Category { get; set; }

        [Required]
        public QuestionType QuestionType { get; set; }

        public string ScoringOption { get; set; }

        //public List<Scores> Scores { get; set; }
        
        public List<Answer> Answers { get; set; }

        public List<QuestionHasMetaData> QuestionHasMetaDatas { get; set; }
    }
}
