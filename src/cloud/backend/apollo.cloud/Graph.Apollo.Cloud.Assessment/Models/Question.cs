using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Invite.Apollo.App.Graph.Common.Models;
using Invite.Apollo.App.Graph.Common.Models.Assessment.Enums;

namespace Invite.Apollo.App.Graph.Assessment.Models
{
    public class Question : IBackendEntity
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
        [Index(IsUnique = true)]
        [Required]
        [MaxLength(62)]
        public Uri Schema { get; set; } = new Uri($"https://invite-apollo.app/{Guid.NewGuid()}");

        #endregion

        [Required]
        public string ExternalId { get; set; } = string.Empty;

        [Required]
        public Assessment Assessment { get; set; }

        [Required]
        public AssessmentCategory QuestionCategory { get; set; }

        [Required]
        public QuestionType QuestionType { get; set; }

        [Required]
        public LayoutType QuestionLayout { get; set; }

        [Required]
        public LayoutType AnswerLayout { get; set; }

        [Required]
        public LayoutType InteractionType { get; set; }

        public List<MetaData> MetaDatas { get; set; }

        public List<Scores> ScoringOptions { get; set; }
    }
}
