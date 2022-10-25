
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace Invite.Apollo.App.Graph.Common.Models.Course
{
    /// <summary>
    /// Describing the Learning Objectives of a Course
    /// </summary>
    [DataContract]
    public class CourseLearningObjectives : IEntity, IBackendEntity
    {
        #region Client Implementation

        [Key]
        [DataMember(Order = 1,IsRequired = true)]
        public long Id { get; set; }

        [DataMember(Order = 2)]
        public long Ticks { get; set; }

        #endregion

        #region Backend Implementation

        [DataMember(Order = 3,IsRequired = true)]
        public long BackendId { get; set; }

        [DataMember(Order = 4)]
        public Uri Schema { get; set; } = null!;

        #endregion

        /// <summary>
        /// Reference to Course
        /// </summary>
        [ForeignKey(nameof(CourseItem))]
        [DataMember(Order = 5, IsRequired = true)]
        public long CourseId { get; set; }

        [DataMember(Order = 6, IsRequired = true)]
        public string CourseIdBackend { get; set; } = string.Empty;

        /// <summary>
        /// Probably Concept -> Move to Metadata for Description for Production
        /// </summary>
        [DataMember(Order = 7)]
        public string Value { get; set; } = string.Empty;

        //TODO: Implement Mapping to Taxonomie
    }
}
