// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace Graph.Apollo.Cloud.Common.Models.Assessment
{
    [DataContract]
    public class MetaDataMetaDataRelation : IEntity
    {
        [DataMember(Order = 1, IsRequired = true)]
        [Key]
        public long Id { get; set; }

        [DataMember(Order = 2, IsRequired = true)]
        public long Ticks { get; set; }

        [DataMember(Order = 3)]
        [ForeignKey(nameof(MetaData))]
        public long SourceId { get; set; }

        [DataMember(Order = 3)]
        [ForeignKey(nameof(MetaData))]
        public long TargetId { get; set; }
    }
}
