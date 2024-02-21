// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using CommunityToolkit.Mvvm.ComponentModel;
using De.HDBW.Apollo.Client.Contracts;
using De.HDBW.Apollo.Client.Helper;
using De.HDBW.Apollo.Client.Models;
using De.HDBW.Apollo.Client.Models.Training;
using De.HDBW.Apollo.Data.Helper;
using Invite.Apollo.App.Graph.Common.Models.Trainings;
using Microsoft.Extensions.Logging;
using Contact = Invite.Apollo.App.Graph.Common.Models.Contact;

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
                    var addedAppointment = false;
                    foreach (var appointment in _appointments ?? new List<Appointment>())
                    {
                        if (addedAppointment)
                        {
                            sections.Add(SeperatorItem.Import());
                            addedAppointment = false;
                        }

                        if (TryCreateAppointmentRange(appointment, out ObservableObject? item))
                        {
                            addedAppointment = true;
                            sections.Add(item);
                        }

                        if (TryCreateAppointmentTimeModelItem(appointment, out ObservableObject? timeModel))
                        {
                            addedAppointment = true;
                            sections.Add(timeModel);
                        }

                        if (TryCreateAppointmentDescription(appointment, out ObservableObject? description))
                        {
                            addedAppointment = true;
                            sections.Add(description);
                        }

                        if (TryCreateAppointmentGuaranteed(appointment, out ObservableObject? guaranteed))
                        {
                            addedAppointment = true;
                            sections.Add(guaranteed);
                        }

                        if (TryCreateAppointmentLessonType(appointment, out ObservableObject? lessonType))
                        {
                            addedAppointment = true;
                            sections.Add(lessonType);
                        }

                        if (TryCreateAppointmentDuration(appointment, out ObservableObject? duration))
                        {
                            addedAppointment = true;
                            sections.Add(duration);
                        }

                        if (TryCreateAppointmentExecutionDuration(appointment, out ObservableObject? executionDuration))
                        {
                            addedAppointment = true;
                            sections.Add(executionDuration);
                        }

                        if (TryCreateExecutionDurationDescription(appointment, out ObservableObject? executionDescription))
                        {
                            addedAppointment = true;
                            sections.Add(executionDescription);
                        }

                        if (TryCreateClickableItem(Resources.Strings.Resources.AppointmentsView_OpenTraining, appointment.AppointmentUrl?.OriginalString, out ObservableObject? appointmentUrl))
                        {
                            addedAppointment = true;
                            sections.Add(appointmentUrl);
                        }

                        if (TryCreateContactItem(Resources.Strings.Resources.Global_Location, appointment.AppointmentLocation, out ObservableObject? contact))
                        {
                            addedAppointment = true;
                            sections.Add(contact);
                        }

                        if (TryCreateClickableItem(Resources.Strings.Resources.Global_OpenBooking, appointment.BookingUrl?.OriginalString, out ObservableObject? link))
                        {
                            addedAppointment = true;
                            sections.Add(link);
                        }

                        if (TryCreateOccurenceNoteOnTimeItem(appointment, out ObservableObject? occurenceNoteOnTime))
                        {
                            addedAppointment = true;
                            sections.Add(occurenceNoteOnTime);
                        }

                        if (appointment.Occurences?.Any() ?? false)
                        {
                            sections.Add(HeaderItem.Import(Resources.Strings.Resources.AppointmentsView_Occurences));
                            var sortedOccurences = appointment.Occurences.OrderBy(x => x.StartDate).ToList();

                            var addedOccurence = false;
                            foreach (var occurence in sortedOccurences)
                            {
                                if (TryCreateOccurenceItem(occurence, out ObservableObject? occurenceItem))
                                {
                                    addedAppointment = true;
                                    if (addedOccurence)
                                    {
                                        sections.Add(SeperatorItem.Import());
                                        addedOccurence = false;
                                    }

                                    sections.Add(occurenceItem);
                                    addedOccurence = true;
                                }
                            }
                        }

                        if (TryCreateCommentItem(appointment, out ObservableObject? comment))
                        {
                            addedAppointment = true;
                            sections.Add(comment);
                        }

                        if (addedAppointment)
                        {
                            sections.Add(SeperatorItem.Import());
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

        protected override void RefreshCommands()
        {
            base.RefreshCommands();
            Sections.OfType<InteractiveLineItem>().ToList().ForEach(line => { line.InteractCommand?.NotifyCanExecuteChanged(); });
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

        private bool TryCreateAppointmentRange(Appointment appointment, [MaybeNullWhen(false)] out ObservableObject item)
        {
            item = null;
            var rateRange = $"{appointment.StartDate.ToUIDate().ToShortDateString()}-{appointment.EndDate.ToUIDate().ToShortDateString()}";
            item = HeaderedLineItem.Import(null, rateRange, Resources.Strings.Resources.AppointmentsView_Period);
            return true;
        }

        private bool TryCreateAppointmentDuration(Appointment appointment, [MaybeNullWhen(false)] out ObservableObject item)
        {
            item = null;
            var parts = new List<string?>
            {
                appointment.DurationDescription,
                appointment.DurationUnit,
            };
            parts = parts.Where(x => !string.IsNullOrWhiteSpace(x)).ToList();
            var duration = string.Join(" ", parts);
            if (string.IsNullOrWhiteSpace(duration))
            {
                return false;
            }

            item = HeaderedLineItem.Import(null, duration, Resources.Strings.Resources.AppointmentsView_Duration);
            return true;
        }

        private bool TryCreateAppointmentExecutionDuration(Appointment appointment, [MaybeNullWhen(false)] out ObservableObject item)
        {
            item = null;
            var parts = new List<string?>
            {
                appointment.ExecutionDuration,
                appointment.ExecutionDurationUnit,
            };
            parts = parts.Where(x => !string.IsNullOrWhiteSpace(x)).ToList();
            var duration = string.Join(" ", parts);
            if (string.IsNullOrWhiteSpace(duration))
            {
                return false;
            }

            item = HeaderedLineItem.Import(null, duration, Resources.Strings.Resources.AppointmentsView_ExecutionDuration);
            return true;
        }

        private bool TryCreateAppointmentDescription(Appointment appointment, [MaybeNullWhen(false)] out ObservableObject item)
        {
            item = null;
            var description = appointment.AppointmentDescription;
            if (string.IsNullOrWhiteSpace(description))
            {
                return false;
            }

            item = HeaderedLineItem.Import(null, description, Resources.Strings.Resources.Global_Description);
            return true;
        }

        private bool TryCreateAppointmentLessonType(Appointment appointment, [MaybeNullWhen(false)] out ObservableObject item)
        {
            item = null;
            var lessonType = appointment.LessonType;
            if (string.IsNullOrWhiteSpace(lessonType))
            {
                return false;
            }

            item = HeaderedLineItem.Import(null, lessonType, Resources.Strings.Resources.AppointmentsView_LessonType);
            return true;
        }

        private bool TryCreateAppointmentGuaranteed(Appointment appointment, [MaybeNullWhen(false)] out ObservableObject item)
        {
            item = null;
            if (!appointment.IsGuaranteed)
            {
                return false;
            }

            item = LineItem.Import(null, Resources.Strings.Resources.AppointmentsView_IsGuaranteed);
            return true;
        }

        private bool TryCreateOccurenceNoteOnTimeItem(Appointment appointment, [MaybeNullWhen(false)] out ObservableObject item)
        {
            item = null;
            if (string.IsNullOrWhiteSpace(appointment.OccurenceNoteOnTime))
            {
                return false;
            }

            item = LineItem.Import(null, appointment.OccurenceNoteOnTime);
            return true;
        }

        private bool TryCreateExecutionDurationDescription(Appointment appointment, [MaybeNullWhen(false)] out ObservableObject item)
        {
            item = null;
            var description = appointment.ExecutionDurationDescription;
            if (string.IsNullOrWhiteSpace(description))
            {
                return false;
            }

            item = HeaderedLineItem.Import(null, description, Resources.Strings.Resources.AppointmentsView_ExecutionDurationDescription);
            return true;
        }

        private bool TryCreateClickableItem(string text, string? url, [MaybeNullWhen(false)] out ObservableObject item)
        {
            item = null;
            if (string.IsNullOrWhiteSpace(url))
            {
                return false;
            }

            item = ButtonLineItem.Import(text, null, url, OpenLink, CanOpenLink);
            return true;
        }

        private bool TryCreateContactItem(string? header, Contact? contact, [MaybeNullWhen(false)] out ObservableObject item)
        {
            item = null;
            if (contact == null)
            {
                return false;
            }

            var contactItem = ContactItem.Import(header, contact!, null, null, null, null);
            if (!contactItem.Items.Any())
            {
                return false;
            }

            item = contactItem;
            return true;
        }

        private bool TryCreateOccurenceItem(Occurence occurence, [MaybeNullWhen(false)] out ObservableObject item)
        {
            item = null;
            if (occurence == null)
            {
                return false;
            }

            item = OccurenceItem.Import(occurence, OnToggleState);
            return true;
        }

        private bool TryCreateCommentItem(Appointment appointment, [MaybeNullWhen(false)] out ObservableObject item)
        {
            item = null;

            if (string.IsNullOrWhiteSpace(appointment.Comment))
            {
                return false;
            }

            item = LineItem.Import(null, appointment.Comment);
            return true;
        }

        private bool TryCreateAppointmentTimeModelItem(Appointment appointment, [MaybeNullWhen(false)] out ObservableObject item)
        {
            item = null;
            string? timeModel = null;
            switch (appointment.TimeModel)
            {
                case TrainingTimeModel.Fulltime:
                    timeModel = Resources.Strings.Resources.TimeModel_Fulltime;
                    break;
                case TrainingTimeModel.Parttime:
                    timeModel = Resources.Strings.Resources.TimeModel_Parttime;
                    break;
                case TrainingTimeModel.Block:
                    timeModel = Resources.Strings.Resources.TimeModel_Block;
                    break;
                default:
                    break;
            }

            if (string.IsNullOrWhiteSpace(timeModel))
            {
                return false;
            }

            item = HeaderedLineItem.Import(null, timeModel, Resources.Strings.Resources.AppointmentsView_TimeModel);
            return true;
        }

        private void LoadonUIThread(List<ObservableObject> sections)
        {
            Sections = new ObservableCollection<ObservableObject>(sections);
        }

        private bool CanOpenLink(string? url)
        {
            return !IsBusy && !string.IsNullOrWhiteSpace(url);
        }

        private async Task OpenLink(string? url, CancellationToken token)
        {
            using (var worker = ScheduleWork(token))
            {
                try
                {
                    await Browser.Default.OpenAsync(url!, BrowserLaunchMode.SystemPreferred);
                }
                catch (OperationCanceledException)
                {
                    Logger?.LogDebug($"Canceled {nameof(OpenLink)} in {GetType().Name}.");
                }
                catch (ObjectDisposedException)
                {
                    Logger?.LogDebug($"Canceled {nameof(OpenLink)} in {GetType().Name}.");
                }
                catch (Exception ex)
                {
                    Logger?.LogError(ex, $"Unknown error in {nameof(OpenLink)} in {GetType().Name}.");
                }
                finally
                {
                    UnscheduleWork(worker);
                }
            }
        }

        private void OnToggleState(OccurenceItem item)
        {
            var index = Sections.IndexOf(item);
            if (item.IsExpanded)
            {
                Sections.Insert(index + 1, ExpandedOccurenceItem.Import(item.Items));
            }
            else
            {
                Sections.RemoveAt(index + 1);
            }
        }
    }
}
