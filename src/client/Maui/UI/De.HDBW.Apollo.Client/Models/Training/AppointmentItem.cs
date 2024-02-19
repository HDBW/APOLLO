// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using De.HDBW.Apollo.Client.Helper;
using Invite.Apollo.App.Graph.Common.Models.Trainings;

namespace De.HDBW.Apollo.Client.Models.Training
{
    public partial class AppointmentItem : ObservableObject
    {
        private readonly Appointment _appointment;

        [ObservableProperty]
        private string _dateRange;

        [ObservableProperty]
        private bool _isGuaranteed;

        [ObservableProperty]
        private string? _description;

        [ObservableProperty]
        private string? _appointmentUrl;

        [ObservableProperty]
        private string? _duration;

        [ObservableProperty]
        private string? _executionDuration;

        [ObservableProperty]
        private string? _executionDurationDescription;

        [ObservableProperty]
        private string? _timeModel;

        [ObservableProperty]
        private string? _occurenceNoteOnTime;

        [ObservableProperty]
        private string? _lessonType;

        [ObservableProperty]
        private string? _bookingUrl;

        [ObservableProperty]
        private ContactItem? _contact;

        [ObservableProperty]
        private ObservableCollection<OccurenceItem> _occurences = new ObservableCollection<OccurenceItem>();

        [ObservableProperty]
        private ObservableCollection<BindableObject> _items = new ObservableCollection<BindableObject>();

        private AppointmentItem(Appointment appointment)
        {
            _appointment = appointment;
            Description = appointment.AppointmentDescription;

            AppointmentUrl = appointment.AppointmentUrl?.OriginalString;
            IsGuaranteed = appointment.IsGuaranteed;

            switch (appointment.TimeModel)
            {
                case TrainingTimeModel.Fulltime:
                    TimeModel = Resources.Strings.Resources.TimeModel_Fulltime;
                    break;
                case TrainingTimeModel.Parttime:
                    TimeModel = Resources.Strings.Resources.TimeModel_Parttime;
                    break;
                case TrainingTimeModel.Block:
                    TimeModel = Resources.Strings.Resources.TimeModel_Block;
                    break;
                default:
                    break;
            }

            OccurenceNoteOnTime = appointment.OccurenceNoteOnTime;
            LessonType = appointment.LessonType;

            DateRange = $"{appointment.StartDate.ToUIDate().ToShortDateString()}-{appointment.EndDate.ToUIDate().ToShortDateString()}";
            var parts = new List<string?>();
            parts.Add(appointment.DurationDescription);
            parts.Add(appointment.DurationUnit);
            parts = parts.Where(x => !string.IsNullOrWhiteSpace(x)).ToList();
            Duration = string.Join(" ", parts);

            parts = new List<string?>();
            parts.Add(appointment.ExecutionDuration);
            parts.Add(appointment.ExecutionDurationUnit);
            parts = parts.Where(x => !string.IsNullOrWhiteSpace(x)).ToList();
            ExecutionDuration = string.Join(" ", parts);

            ExecutionDurationDescription = appointment.ExecutionDurationDescription;
            BookingUrl = appointment.BookingUrl?.OriginalString;

            if (appointment.AppointmentLocation != null)
            {
                var contact = ContactItem.Import(Resources.Strings.Resources.Global_Location, appointment.AppointmentLocation, null, null, null, null);
                if (contact.Items.Any())
                {
                    Contact = Contact;
                }
            }

            foreach (var occurence in appointment.Occurences)
            {
                var item = OccurenceItem.Import(occurence, null);
                Occurences.Add(item);
            }
        }

        public static AppointmentItem Import(Appointment appointment)
        {
            return new AppointmentItem(appointment);
        }
    }
}
