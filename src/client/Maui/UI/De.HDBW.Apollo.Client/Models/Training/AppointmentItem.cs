// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using Invite.Apollo.App.Graph.Common.Models.Trainings;

namespace De.HDBW.Apollo.Client.Models.Training
{
    public partial class AppointmentItem : ObservableObject
    {
        private readonly Appointment _appointment;

        [ObservableProperty]
        private ObservableCollection<string> _items = new ObservableCollection<string>();

        private AppointmentItem(Appointment appointment)
        {
            _appointment = appointment;
            var items = new List<string>()
            {
                appointment.AppointmentLocation?.City ?? string.Empty,
                appointment.AppointmentType ?? string.Empty,
                appointment.AppointmentType ?? string.Empty,
                appointment.AppointmentDescription ?? string.Empty,
                appointment.AppointmentDescription ?? string.Empty,
                appointment.StartDate.ToString(),
                appointment.EndDate.ToString(),
                $"{appointment.Duration} {appointment.DurationDescription} {appointment.DurationUnit}",
                $"{appointment.ExecutionDurationDescription} {appointment.ExecutionDuration} {appointment.ExecutionDurationUnit}",
                appointment.IsGuaranteed.ToString(),
                appointment.TrainingMode?.ToString() ?? string.Empty,
                appointment.OccurenceNoteOnTime ?? string.Empty,
                appointment.TimeInvestAttendee?.ToString() ?? string.Empty,
                appointment.TimeModel?.ToString() ?? string.Empty,
                appointment.LessonType ?? string.Empty,
                appointment.Comment ?? string.Empty,
            };

            foreach (var occupation in appointment.Occurences)
            {
                items.Add("-   : " + occupation.StartDate.ToString());
                items.Add("-   : " + occupation.EndDate.ToString());
                items.Add("-   : " + occupation.Duration.ToString());
                items.Add("-   : " + occupation.Location?.City ?? string.Empty);
            }
            Items = new ObservableCollection<string>(items);
        }

        public static AppointmentItem Import(
           Appointment appointment)
        {
            return new AppointmentItem(appointment);
        }
    }
}
