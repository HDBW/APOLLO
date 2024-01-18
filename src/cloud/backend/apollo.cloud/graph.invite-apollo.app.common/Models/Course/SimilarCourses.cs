// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace Invite.Apollo.App.Graph.Common.Models.Course
{
    [DataContract]
    public class SimilarCourses : BaseItem
    {
        
        #region Relations

        [ForeignKey(nameof(CourseItem))]
        [DataMember(Order = 5, IsRequired = false)]
        public long CourseOriginId { get; set; }

        [ForeignKey(nameof(CourseItem))]
        [DataMember(Order = 6, IsRequired = false)]
        public long CourseSimilarId { get; set; }


        #endregion

        //TODO: Implement Reason in the future as MetaDataRelation

    }
}
