// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Graph.Apollo.Cloud.Common.Models.Assessment.Enums;

namespace Graph.Apollo.Cloud.Common.Models.Assessment
{
    [DataContract]
    public class MetaData : IEntity
    {
        [DataMember(Order = 1)]
        [Key]
        public long Id { get; set; }

        [DataMember(Order = 2, IsRequired = true)]
        public long Ticks { get; set; }

        [DataMember(Order = 2)]
        public MetaDataType Type { get; set; }

        [DataMember(Order=3)]
        public string Value { get; set; }
    }
}
