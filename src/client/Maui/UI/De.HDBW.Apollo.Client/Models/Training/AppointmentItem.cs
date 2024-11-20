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
        private string _dateRange = string.Empty;

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
