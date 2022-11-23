using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.Serialization;
using Invite.Apollo.App.Graph.Common.Models.ContentManagement;
using Invite.Apollo.App.Graph.Common.Models.Course.Enums;
using ProtoBuf;

namespace Invite.Apollo.App.Graph.Common.Models.Course
{
    [DataContract]
    public class CourseAppointment : BaseItem, IPublishingInfo
    {

        [DataMember(Order = 5)]
        [ForeignKey(nameof(CourseItem))]
        public long CourseId { get; set; }

        [DataMember(Order = 7)]
        public string BookingCode { get; set; } = string.Empty;

        [DataMember(Order = 8)]
        public string Summary { get; set; } = string.Empty;

        [DataMember(Order = 9)]
        public DateTime? StartDate { get; set; }

        [DataMember(Order = 10)]
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
        [DataMember(Order = 11)]
        public string Recurrence { get; set; } = string.Empty;

        [DataMember(Order = 12)]
        public string Location { get; set; } = string.Empty;

        [DataMember(Order = 13)]
        public bool? IsBookable { get; set; }

        [DataMember(Order = 14)]
        public bool? IsCancelled { get; set; }

        [DataMember(Order = 15)]
        public CourseType Type { get; set; }

        [DataMember(Order = 16)]
        public AppointmentType AppointmentType { get; set; }

        [DataMember(Order = 17)]
        public OccurrenceType OccurrenceType { get; set; }

        [DataMember(Order = 18)]
        [ForeignKey(nameof(BookingContact))]
        public long BookingContact { get; set; }

        [DataMember(Order = 19)]
        public Uri? BookingUrl { get; set; } = null!;

        [DataMember(Order = 20)]
        public string Language { get; set; } = null!;

        [DataMember(Order = 21)]
        public int? AvailableSeats { get; set; }

        #region Implementation of IPublishingInfo
        [DataMember(Order = 22,IsRequired = false)]
        public DateTime? PublishingDate { get; set; }

        [DataMember(Order = 23, IsRequired = false)]
        public DateTime? LatestUpdate { get; set; }

        [DataMember(Order = 24, IsRequired = false)]
        public DateTime? Deprecation { get; set; }

        [DataMember(Order = 25, IsRequired = false)]
        public string? DeprecationReason { get; set; }

        [DataMember(Order = 26, IsRequired = false)]
        public DateTime? UnPublishingDate { get; set; }

        [DataMember(Order = 27, IsRequired = false)]
        public DateTime? Deleted { get; set; }

        [ForeignKey(nameof(CourseAppointment))]
        [DataMember(Order = 28, IsRequired = false)]
        public long? SuccessorId { get; set; }

        [ForeignKey(nameof(CourseAppointment))]
        [DataMember(Order = 29, IsRequired = false)]
        public long? PredecessorId { get; set; }

        #endregion

        /// <summary>
        /// FIXME: CourseOffer has price relation and more detail for after december scope
        /// </summary>
        [Obsolete]
        [DataMember(Order = 32, IsRequired = false)]
        public decimal Price { get; set; }

        /// <summary>
        /// FIXME: CourseOffer has price relation more detail for after december scope
        /// </summary>
        [Obsolete]
        [DataMember(Order = 33, IsRequired = false)]
        public string Currency { get; set; } = string.Empty;
    }
}
