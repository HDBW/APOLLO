using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Invite.Apollo.App.Graph.Common.Models;
using Invite.Apollo.App.Graph.Common.Models.Assessment.Enums;

namespace Invite.Apollo.App.Graph.Assessment.Models
{
    public class Answer : BaseItem
    {
        public long QuestionId { get; set; }

        [Required]
        public Question Question { get; set; }

        public AnswerType AnswerType { get; set; }

        public string Value { get; set; } = string.Empty;

        public List<AnswerHasMetaData> AnswerHasMetaDatas { get; set; }

    }
}
