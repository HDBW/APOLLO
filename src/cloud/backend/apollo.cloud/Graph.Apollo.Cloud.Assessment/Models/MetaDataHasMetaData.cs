using System.ComponentModel.DataAnnotations;

namespace Invite.Apollo.App.Graph.Assessment.Models
{
    public class MetaDataHasMetaData: BaseItem
    {
        [Required]
        public long SourceMetaDataId { get; set; }
        public MetaData SourceMetaData{ get; set; }

        [Required]
        public long TargetMetaDataId { get; set; }
        public MetaData TargetMetaData { get; set; }

    }
}
