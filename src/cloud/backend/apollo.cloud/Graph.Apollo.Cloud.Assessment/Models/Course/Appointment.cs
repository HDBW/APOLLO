using System.Globalization;
using System.Runtime.Serialization;
using Invite.Apollo.App.Graph.Common.Models.ContentManagement;
using Invite.Apollo.App.Graph.Common.Models.Course;
using Invite.Apollo.App.Graph.Common.Models.Course.Enums;

namespace Invite.Apollo.App.Graph.Assessment.Models.Course
{
    public class Appointment : BaseItem, IPublishingInfo
    {

        public long CourseId { get; set; }

        public string BookingCode { get; set; } = string.Empty;

        public string Summary { get; set; } = string.Empty;

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        //FIXME: Timespan is not considering Recurrence rule, basically time spent. 
        [IgnoreDataMember]
        public TimeSpan? Duration
        {
            get { return StartDate != null && EndDate != null ? EndDate - StartDate : null; }
        }

        /// <summary>
        /// Recurrence Rule as in Google Calendar
        /// RRULE:FREQ=WEEKLY;UNTIL=20110617T065959Z
        /// </summary>
        public string Recurrence { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public bool? IsBookable { get; set; }
        public bool? IsCancelled { get; set; }
        public CourseType Type { get; set; }
        public AppointmentType AppointmentType { get; set; }
        public OccurrenceType OccurrenceType { get; set; }
        public long BookingContact { get; set; }
        public Uri BookingUrl { get; set; } = null!;
        public CultureInfo Language { get; set; } = null!;
        public int AvailableSeats { get; set; }

        #region Implementation of IPublishingInfo
        public DateTime? PublishingDate { get; set; }
        public DateTime? LatestUpdate { get; set; }
        public DateTime? Deprecation { get; set; }
        public string? DeprecationReason { get; set; }
        public DateTime? UnPublishingDate { get; set; }
        public DateTime? Deleted { get; set; }
        //[ForeignKey(nameof(CourseAppointment))]
        public long? SuccessorId { get; set; }
        //[ForeignKey(nameof(CourseAppointment))]
        public long? PredecessorId { get; set; }

        #endregion

        /// <summary>
        /// FIXME: CourseOffer has price relation and more detail for after december scope
        /// </summary>
        [Obsolete]
        public decimal Price { get; set; }

        /// <summary>
        /// FIXME: CourseOffer has price relation more detail for after december scope
        /// </summary>
        [Obsolete]
        public string Currency { get; set; } = string.Empty;

        public Course Course { get; set; }
    }
}
