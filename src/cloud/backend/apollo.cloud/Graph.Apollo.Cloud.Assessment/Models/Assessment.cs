using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Intrinsics.X86;
using System.Runtime.Serialization;
using Invite.Apollo.App.Graph.Common.Models;
using Invite.Apollo.App.Graph.Common.Models.Assessment.Enums;
using Microsoft.ApplicationInsights.DataContracts;

namespace Invite.Apollo.App.Graph.Assessment.Models
{
    public class Assessment : BaseItem
    {
        
        /// <summary>
        /// Import Id from External Dataprovider.
        /// Not mapped/ serialized for client
        /// </summary>
        [Required]
        public string ExternalId { get; set; } = string.Empty;

        /// <summary>
        /// Assessment Title
        /// </summary>
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// Associated Profession or Empty string
        /// </summary>
        public string Profession { get; set; } = string.Empty;

        /// <summary>
        /// If the Assessment is used for a specific profession
        /// Kldb Associated Occupations - 
        /// </summary>
        [RegularExpression(@"(^\d{5}$)", ErrorMessage = "Must be a Kldb 5 digit classifier")]
        public string Kldb { get; set; } = string.Empty;



        /// <summary>
        /// If the Assessment is used for a specific profession -
        /// Esco Associated Occupations
        /// </summary>
        public string EscoOccupationId { get; set; } = string.Empty;

        /// <summary>
        /// Type of the Assessment
        /// </summary>
        public AssessmentType AssessmentType { get; set; }

        //TODO: Should be Publisher reference 
        /// <summary>
        /// Assessment Publisher
        /// </summary>
        public string Publisher { get; set; }

        // TODO: Will be auto calculated in the future
        /// <summary>
        /// Expected Minutes (Duration) to take the Assessment
        /// </summary>
        public TimeSpan Duration { get; set; }

        /// <summary>
        /// The Description of the Assessment as Markdown or HTML
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// The Disclaimer of the Assessment
        /// </summary>
        public string Disclaimer { get; set; }

        /// <summary>
        /// Esco Associated Skills
        /// </summary>
        public List<EscoSkill> EscoSkills { get; set; } = new();

        
        public List<Question> Questions { get; set; }
    }
}
