// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using Invite.Apollo.App.Graph.Common.Models.Course.Enums;

namespace Invite.Apollo.App.Graph.Common.Models.Course
{
    [DataContract]
    public class CourseAppointmentOffer : BaseItem
    {
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
        public string OfferDetails { get; set; } = string.Empty;

        [DataMember(Order = 9)]
        public Uri OfferUrl { get; set; } = null!;

        [DataMember(Order = 10)]
        public bool IsAvailable { get; set; }

    }
}
