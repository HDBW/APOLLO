using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Runtime.Serialization;
using Invite.Apollo.App.Graph.Common.Models.ContentManagement;
using Invite.Apollo.App.Graph.Common.Models.Course.Enums;
using ProtoBuf;

namespace Invite.Apollo.App.Graph.Common.Models.Course
{
    [DataContract]
    //[ProtoReserved(18,19,"Price")] //FIXME: https://developers.google.com/protocol-buffers/docs/proto3#reserved
    public class CourseItem : IEntity, IBackendEntity, IPublishingInfo
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

        /// <summary>
        /// The title of a course provided by the training provider
        /// </summary>
        [DataMember(Order = 5,IsRequired = true)]
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// A short description of the course
        /// A AI based feature which generates the key Talking points of the course
        /// </summary>
        [DataMember(Order = 6, IsRequired = false)]
        public string ShortDescription { get; set; } = string.Empty;

        /// <summary>
        /// Course Description as HTML
        /// A description of the course
        /// </summary>
        [DataMember(Order = 7, IsRequired = true)]
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// A list of person types or occupations as well as text describing the target audience.
        /// TODO: Maybe a list of PersonTypes as identified by the backend?
        /// </summary>
        [DataMember(Order = 8, IsRequired = false)]
        public string TargetGroup { get; set; } = string.Empty;

        /// <summary>
        /// Indicates the type of a course: `Unknown, Online, InPerson, OnAndOffline, InHouse, All`
        /// </summary>
        [DataMember(Order = 9, IsRequired = true)]
        public CourseType Type { get; set; }

        /// <summary>
        ///  Indicates if a course is available: Unknown, Available, Unavailable
        /// </summary>
        //TODO: Implement Calculation for availability
        [DataMember(Order = 10, IsRequired = false)]
        public CourseAvailability Availability { get; set; }

        /// <summary>
        /// Indicates the latest update from the training provider.
        /// </summary>
        [DataMember(Order = 11, IsRequired = false)]
        public DateTime LatestUpdateFromProvider { get; set; }

        /// <summary>
        /// The original text provided as prerequisites as the training provider publishes the course.
        /// </summary>
        [DataMember(Order = 12, IsRequired = false)]
        public string PreRequisitesDescription { get; set; } = string.Empty;

        //TODO: Define Scope of LearningOutcomes note backend only!

        //TODO: Define Scope of Skills and EscoSkills

        //TODO: Define Scope of Occupations

        //TODO: Define Scope of Qualifications

        /// <summary>
        /// This is a draft for Key phrases
        /// Key phrases are BiTerm Analysis of the given text
        /// </summary>
        [DataMember(Order = 13, IsRequired = false)]
        public string KeyPhrases { get; set; } = string.Empty;

        /// <summary>
        /// Duration of the Course
        /// </summary>
        [DataMember(Order = 14, IsRequired = false)]
        public TimeSpan Duration { get; set; }


        #region Classification

        /// <summary>
        /// Indicates the available Occurrences
        /// displays if appointments are available: part time, full time, both
        /// </summary>
        [DataMember(Order = 15, IsRequired = false)]
        public OccurrenceType Occurrence { get; set; }

        /// <summary>
        /// Course Language the description of the course is in language
        /// </summary>
        [DataMember(Order = 16, IsRequired = false)]
        public CultureInfo Language { get; set; } = null!;

        /// <summary>
        /// Indicates the type of the course
        /// Online, InPerson, ...
        /// </summary>
        [DataMember(Order = 17, IsRequired = false)]
        public CourseType CourseType { get; set; }

        #endregion

        
        // FIXME: Review Price
        [Obsolete]
        [DataMember(Order = 40, IsRequired = false)]
        public decimal? Price { get; set; }
        [Obsolete]
        [DataMember(Order = 41, IsRequired = false)]
        public string Currency { get; set; } = string.Empty;


        #region relations

        /// <summary>
        /// Indicates the Instructor on the Client
        /// </summary>
        [DataMember(Order = 20, IsRequired = false)]
        [ForeignKey(nameof(CourseContact))]
        public long InstructorId { get; set; }

        /// <summary>
        /// Indicates the Instructor on the Client
        /// </summary>
        [DataMember(Order = 21, IsRequired = false)]
        public long InstructorBackendId { get; set; }

        /// <summary>
        /// The information about the training provider offering the course.
        /// </summary>
        [DataMember(Order = 22, IsRequired = false)]
        [ForeignKey(nameof(EduProviderItem))]
        public long TrainingProviderId { get; set; }

        /// <summary>
        /// The information about the training provider offering the course.
        /// </summary>
        [DataMember(Order = 23, IsRequired = false)]
        public long TrainingProviderBackendId { get; set; }

        /// <summary>
        /// The course provider offering the course via the training provider
        /// </summary>
        [DataMember(Order = 24, IsRequired = false)]
        [ForeignKey(nameof(EduProviderItem))]
        public long CourseProviderId { get; set; }

        /// <summary>
        /// The course provider offering the course via the training provider
        /// </summary>
        [DataMember(Order = 25, IsRequired = false)]
        public long CourseProviderBackendId { get; set; }

        //TODO: QualificationProvider

        #endregion

        #region Publishing Stuff

        /// <summary>
        /// Content Management | The date the course was or will be published. 
        /// </summary>
        [DataMember(Order = 30, IsRequired = false)]
        public DateTime? PublishingDate { get; set; }

        /// <summary>
        ///  The date the course was last updated.
        /// Note: Updates may happen to courses even though the training provider did not update the course
        /// </summary>
        [DataMember(Order = 31, IsRequired = false)]
        public DateTime? LatestUpdate { get; set; }

        /// <summary>
        /// The date of the course deprecation.
        /// </summary>
        [DataMember(Order = 32, IsRequired = false)]
        public DateTime? Deprecation { get; set; }

        /// <summary>
        ///  The reason for the course deprecation.
        /// </summary>
        [DataMember(Order = 33, IsRequired = false)]
        public string? DeprecationReason { get; set; }

        /// <summary>
        /// The date the course was or will be unpublished.
        /// </summary>
        [DataMember(Order = 34, IsRequired = false)]
        public DateTime? UnPublishingDate { get; set; }

        /// <summary>
        /// The date the course was deleted! ^_^
        /// </summary>
        [DataMember(Order = 35, IsRequired = false)]
        public DateTime? Deleted { get; set; }

        /// <summary>
        /// The Course replacing the current course.
        /// </summary>
        [ForeignKey(nameof(CourseItem))]
        [DataMember(Order = 36, IsRequired = false)]
        public long? SuccessorId { get; set; }

        [DataMember(Order = 37, IsRequired = false)]
        public long? SuccessorBackendId { get; set; }

        /// <summary>
        /// The Course replaced by the current course.
        /// </summary>
        [ForeignKey(nameof(CourseItem))]
        [DataMember(Order = 38, IsRequired = false)]
        public long? PredecessorId { get; set; }

        [DataMember(Order = 39, IsRequired = false)]
        public long? PredecessorBackendId { get; set; }

        #endregion
    }

    /// <summary>
    /// TODO: Implement Request
    /// </summary>
    [DataContract]
    public class CourseRequest : ICorrelationId
    {
        [DataMember(Order = 1, IsRequired = true)]
        public string CorrelationId { get; set; } = string.Empty;
    }

    /// <summary>
    /// TODO: Implement Response
    /// </summary>
    [DataContract]
    public class CourseResponse : ICorrelationId
    {
        public CourseResponse()
        {
            Courses = new Collection<CourseItem>();
            CourseModules = new Collection<CourseModuleItem>();
        }

        [DataMember(Order = 1,IsRequired = true)]
        public string CorrelationId { get; set; } = string.Empty;

        [DataMember(Order = 2)]
        public Collection<CourseItem> Courses { get; set; }

        [DataMember(Order = 3)]
        public Collection<CourseModuleItem> CourseModules { get; set; }
    }

    [DataContract]
    public class CourseSearchRequest : ICorrelationId
    {
        [DataMember(Order = 1,IsRequired = true)]
        public string CorrelationId { get; set; } = string.Empty;
    }

    [DataContract]
    public class CourseSearchResponse : ICorrelationId
    {
        [DataMember(Order = 1, IsRequired = true)]
        public string CorrelationId { get; set; } = string.Empty;
    }
}
