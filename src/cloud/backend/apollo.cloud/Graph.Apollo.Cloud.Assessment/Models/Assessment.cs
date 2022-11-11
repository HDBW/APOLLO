using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Intrinsics.X86;
using Invite.Apollo.App.Graph.Common.Models;
using Invite.Apollo.App.Graph.Common.Models.Assessment.Enums;
using Microsoft.EntityFrameworkCore;

namespace Invite.Apollo.App.Graph.Assessment.Models
{
    public class Assessment : IBackendEntity
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

        /// <summary>
        /// Client AssessmentType
        /// </summary>
        public AssessmentType AssessmentType { get; set; } = AssessmentType.Unknown;

        /// <summary>
        /// Backend ItemType
        /// </summary>
        public AssessmentItemType ItemType { get; set; } = AssessmentItemType.Unknown;

        /// <summary>
        /// The Description of the Assessment as Markdown or HTML
        /// </summary>
        public string ItemStem { get; set; } = string.Empty;

        /// <summary>
        /// The Title of the Assessment
        /// </summary>
        public string ItemTitle { get; set; } = string.Empty;

        /// <summary>
        /// The Disclaimer of the Assessment
        /// </summary>
        public string Disclaimer { get; set; } = string.Empty;

        /// <summary>
        /// Esco Associated Occupations
        /// </summary>
        public List<Uri> EscoOccupations { get; set; } = new();

        /// <summary>
        /// Esco Associated Skills
        /// </summary>
        public List<Uri> EscoSkills { get; set; } = new();

        /// <summary>
        /// Kldb Associated Id
        /// </summary>
        [RegularExpression(@"(^\d{5}$)", ErrorMessage = "Must be a Kldb 5 digit classifier")]
        public string Kldb { get; set; } = string.Empty;

        /// <summary>
        /// Associated Profession
        /// </summary>
        public string Profession { get; set; } = string.Empty;

        /// <summary>
        /// Publisher as String
        /// </summary>
        public string Publisher { get; set; } = string.Empty;

        /// <summary>
        /// Timespan required to complete the Assessment as TimeSpan from Minutes
        /// </summary>
        public TimeSpan Duration { get; set; } = TimeSpan.Zero;

        
        public List<AssessmentQuestion> AssessmentQuestions { get; set; }
    }
}
