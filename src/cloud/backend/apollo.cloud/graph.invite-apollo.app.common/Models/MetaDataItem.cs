using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Invite.Apollo.App.Graph.Common.Models.Assessment.Enums;

namespace Invite.Apollo.App.Graph.Common.Models
{
    [DataContract]
    public class MetaDataItem : BaseItem, IEntity, IBackendEntity
    {
        [DataMember(Order = 5)]
        public MetaDataType Type { get; set; }

        [DataMember(Order = 6)]
        public string? Value { get; set; } = string.Empty;
    }
}
