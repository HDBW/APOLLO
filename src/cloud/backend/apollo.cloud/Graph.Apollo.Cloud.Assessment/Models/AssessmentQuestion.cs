using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Invite.Apollo.App.Graph.Common.Models;
using Invite.Apollo.App.Graph.Common.Models.Assessment.Enums;

namespace Invite.Apollo.App.Graph.Assessment.Models
{
    public class AssessmentQuestion : IBackendEntity
    {
        #region Implementation of IBackendEntity

        /// <summary>
        /// The Id of the Backend Unique Identifier
        /// </summary>
        [Key]
        public long BackendId { get; set; }

        /// <summary>
        /// Another Unique Identifier used as Uri for Services
        /// </summary>
        [Required]
        [MaxLength(62)]
        public Uri Schema { get; set; } = new Uri($"https://invite-apollo.app/{Guid.NewGuid()}");

        #endregion

        [Required]
        public string ExternalId { get; set; } = string.Empty;

        [Required]
        public AssessmentCategory QuestionCategory { get; set; }

        [Required]
        public AssessmentQuestionType AssessmentQuestionType { get; set; }

        [Required]
        public LayoutType QuestionLayout { get; set; }

        [Required]
        public LayoutType AnswerLayout { get; set; }

        [Required]
        public LayoutType InteractionType { get; set; }

        public List<AssessmentScores> ScoringOptions { get; set; }

        public List<AssessmentAnswer> AssessmentAnswers { get; set; }

        #region Relations

        [Required]
        public long AssessmentId { get; set; }

        [Required]
        public Assessment Assessment { get; set; }

        #endregion
    }
}
