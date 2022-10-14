using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text;

namespace Invite.Apollo.App.Graph.Common.Models.Assessment
{
    [DataContract]
    public class MetaDataMetaDataRelation
    {
        [DataMember(Order = 1, IsRequired = true)]
        [Key]
        public long Id { get; set; }

        [DataMember(Order = 2, IsRequired = true)]
        public long Ticks { get; set; }

        [DataMember(Order = 3)]
        [ForeignKey(nameof(MetaData))]
        public long SourceId { get; set; }

        [DataMember(Order = 4)]
        [ForeignKey(nameof(MetaData))]
        public long TargetId { get; set; }
    }
}
