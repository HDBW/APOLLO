// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace Invite.Apollo.App.Graph.Common.Models.Course
{
    /// <summary>
    /// Describing the Learning Objectives of a Course
    /// </summary>
    [DataContract]
    public class CourseLearningObjectives : BaseItem
    {
        
        /// <summary>
        /// Reference to Course
        /// </summary>
        [ForeignKey(nameof(CourseItem))]
        [DataMember(Order = 5, IsRequired = true)]
        public long CourseId { get; set; }


        /// <summary>
        /// Probably Concept -> Move to Metadata for Description for Production
        /// </summary>
        [DataMember(Order = 7)]
        public string Value { get; set; } = string.Empty;

        //TODO: Implement Mapping to Taxonomie
    }
}
