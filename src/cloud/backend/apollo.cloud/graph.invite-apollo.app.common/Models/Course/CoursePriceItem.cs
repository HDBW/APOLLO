﻿using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using ProtoBuf;

namespace Invite.Apollo.App.Graph.Common.Models.Course
{
    [DataContract]
    public class CoursePriceItem : BaseItem
    {
        
        [DataMember(Order = 7)]
        public decimal Price { get; set; }

        [DataMember(Order = 8)]
        public string Currency { get; set; } = string.Empty;

        [DataMember(Order = 9)]
        public DateTime? StartTime { get; set; }

        [DataMember(Order = 10)]
        public DateTime? EndTime { get; set; }

        [DataMember(Order = 11)]
        public string Description { get; set; } = string.Empty;

        [DataMember(Order = 12)]
        public string Conditions { get; set; } = string.Empty;
    }
}
