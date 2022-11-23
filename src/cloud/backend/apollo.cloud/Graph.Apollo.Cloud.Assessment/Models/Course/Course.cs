using System.Globalization;
using System.Runtime.Serialization;
using Invite.Apollo.App.Graph.Common.Models.ContentManagement;
using Invite.Apollo.App.Graph.Common.Models.Course;
using Invite.Apollo.App.Graph.Common.Models.Course.Enums;

namespace Invite.Apollo.App.Graph.Assessment.Models.Course
{
    //[ProtoReserved(18,19,"Price")] //FIXME: https://developers.google.com/protocol-buffers/docs/proto3#reserved
    public class Course : BaseItem, IPublishingInfo
    {
        public string ExternalId { get; set; }

        /// <summary>
        /// The title of a course provided by the training provider
        /// </summary>
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// A short description of the course
        /// A AI based feature which generates the key Talking points of the course
        /// </summary>
        public string ShortDescription { get; set; } = string.Empty;

        /// <summary>
        /// Course Description as HTML
        /// A description of the course
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// A list of person types or occupations as well as text describing the target audience.
        /// TODO: Maybe a list of PersonTypes as identified by the backend?
        /// </summary>
        public string TargetGroup { get; set; } = string.Empty;

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
        public string PreRequisitesDescription { get; set; } = string.Empty;

        //TODO: Define Scope of LearningOutcomes note backend only!

        //TODO: Define Scope of Skills and EscoSkills

        //TODO: Define Scope of Occupations

        //TODO: Define Scope of Qualifications

        /// <summary>
        /// This is a draft for Key phrases
        /// Key phrases are BiTerm Analysis of the given text
        /// </summary>
        public string KeyPhrases { get; set; } = string.Empty;

        /// <summary>
        /// Duration of the Course
        /// </summary>
        public TimeSpan Duration { get; set; }

        public Uri CourseUrl { get; set; } = null;


        #region Classification

        /// <summary>
        /// Indicates the available Occurrences
        /// displays if appointments are available: part time, full time, both
        /// </summary>
        public OccurrenceType Occurrence { get; set; }

        /// <summary>
        /// Course Language the description of the course is in language
        /// TODO: CultureInfo
        public string Language { get; set; } = null!;

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
        public string Currency { get; set; } = string.Empty;


        #region relations

        /// <summary>
        /// Indicates the Instructor on the Client
        /// </summary>
        public long InstructorId { get; set; }
        
        /// <summary>
        /// The information about the training provider offering the course.
        /// </summary>
        public long TrainingProviderId { get; set; }

        
        /// <summary>
        /// The course provider offering the course via the training provider
        /// </summary>
        public long CourseProviderId { get; set; }

        
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
        public long? SuccessorId { get; set; }

        /// <summary>
        /// The Course replaced by the current course.
        /// </summary>
        public long? PredecessorId { get; set; }

        #endregion

        /// <summary>
        /// Indication for UI only not related to Tags!
        /// </summary>
        public CourseTagType CourseTagType { get; set; }

        public List<Appointment> Appointments { get; set; }
        public List<CourseHasContacts> CourseContacts { get; set; }
    }

}
