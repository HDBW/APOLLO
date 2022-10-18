﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace Invite.Apollo.App.Graph.Common.Models.Course
{
    /// <summary>
    /// A course can be part of a other course.
    /// (In this case the actual course is a module of a course)
    /// Since a module can be part of more than one course this is a reference
    /// to a list of Unique Identifiers of the parent courses.
    /// </summary>
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
