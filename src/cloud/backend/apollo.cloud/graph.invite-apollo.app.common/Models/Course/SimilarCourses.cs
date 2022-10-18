using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace Invite.Apollo.App.Graph.Common.Models.Course
{
    [DataContract]
    public class SimilarCourses : IEntity, IBackendEntity
    {
        #region Implementation of IEntity
        [Key]
        [DataMember(Order = 1)]
        public long Id { get; set; }

        [DataMember(Order = 2, IsRequired = true)]
        public long Ticks { get; set; }

        #endregion

        #region Implementation of IBackendEntity
        [DataMember(Order = 3, IsRequired = true)]
        public long BackendId { get; set; }

        [DataMember(Order = 4, IsRequired = true)]
        public Uri Schema { get; set; } = null!;

        #endregion

        #region Relations

        [ForeignKey(nameof(CourseItem))]
        [DataMember(Order = 5, IsRequired = false)]
        public long CourseOriginId { get; set; }

        [ForeignKey(nameof(CourseItem))]
        [DataMember(Order = 6, IsRequired = false)]
        public long CourseSimilarId { get; set; }

        [DataMember(Order = 7, IsRequired = false)]
        public long CourseOriginBackendId { get; set; }

        [DataMember(Order = 8, IsRequired = false)]
        public long CourseSimilarBackendId { get; set; }

        #endregion

        //TODO: Implement Reason in the future as MetaDataRelation

    }
}
