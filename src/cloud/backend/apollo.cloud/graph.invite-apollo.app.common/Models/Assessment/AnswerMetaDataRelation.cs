using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace Invite.Apollo.App.Graph.Common.Models.Assessment
{
    [DataContract]
    public class AnswerMetaDataRelation : BaseItem
    {

        [DataMember(Order = 5)]
        [ForeignKey(nameof(AnswerItem))]
        public long AnswerId { get; set; }

        [ForeignKey(nameof(MetaDataItem))]
        [DataMember(Order = 6)]
        public long MetaDataId { get; set; }



    }
}
