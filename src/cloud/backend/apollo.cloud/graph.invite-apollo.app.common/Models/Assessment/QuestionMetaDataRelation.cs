using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System;

namespace Invite.Apollo.App.Graph.Common.Models.Assessment
{
    [DataContract]
    public class QuestionMetaDataRelation : BaseItem
    {

        [DataMember(Order = 5)]
        [ForeignKey(nameof(QuestionItem))]
        public long QuestionId { get; set; }

        [DataMember(Order = 6)]
        [ForeignKey(nameof(MetaDataItem))]
        public long MetaDataId { get; set; }
    }
}
