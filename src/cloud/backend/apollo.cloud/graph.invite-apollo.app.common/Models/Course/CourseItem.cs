using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Runtime.Serialization;
using Invite.Apollo.App.Graph.Common.Models.ContentManagement;
using Invite.Apollo.App.Graph.Common.Models.Course.Enums;

namespace Invite.Apollo.App.Graph.Common.Models.Course
{
    [DataContract]
    public class CourseItem : IEntity, IApolloGraphItem, IPublishingInfo
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
        public string BackendId { get; set; } = null!;

        [DataMember(Order = 4)]
        public Uri Schema { get; set; } = null!;

        #endregion

        public string Title { get; set; } = null!;

        /// <summary>
        /// Short description aka Executive Summary
        /// A AI based feature which generates the key Talking points of the course
        /// </summary>
        public string ShortDescription { get; set; } = null!;

        /// <summary>
        /// Course Description as HTML
        /// </summary>
        public string Description { get; set; } = null!;

        /// <summary>
        /// 
        /// TODO: Maybe a list of PersonTypes as identified by the backend?
        /// </summary>
        public string TargetGroup { get; set; } = null!;


        public CourseType Type { get; set; }


        //TODO: Implement Calculation for availability
        public CourseAvailability Availability { get; set; }

        public DateTime LatestUpdateFromProvider { get; set; }

        
        public string PreRequisitesDescription { get; set; } = null!;

        //TODO: Define Scope of LearningOutcomes note backend only!

        //TODO: Define Scope of Skills and EscoSkills

        //TODO: Define Scope of Occupations

        //TODO: Define Scope of Qualifications

        /// <summary>
        /// This is a draft for Key phrases
        /// Key phrases are BiTerm Analysis of the given text
        /// </summary>
        public string KeyPhrases { get; set; } = null!;

        /// <summary>
        /// Duration of the Course
        /// </summary>
        public TimeSpan Duration { get; set; }


        #region Classification

        /// <summary>
        /// Indicates the available Occurrences
        /// displays if appointments are available: PartTime, Fulltime, Both
        /// </summary>
        public OccurrenceType Occurrence { get; set; }

        /// <summary>
        /// Course Language the description of the course is in language
        /// </summary>
        public CultureInfo Language { get; set; } = null!;

        /// <summary>
        /// Indicates the type of the course
        /// Online, InPerson, ...
        /// </summary>
        public CourseType CourseType { get; set; }



        #endregion

        // FIXME: Review Price
        [Obsolete]
        public decimal? Price { get; set; }
        [Obsolete]
        public string Currency { get; set; } = null!;


        #region relations

        /// <summary>
        /// Indicates the Instructor on the Client
        /// </summary>
        [ForeignKey(nameof(CourseContact))]
        public long InstructorId { get; set; }

        [ForeignKey(nameof(EduProvider))]
        public long TrainingProviderId { get; set; }

        [ForeignKey(nameof(EduProvider))]
        public long CourseProviderId { get; set; }

        public string TrainingProviderIdBackend { get; set; } = null!;
        public string CourseProviderIdBackend { get; set; } = null!;

        //TODO: QualificationProvider

        [ForeignKey(nameof(CourseItem))]
        public long? CourseParentId { get; set; }

        public string? CourseParentBackendId { get; set; }

        #endregion

        #region Publishing Stuff

        public DateTime? PublishingDate { get; set; }
        public DateTime? LatestUpdate { get; set; }
        public DateTime? Deprecation { get; set; }
        public string? DeprecationReason { get; set; }
        public DateTime? UnPublishingDate { get; set; }
        public DateTime? Deleted { get; set; }

        [ForeignKey(nameof(CourseItem))]
        public long? Successor { get; set; }

        [ForeignKey(nameof(CourseItem))]
        public long? Predecessor { get; set; }

        public string? SuccessorBackend { get; set; }
        public string? PredecessorBackend { get; set; }

        #endregion
    }

    [DataContract]
    public class CourseRequest : ICorrelationId
    {
        [DataMember(Order = 1, IsRequired = true)]
        public string CorrelationId { get; set; } = null!;
    }

    [DataContract]
    public class CourseResponse : ICorrelationId
    {
        public CourseResponse()
        {
            Courses = new Collection<CourseItem>();
            CourseModules = new Collection<CourseModules>();
        }

        [DataMember(Order = 1,IsRequired = true)]
        public string CorrelationId { get; set; } = null!;


        public Collection<CourseItem> Courses { get; set; }

        public Collection<CourseModules> CourseModules { get; set; }
    }

    [DataContract]
    public class CourseSearchRequest : ICorrelationId
    {
        [DataMember(Order = 1,IsRequired = true)]
        public string CorrelationId { get; set; } = null!;
    }

    [DataContract]
    public class CourseSearchResponse : ICorrelationId
    {
        [DataMember(Order = 1, IsRequired = true)]
        public string CorrelationId { get; set; } = null!;
    }
}
