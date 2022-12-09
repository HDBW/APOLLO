using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Invite.Apollo.App.Graph.Assessment.Models
{
    public class MetaDataHasMetaData: BaseItem
    {
        [Required]
        public long SourceMetaDataId { get; set; }
        [DeleteBehavior(DeleteBehavior.NoAction)]
        [ForeignKey("SourceMetaDataId")]
        public MetaData SourceMetaData{ get; set; }

        [Required]
        public long TargetMetaDataId { get; set; }
        [DeleteBehavior(DeleteBehavior.NoAction)]
        [ForeignKey("TargetMetaDataId")]
        public MetaData TargetMetaData { get; set; }

    }
}
