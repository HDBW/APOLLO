using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using ProtoBuf;

namespace Invite.Apollo.App.Graph.Common.Models.Taxonomy
{
    [DataContract]
    public class OccupationMetaDataRelation : BaseItem
    {

        [ForeignKey(nameof(Occupation))]
        public long OccupationId { get; set; }

        [ForeignKey(nameof(Models.MetaDataItem))]
        public long MetaDataItemId { get; set; }
    }
}
