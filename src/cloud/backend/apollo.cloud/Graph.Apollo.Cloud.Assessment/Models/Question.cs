using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;
using Invite.Apollo.App.Graph.Common.Models;
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

        ////Change after December       
        //[Required]
        //public LayoutType QuestionLayout { get; set; }
        ////Change after December       
        //[Required]
        //public LayoutType AnswerLayout { get; set; }
        ////Change after December       
        //[Required]
        //public LayoutType InteractionType { get; set; }

        public List<MetaData> MetaDatas { get; set; }

        public List<Scores> Scores { get; set; }
        
        public List<Answer> Answers { get; set; }

    }
}
