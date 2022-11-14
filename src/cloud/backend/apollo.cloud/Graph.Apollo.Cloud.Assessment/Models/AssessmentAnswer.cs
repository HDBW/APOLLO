using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Invite.Apollo.App.Graph.Common.Models;

namespace Invite.Apollo.App.Graph.Assessment.Models
{
    public class AssessmentAnswer : IBackendEntity
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
        //[Index(IsUnique = true)]
        [Required]
        [MaxLength(62)]
        public Uri Schema { get; set; } = new Uri($"https://invite-apollo.app/{Guid.NewGuid()}");

        public long AssessmentQuestionId { get; set; }

        #endregion

        [Required]
        public AssessmentQuestion AssessmentQuestion { get; set; }

        public string Value { get; set; } = string.Empty;

        public List<AssessmentAnswerHasMetaData> AssessmentAnswerHasMetaDatas { get; set; }
    }
}
