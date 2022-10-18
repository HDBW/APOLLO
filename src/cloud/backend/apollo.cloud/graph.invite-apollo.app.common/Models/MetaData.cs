using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Invite.Apollo.App.Graph.Common.Models.Assessment.Enums;

namespace Invite.Apollo.App.Graph.Common.Models
{
    [DataContract]
    public class MetaData : IEntity
    {
        [DataMember(Order = 1)]
        [Key]
        public long Id { get; set; }

        [DataMember(Order = 2, IsRequired = true)]
        public long Ticks { get; set; }

        [DataMember(Order = 3)]
        public MetaDataType Type { get; set; }

        [DataMember(Order = 4)]
        public string? Value { get; set; }
    }
}
