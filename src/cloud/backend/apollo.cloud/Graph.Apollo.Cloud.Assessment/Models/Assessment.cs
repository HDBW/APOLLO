using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Invite.Apollo.App.Graph.Common.Models;
using Invite.Apollo.App.Graph.Common.Models.Assessment.Enums;
using Microsoft.ApplicationInsights.DataContracts;

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
        [Index(IsUnique = true)]
        [Required]
        [MaxLength(62)]
        public Uri Schema { get; set; }

        #endregion

        [Index(IsUnique = true)]
        [Required]
        public string ExternalId { get; set; }

        /// <summary>
        /// Client AssessmentType
        /// </summary>
        public AssessmentType AssessmentType { get; set; }

        /// <summary>
        /// Backend ItemType
        /// </summary>
        public ItemType ItemType { get; set; }
    }

    public enum ItemType
    {
        Unknown = 0,
        Sort = 1,
        Choice = 2,
        Associate = 3,
        Imagemap = 4,
        EaFrequency = 5,
        Rating = 6
    }
}
