using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Invite.Apollo.App.Graph.Common.Models;

namespace Invite.Apollo.App.Graph.Assessment.Models
{
    public class Asset : IBackendEntity
    {
        #region Implementation of IBackendEntity

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

        public string ExternalId { get; set; }

        public string assetName { get; set; }

        public List<Uri> file { get; set; }

        public List<Uri> Blob { get; set; }

        public List<Uri> Cdn { get; set; }
    }
}
