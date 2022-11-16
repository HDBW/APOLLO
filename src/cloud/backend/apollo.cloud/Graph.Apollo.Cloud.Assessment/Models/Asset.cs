using System;
using System.ComponentModel.DataAnnotations;
using Invite.Apollo.App.Graph.Common.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Invite.Apollo.App.Graph.Assessment.Models
{
    //TODO: HealthCheck
    public class Asset : BaseItem
    {
        /// <summary>
        /// External Id from DataPrivider (Import).
        /// </summary>
        [Required]
        [MaxLength(64)]
        public string ExternalId { get; set; }

        public string assetName { get; set; }

        /// <summary>
        /// External Uri to DataPrivider (Import).
        /// </summary>
        public Uri FileUri { get; set; }

        public Uri BlobUris { get; set; }

        public Uri CdnUris { get; set; }

        public List<MetaData> MetaDatas { get; set; }
    }

    //public class AssetConfiguration : IEntityTypeConfiguration<Asset>
    //{
    //    public void Configure(EntityTypeBuilder<Asset> builder)
    //    {
    //        //TODO: Review by talisi
    //        // This Converter will perform the conversion to and from Json to the desired type
    //        builder.Property(e => e.FilesUris).HasJsonConversion<List<Uri>>();
    //        builder.Property(e => e.BlobUris).HasJsonConversion<List<Uri>>();
    //        builder.Property(e => e.CdnUris).HasJsonConversion<List<Uri>>();
    //    }
    //}
}
