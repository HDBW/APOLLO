﻿using System.Runtime.Serialization;
using ProtoBuf;

namespace Invite.Apollo.App.Graph.Common.Models
{
    public class ApolloLabel : BaseItem
    {

        [DataMember(Order = 5, IsRequired = true)]
        public string Value { get; set; } = null!;

        [DataMember(Order = 6, IsRequired = true)]
        public string ColorCode { get; set; } = null!; 
    }
}
