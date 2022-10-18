using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace Invite.Apollo.App.Graph.Common.Models.Course
{
    /// <summary>
    /// Defines a list of Benefits for a specific Course.
    /// </summary>
    [DataContract]
    public class CourseBenefits : IEntity, IApolloGraphItem

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


        [DataMember(Order = 5, IsRequired = true)]
        [ForeignKey(nameof(CourseItem))]
        public long CourseId { get; set; }

        [DataMember(Order = 6)]
        public string Value { get; set; } = null!;

        [DataMember(Order = 7)]
        public Uri Image { get; set; } = null!;

        //TODO: Implement Qualification

    }
}
