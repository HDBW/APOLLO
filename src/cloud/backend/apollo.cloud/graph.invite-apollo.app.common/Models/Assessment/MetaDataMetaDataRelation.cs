using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using ProtoBuf;

namespace Invite.Apollo.App.Graph.Common.Models.Assessment
{
    [DataContract]
    public class MetaDataMetaDataRelation : BaseItem
    {

        [DataMember(Order = 5)]
        [ForeignKey(nameof(MetaDataItem))]
        public long SourceId { get; set; }

        [DataMember(Order = 6)]
        [ForeignKey(nameof(MetaDataItem))]
        public long TargetId { get; set; }
    }
}
