// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using CommunityToolkit.Mvvm.ComponentModel;
using De.HDBW.Apollo.Client.Contracts;
using De.HDBW.Apollo.Client.Models;
using De.HDBW.Apollo.Client.Models.Training;
using De.HDBW.Apollo.Data.Helper;
using Invite.Apollo.App.Graph.Common.Models.Trainings;
using Microsoft.Extensions.Logging;

namespace De.HDBW.Apollo.Client.ViewModels.Training
{
    public partial class AppointmentsViewModel : BaseViewModel
    {
        [ObservableProperty]
        private ObservableCollection<ObservableObject> _sections = new ObservableCollection<ObservableObject>();

        private List<Appointment>? _appointments;

        public AppointmentsViewModel(
            IDispatcherService dispatcherService,
            INavigationService navigationService,
            IDialogService dialogService,
            ILogger<AppointmentsViewModel> logger)
            : base(dispatcherService, navigationService, dialogService, logger)
        {
        }

        public async override Task OnNavigatedToAsync()
        {
            using (var worker = ScheduleWork())
            {
                try
                {
                    var sections = new List<ObservableObject>();
                    foreach (var appointment in _appointments ?? new List<Appointment>())
                    {
                        if (TryCreateAppointmentItem(appointment, out AppointmentItem? item))
                        {
                            sections.Add(item);
                        }
                    }

                    await ExecuteOnUIThreadAsync(() => LoadonUIThread(sections), worker.Token).ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    Logger?.LogError(ex, $"Unknown error while {nameof(OnNavigatedToAsync)} in {GetType().Name}.");
                }
                finally
                {
                    UnscheduleWork(worker);
                }
            }
        }

        protected override void OnPrepare(NavigationParameters navigationParameters)
        {
            base.OnPrepare(navigationParameters);
            var data = navigationParameters.GetValue<string?>(NavigationParameter.Data);
            _appointments = data.Deserialize<List<Appointment>>();
        }

        private bool TryCreateAppointmentItem(Appointment appointment, [MaybeNullWhen(false)] out AppointmentItem item)
        {
            item = null;
            var appointmentItem = AppointmentItem.Import(appointment);
            if (!appointmentItem.Items.Any())
            {
                return false;
            }

            item = appointmentItem;
            return true;
        }

        private void LoadonUIThread(List<ObservableObject> sections)
        {
            Sections = new ObservableCollection<ObservableObject>(sections);
        }
    }
}
