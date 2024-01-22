// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System;
using System.Runtime.Serialization;

namespace Invite.Apollo.App.Graph.Common.Models.Course
{
    [DataContract]
    public class EduProviderItem : BaseItem
    {
        
        [DataMember(Order = 5,IsRequired = true)]
        public string Name { get; set; } = string.Empty;

        [DataMember(Order = 6)]
        public string Description { get; set; } = string.Empty;

        [DataMember(Order = 7)]
        public Uri Website { get; set; } = null!;

        [DataMember(Order = 8)]
        public Uri Logo { get; set; } = null!;
    }
}
