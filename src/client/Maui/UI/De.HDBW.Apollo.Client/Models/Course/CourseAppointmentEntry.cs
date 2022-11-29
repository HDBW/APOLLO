// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using CommunityToolkit.Mvvm.ComponentModel;
using Invite.Apollo.App.Graph.Common.Models.Course;

namespace De.HDBW.Apollo.Client.Models.Course
{
    public partial class CourseAppointmentEntry : ObservableObject
    {
        private readonly CourseAppointment _courseAppointment;

        private CourseAppointmentEntry(CourseAppointment courseAppointment)
        {
            ArgumentNullException.ThrowIfNull(courseAppointment);
            _courseAppointment = courseAppointment;
        }

        public string DisplayAppointment
        {
            get
            {
                return $"{string.Format("{0:d}", _courseAppointment.StartDate ?? DateTime.MinValue)} - {string.Format("{0:d}", _courseAppointment.EndDate ?? DateTime.MaxValue)}";
            }
        }

        public static CourseAppointmentEntry Import(CourseAppointment courseAppointment)
        {
            return new CourseAppointmentEntry(courseAppointment);
        }
    }
}
