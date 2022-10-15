using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace Invite.Apollo.App.Graph.Common.Models.Course
{
    [DataContract]
    public class CourseItem : IEntity, IApolloGraphItem
    {
        #region client stuff
        [Key]
        [DataMember(Order = 1)]
        public long Id { get; set; }

        [DataMember(Order = 2)]
        public long Ticks { get; set; }

        #endregion

        #region cloud stuff

        [DataMember(Order = 3,IsRequired = true)]
        public string BackendId { get; set; }

        [DataMember(Order = 4)]
        public Uri Schema { get; set; }

        #endregion

        public CourseType Type { get; set; }

        #region relations

        [ForeignKey(nameof(EduProvider))]
        public long TrainingProviderId { get; set; }

        [ForeignKey(nameof(EduProvider))]
        public long CourseProviderId { get; set; }

        public string TrainingProviderIdBackend { get; set; }
        public string CourseProviderIdBackend { get; set; }

        public CourseAvailability Availability { get; set; }

        //TODO: QualificationProvider

        [ForeignKey(nameof(CourseItem))]
        public long? CoursePredecessor { get; set; }

        public string? CoursePredecessorBackendId { get; set; }

        [ForeignKey(nameof(CourseItem))]
        public long? Successor { get; set; }

        public string? CourseSuccessorId { get; set; }
        
        [ForeignKey(nameof(CourseItem))]
        public long? CourseParent { get; set; }

        public string? CourseParentId { get; set; }

        #endregion
    }

    [DataContract]
    public class CourseRequest : ICorrelationId
    {
        [DataMember(Order = 1, IsRequired = true)]
        public string CorrelationId { get; set; }
    }

    [DataContract]
    public class CourseResponse : ICorrelationId
    {
        [DataMember(Order = 1,IsRequired = true)]
        public string CorrelationId { get; set; }


        public Collection<CourseItem> Courses { get; set; }

        public Collection<CourseModules> CourseModules { get; set; }
    }

    [DataContract]
    public class CourseSearchRequest : ICorrelationId
    {
        [DataMember(Order = 1,IsRequired = true)]
        public string CorrelationId { get; set; }
    }

    [DataContract]
    public class CourseSearchResponse : ICorrelationId
    {
        [DataMember(Order = 1, IsRequired = true)]
        public string CorrelationId { get; set; }
    }
}
