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

        /// <summary>
        /// The unique identifier of the appointment.
        /// </summary>
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

        /// <summary>
        /// The title of a course provided by the training provider
        /// </summary>
        public string Title { get; set; } = null!;

        /// <summary>
        /// A short description of the course
        /// A AI based feature which generates the key Talking points of the course
        /// </summary>
        public string ShortDescription { get; set; } = null!;

        /// <summary>
        /// Course Description as HTML
        /// A description of the course
        /// </summary>
        public string Description { get; set; } = null!;

        /// <summary>
        /// A list of person types or occupations as well as text describing the target audience.
        /// TODO: Maybe a list of PersonTypes as identified by the backend?
        /// </summary>
        public string TargetGroup { get; set; } = null!;

        /// <summary>
        /// Indicates the type of a course: `Unknown, Online, InPerson, OnAndOffline, InHouse, All`
        /// </summary>
        public CourseType Type { get; set; }

        /// <summary>
        ///  Indicates if a course is available: Unknown, Available, Unavailable
        /// </summary>
        //TODO: Implement Calculation for availability
        public CourseAvailability Availability { get; set; }

        /// <summary>
        /// Indicates the latest update from the training provider.
        /// </summary>
        public DateTime LatestUpdateFromProvider { get; set; }

        /// <summary>
        /// The original text provided as prerequisites as the training provider publishes the course.
        /// </summary>
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
        /// displays if appointments are available: part time, full time, both
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

        /// <summary>
        /// The information about the training provider offering the course.
        /// </summary>
        [ForeignKey(nameof(EduProvider))]
        public long TrainingProviderId { get; set; }

        /// <summary>
        /// The course provider offering the course via the training provider
        /// </summary>
        [ForeignKey(nameof(EduProvider))]
        public long CourseProviderId { get; set; }

        public string TrainingProviderIdBackend { get; set; } = null!;
        public string CourseProviderIdBackend { get; set; } = null!;

        //TODO: QualificationProvider

        #endregion

        #region Publishing Stuff

        /// <summary>
        /// Content Management | The date the course was or will be published. 
        /// </summary>
        public DateTime? PublishingDate { get; set; }

        /// <summary>
        ///  The date the course was last updated.
        /// Note: Updates may happen to courses even though the training provider did not update the course
        /// </summary>
        public DateTime? LatestUpdate { get; set; }

        /// <summary>
        /// The date of the course deprecation.
        /// </summary>
        public DateTime? Deprecation { get; set; }

        /// <summary>
        ///  The reason for the course deprecation.
        /// </summary>
        public string? DeprecationReason { get; set; }

        /// <summary>
        /// The date the course was or will be unpublished.
        /// </summary>
        public DateTime? UnPublishingDate { get; set; }

        /// <summary>
        /// The date the course was deleted! ^_^
        /// </summary>
        public DateTime? Deleted { get; set; }

        /// <summary>
        /// The Course replacing the current course.
        /// </summary>
        [ForeignKey(nameof(CourseItem))]
        public long? Successor { get; set; }

        /// <summary>
        /// The Course replaced by the current course.
        /// </summary>
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
