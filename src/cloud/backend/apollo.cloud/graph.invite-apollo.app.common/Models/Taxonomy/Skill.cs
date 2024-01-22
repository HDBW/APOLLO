// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Runtime.Serialization;

namespace Invite.Apollo.App.Graph.Common.Models.Esco
{

    [DataContract]
    public class Skill : BaseItem
    {
        public string? Id { get; set; }

        public string Value { get; set; }
    }
}
