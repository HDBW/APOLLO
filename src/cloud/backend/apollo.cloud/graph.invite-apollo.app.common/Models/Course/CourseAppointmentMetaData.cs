// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace Invite.Apollo.App.Graph.Common.Models.Course
{
    [DataContract]
    public class CourseAppointmentMetaData : BaseItem
    {
        [DataMember(Order = 5)]
        [ForeignKey(nameof(MetaDataItem))]
        public long MetaDataId { get; set; }

        [DataMember(Order = 6)]
        [ForeignKey(nameof(CourseAppointment))]
        public long CourseAppointmentId { get; set; }
    }
}
