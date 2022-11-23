﻿using System;
using System.Runtime.Serialization;
using ProtoBuf;

namespace Invite.Apollo.App.Graph.Common.Models.Course
{
    [DataContract]
    public class LoanOption : BaseItem

    {
        [DataMember(Order = 5)]
        public string Title { get; set; } = string.Empty;

        [DataMember(Order = 6)]
        public string Description { get; set; } = string.Empty;

        [DataMember(Order = 7)]
        public string Conditions { get; set; } = string.Empty;

        [DataMember(Order = 8)]
        public Uri LoanOptionsUrl { get; set; } = null!;

        [DataMember(Order = 9)]
        public Uri LoanOptionsImage { get; set; } = null!;

        [DataMember(Order = 10)]
        public bool? IsAvailable { get; set; }
    }
}
