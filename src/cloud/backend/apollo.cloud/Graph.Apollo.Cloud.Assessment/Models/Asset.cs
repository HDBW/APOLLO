using System;
using System.ComponentModel.DataAnnotations;
using Invite.Apollo.App.Graph.Common.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

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
        //[System.ComponentModel.DataAnnotations.Schema.Index(IsUnique = true)]
        [Required]
        [MaxLength(62)]
        public Uri Schema { get; set; } = new Uri($"https://invite-apollo.app/{Guid.NewGuid()}");

        //public long MetaDataId { get; set; }
        
        #endregion

        [Required]
        [MaxLength(64)]
        //[System.ComponentModel.DataAnnotations.Schema.Index(IsUnique = true)]
        public string ExternalId { get; set; }

        public string assetName { get; set; }

        public List<Uri> FilesUris { get; set; } = new();

        public List<Uri> BlobUris { get; set; } = new();
        public List<Uri> CdnUris { get; set; } = new();
        public List<MetaData> MetaDatas { get; set; }
    }

    public class AssetConfiguration : IEntityTypeConfiguration<Asset>
    {
        public void Configure(EntityTypeBuilder<Asset> builder)
        {
            //TODO: Review by talisi
            // This Converter will perform the conversion to and from Json to the desired type
            builder.Property(e => e.FilesUris).HasJsonConversion<List<Uri>>();
            builder.Property(e => e.BlobUris).HasJsonConversion<List<Uri>>();
            builder.Property(e => e.CdnUris).HasJsonConversion<List<Uri>>();
        }
    }
}
