﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace Invite.Apollo.App.Graph.Common.Models.Course
{
    [DataContract]
    public class CourseModules : IEntity, IApolloGraphItem
    {
        #region client stuff
        [Key]
        [DataMember(Order = 1)]
        public long Id { get; set; }

        [DataMember(Order = 2)]
        public long Ticks { get; set; }

        #endregion

        #region cloud stuff

        [DataMember(Order = 3, IsRequired = true)]
        public string BackendId { get; set; } = null!;

        [DataMember(Order = 4)]
        public Uri Schema { get; set; } = null!;

        #endregion

        [DataMember(Order=5,IsRequired = true)]
        [ForeignKey(nameof(CourseItem))]
        public long CourseId { get; set; }


        [DataMember(Order = 7,IsRequired = true)]
        [ForeignKey(nameof(CourseItem))]
        public long ModuleId { get; set; }

    }
}
