using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace Invite.Apollo.App.Graph.Common.Models.Course
{
    [DataContract]
    public class SimilarCourses : IEntity, IApolloGraphItem
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

        #region Relations

        [ForeignKey(nameof(CourseItem))]
        [DataMember(Order = 5, IsRequired = true)]
        public long CourseOriginId { get; set; }

        [ForeignKey(nameof(CourseItem))]
        [DataMember(Order = 6, IsRequired = true)]
        public long CourseSimilarId { get; set; }

        #endregion

        //TODO: Implement Reason in the future as MetaDataRelation

    }
}
