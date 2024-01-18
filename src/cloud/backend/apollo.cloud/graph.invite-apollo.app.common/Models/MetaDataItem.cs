// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Runtime.Serialization;
using Invite.Apollo.App.Graph.Common.Models.Assessment.Enums;


namespace Invite.Apollo.App.Graph.Common.Models
{
    [DataContract]
    public class MetaDataItem : BaseItem
    {
        [DataMember(Order = 5)]
        public MetaDataType Type { get; set; }

        [DataMember(Order = 6)]
        public string Value { get; set; } = string.Empty;
    }
}
