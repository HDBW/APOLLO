using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using Invite.Apollo.App.Graph.Common.Models.Course.Enums;

namespace Invite.Apollo.App.Graph.Common.Models.Course
{
    [DataContract]
    public class CourseAppointmentOffer : IEntity, IBackendEntity
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

        #region References

        [DataMember(Order = 5, IsRequired = true)]
        [ForeignKey(nameof(CourseAppointment))]
        public long CourseAppointmentId { get; set; }

        [DataMember(Order = 6, IsRequired = true)]
        [ForeignKey(nameof(CoursePriceItem))]
        public long CoursePriceId { get; set; }


        #endregion

        [DataMember(Order = 7)]
        public OfferType OfferType { get; set; }

        [DataMember(Order = 8)]
        public string OfferDetails { get; set; } = null!;

        [DataMember(Order = 9)]
        public Uri OfferUrl { get; set; } = null!;

        [DataMember(Order = 10)]
        public bool IsAvailable { get; set; }

    }
}
