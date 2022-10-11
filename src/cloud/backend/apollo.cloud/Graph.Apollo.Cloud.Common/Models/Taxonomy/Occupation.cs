// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Runtime.Serialization;

namespace Graph.Apollo.Cloud.Common.Models.Taxonomy
{
    [DataContract]
    public class Occupation
    {
        [DataMember(Order = 1)]
        public long Id { get; set; }

        [DataMember(Order = 2)]
        public string Name { get; set; }
    }
}
