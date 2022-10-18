﻿using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Invite.Apollo.App.Graph.Common.Models.Course
{
    [DataContract]
    public class CourseContact : IEntity, IApolloGraphItem
    {
        #region Client and Backend Stuff

        [Key]
        [DataMember(Order = 1,IsRequired = true)]
        public long Id { get; set; }
        [DataMember(Order = 2,IsRequired = true)]
        public long Ticks { get; set; }
        [DataMember(Order = 3,IsRequired = true)]
        public string BackendId { get; set; } = null!;
        [DataMember(Order = 4)]
        public Uri Schema { get; set; } = null!;

        #endregion

        [DataMember(Order = 5)]
        public string ContactName { get; set; } = null!;

        [DataMember(Order = 6)]
        public string ContactMail { get; set; } = null!;

        [DataMember(Order = 7)]
        public string ContactPhone { get; set; } = null!;

        [DataMember(Order = 8)] public Uri Url { get; set; } = null!;

        [DataMember(Order = 9)]
        public Uri Image { get; set; } = null!;
    }
}
