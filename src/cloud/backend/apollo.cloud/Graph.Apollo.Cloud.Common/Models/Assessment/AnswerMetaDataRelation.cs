
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace Graph.Apollo.Cloud.Common.Models.Assessment
{
    [DataContract]
    public class AnswerMetaDataRelation : IEntity
    {
        [DataMember(Order = 1)]
        [Key]
        public long Id { get; set; }

        [DataMember(Order = 2)]
        public long Ticks { get; set; }

        [DataMember(Order = 3)]
        [ForeignKey(nameof(AnswerItem))]
        public long AnswerId { get; set; }

        [ForeignKey(nameof(MetaData))]
        [DataMember(Order = 4)]
        public long MetaDataId { get; set; }
    }
}
