// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using De.HDBW.Apollo.Client.Contracts;
using De.HDBW.Apollo.Client.Models;
using De.HDBW.Apollo.Client.Models.Training;
using De.HDBW.Apollo.Data.Helper;
using De.HDBW.Apollo.SharedContracts.Services;
using Invite.Apollo.App.Graph.Common.Models.Trainings;
using Microsoft.Extensions.Logging;
using Contact = Invite.Apollo.App.Graph.Common.Models.Contact;

namespace De.HDBW.Apollo.Client.ViewModels
{
    public partial class TrainingViewModel : BaseViewModel
    {
        private string? _trainingId;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(HasProductUrl))]
        private Uri? _productUrl;

        [ObservableProperty]
        private string? _price;

        [ObservableProperty]
        private ObservableCollection<ObservableObject> _sections = new ObservableCollection<ObservableObject>();

        private Training? _training;

        public TrainingViewModel(
            IDispatcherService dispatcherService,
            INavigationService navigationService,
            IDialogService dialogService,
            ITrainingService trainingService,
            ILogger<TrainingViewModel> logger)
            : base(
                dispatcherService,
                navigationService,
                dialogService,
                logger)
        {
            ArgumentNullException.ThrowIfNull(trainingService);
            TrainingService = trainingService;
        }

        public bool HasProductUrl
        {
            get
            {
                return ProductUrl != null;
            }
        }

        private ITrainingService TrainingService { get; }

        public async override Task OnNavigatedToAsync()
        {
            if (string.IsNullOrWhiteSpace(_trainingId) || _training != null)
            {
                return;
            }

            using (var worker = ScheduleWork())
            {
                try
                {
                    var training = await TrainingService.GetTrainingAsync(_trainingId!, worker.Token).ConfigureAwait(false);
                    var sections = new List<ObservableObject>();
                    if (training != null)
                    {
                        if (TryCreateHeader(training, out ObservableObject header))
                        {
                            sections.Add(header);
                        }

                        if (TryCreateExpandableItem(Resources.Strings.Resources.Global_Description, training.Description ?? training.ShortDescription, out ObservableObject description))
                        {
                            sections.Add(description);
                        }

                        if (TryCreateTagItem(Resources.Strings.Resources.Global_Tags, training.Tags, out ObservableObject tags))
                        {
                            sections.Add(tags);
                        }

                        if (TryCreateTagItem(Resources.Strings.Resources.Global_Categories, training.Categories, out ObservableObject categories))
                        {
                            sections.Add(categories);
                        }

                        if (TryCreateExpandableItem(Resources.Strings.Resources.Global_TargetAudience, training.TargetAudience, out ObservableObject targetAudience))
                        {
                            sections.Add(targetAudience);
                        }

                        if (TryCreateExpandableListItem(Resources.Strings.Resources.Global_PreRequisites, training.Prerequisites, out ObservableObject prerequisites))
                        {
                            sections.Add(prerequisites);
                        }

                        if (TryCreateExpandableListItem(Resources.Strings.Resources.Global_Contents, training.Content, out ObservableObject contents))
                        {
                            sections.Add(contents);
                        }

                        if (TryCreateExpandableListItem(Resources.Strings.Resources.Global_Benefits, training.BenefitList, out ObservableObject benefits))
                        {
                            sections.Add(benefits);
                        }

                        if (TryCreateExpandableListItem(Resources.Strings.Resources.Global_Certificates, training.Certificate, out ObservableObject certificates))
                        {
                            sections.Add(certificates);
                        }

                        if (TryCreateNavigationItem(Resources.Strings.Resources.TrainingsView_LoanOptions, Routes.LoansView, training.Loans?.Serialize(), out ObservableObject loans))
                        {
                            sections.Add(loans);
                        }

                        if (TryCreateContactListItem(Resources.Strings.Resources.Global_Contact, training.Contacts, out ObservableObject contacts))
                        {
                            sections.Add(contacts);
                        }

                        if (TryCreateContactItem(Resources.Strings.Resources.Global_Contact, training.Contacts, out ObservableObject contact))
                        {
                            sections.Add(contact);
                        }

                        if (TryCreateNavigationItem(Resources.Strings.Resources.TrainingsView_Appointment, Routes.AppointmentsView, training.Appointment?.Serialize(), out ObservableObject appointment))
                        {
                            sections.Add(appointment);
                        }
                        //foreach (var appointment in training.Appointment ?? new List<Appointment>())
                        //{
                        //    if (TryCreateAppointmentItem(appointment, out ObservableObject item))
                        //    {
                        //        sections.Add(item);
                        //    }
                        //}
                    }

                    //if (training?.TrainingMode == TrainingMode.Online)
                    //{
                    //    Location = Resources.Strings.Resources.Global_Location_Online;
                    //}
                    //else
                    //{
                    //    var hasSet = new HashSet<string?>();
                    //    foreach (var appointment in training?.Appointment ?? new List<Appointment>())
                    //    {
                    //        hasSet.Add(appointment.AppointmentLocation?.City);
                    //        foreach (var occurence in appointment.Occurences)
                    //        {
                    //            hasSet.Add(occurence?.Location?.City);
                    //        }
                    //    }

                    //    Location = string.Join(",", hasSet.Where(x => !string.IsNullOrWhiteSpace(x)));
                    //}

                    //Duration = training?.Appointment[0].DurationDescription

                    await ExecuteOnUIThreadAsync(() => LoadonUIThread(training, sections), worker.Token).ConfigureAwait(false);
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
            _trainingId = navigationParameters.GetValue<string?>(NavigationParameter.Id);
        }

        protected override void RefreshCommands()
        {
            base.RefreshCommands();
            OpenProductCommand?.NotifyCanExecuteChanged();
            var contactListSection = Sections.OfType<ContactListItem>().FirstOrDefault();
            contactListSection?.RefreshCommands();
            var contactItemSection = Sections.OfType<ContactItem>().FirstOrDefault();
            contactItemSection?.RefreshCommands();
            var navigationItems = Sections.OfType<NavigationItem>().ToList();
            foreach (var naviagtionItem in navigationItems)
            {
                naviagtionItem.RefreshCommands();
            }
        }

        private void LoadonUIThread(
            Training? training, List<ObservableObject> sections)
        {
            _training = training;
            ProductUrl = _training?.ProductUrl;
            Price = $"{_training?.Price ?? 0:0.##} €";
            Sections = new ObservableCollection<ObservableObject>(sections);
        }

        [RelayCommand(AllowConcurrentExecutions = false, CanExecute = nameof(CanOpenProduct))]
        private async Task OpenProduct(CancellationToken token)
        {
            using (var worker = ScheduleWork(token))
            {
                try
                {
                    await Browser.Default.OpenAsync(ProductUrl!, BrowserLaunchMode.SystemPreferred);
                }
                catch (OperationCanceledException)
                {
                    Logger?.LogDebug($"Canceled {nameof(OpenProduct)} in {GetType().Name}.");
                }
                catch (ObjectDisposedException)
                {
                    Logger?.LogDebug($"Canceled {nameof(OpenProduct)} in {GetType().Name}.");
                }
                catch (Exception ex)
                {
                    Logger?.LogError(ex, $"Unknown error in {nameof(OpenProduct)} in {GetType().Name}.");
                }
                finally
                {
                    UnscheduleWork(worker);
                }
            }
        }

        private bool CanOpenProduct()
        {
            return !IsBusy && ProductUrl != null;
        }

        private async Task OpenMail(string? email, CancellationToken token)
        {
            using (var worker = ScheduleWork(token))
            {
                try
                {
                    string[] recipients = new[] { email! };

                    var message = new EmailMessage
                    {
                        Subject = string.Empty,
                        Body = string.Empty,
                        BodyFormat = EmailBodyFormat.PlainText,
                        To = new List<string>(recipients),
                    };

                    await Email.Default.ComposeAsync(message);
                }
                catch (OperationCanceledException)
                {
                    Logger?.LogDebug($"Canceled {nameof(OpenMail)} in {GetType().Name}.");
                }
                catch (ObjectDisposedException)
                {
                    Logger?.LogDebug($"Canceled {nameof(OpenMail)} in {GetType().Name}.");
                }
                catch (Exception ex)
                {
                    Logger?.LogError(ex, $"Unknown error in {nameof(OpenMail)} in {GetType().Name}.");
                }
                finally
                {
                    UnscheduleWork(worker);
                }
            }
        }

        private bool CanOpenMail(string? email)
        {
            return !IsBusy && !string.IsNullOrWhiteSpace(email) && Email.Default.IsComposeSupported;
        }

        private Task OpenDailer(string? phoneNumber, CancellationToken token)
        {
            using (var worker = ScheduleWork(token))
            {
                try
                {
                    token.ThrowIfCancellationRequested();
                    PhoneDialer.Default.Open(phoneNumber!);
                }
                catch (OperationCanceledException)
                {
                    Logger?.LogDebug($"Canceled {nameof(OpenDailer)} in {GetType().Name}.");
                }
                catch (ObjectDisposedException)
                {
                    Logger?.LogDebug($"Canceled {nameof(OpenDailer)} in {GetType().Name}.");
                }
                catch (Exception ex)
                {
                    Logger?.LogError(ex, $"Unknown error in {nameof(OpenDailer)} in {GetType().Name}.");
                }
                finally
                {
                    UnscheduleWork(worker);
                }

                return Task.CompletedTask;
            }
        }

        private bool CanNavigateToRouteHandler(NavigationItem item)
        {
            return !IsBusy;
        }

        private async Task NavigateToRouteHandler(NavigationItem item, CancellationToken token)
        {
            using (var worker = ScheduleWork(token))
            {
                try
                {
                    await NavigationService.NavigateAsync(item.Route, token, item.Parameters).ConfigureAwait(false);
                }
                catch (OperationCanceledException)
                {
                    Logger?.LogDebug($"Canceled {nameof(NavigateToRouteHandler)} in {GetType().Name}.");
                }
                catch (ObjectDisposedException)
                {
                    Logger?.LogDebug($"Canceled {nameof(NavigateToRouteHandler)} in {GetType().Name}.");
                }
                catch (Exception ex)
                {
                    Logger?.LogError(ex, $"Unknown error in {nameof(NavigateToRouteHandler)} in {GetType().Name}.");
                }
                finally
                {
                    UnscheduleWork(worker);
                }
            }
        }

        private bool CanOpenDailer(string? phoneNumber)
        {
            return !IsBusy && !string.IsNullOrWhiteSpace(phoneNumber) && PhoneDialer.Default.IsSupported;
        }

        private bool TryCreateContactListItem(string headline, List<Contact>? contacts, out ObservableObject item)
        {
            item = null;
            if (contacts == null || contacts.Count() < 2)
            {
                return false;
            }

            item = ContactListItem.Import(
                headline,
                contacts,
                OpenMail,
                CanOpenMail,
                OpenDailer,
                CanOpenDailer);
            return true;
        }

        private bool TryCreateContactItem(string headline, List<Contact>? contacts, out ObservableObject item)
        {
            item = null;
            if (contacts == null || contacts.Count() != 1)
            {
                return false;
            }

            item = ContactItem.Import(
                headline,
                contacts.First(),
                OpenMail,
                CanOpenMail,
                OpenDailer,
                CanOpenDailer);
            return true;
        }

        private bool TryCreateExpandableListItem(string headline, IEnumerable<string>? content, out ObservableObject item)
        {
            item = null;
            if (!(content?.Any() ?? false))
            {
                return false;
            }

            content = content.Select(x => x.Trim());

            item = ExpandableListItem.Import(headline, content);
            return true;
        }

        private bool TryCreateAppointmentItem(Appointment appointment, out ObservableObject item)
        {
            item = AppointmentItem.Import(appointment);
            return true;
        }

        private bool TryCreateTagItem(string? headline, List<string>? tags, out ObservableObject item)
        {
            item = null;
            if (!(tags?.Any() ?? false))
            {
                return false;
            }

            item = TagItem.Import(headline, tags);
            return true;
        }

        private bool TryCreateExpandableItem(string headline, string? content, out ObservableObject item)
        {
            item = null;
            if (string.IsNullOrWhiteSpace(content))
            {
                return false;
            }

            item = ExpandableItem.Import(headline, content);
            return true;
        }

        private bool TryCreateHeader(Training training, out ObservableObject item)
        {
            var eduProvider = training.TrainingProvider;
            if (string.IsNullOrWhiteSpace(eduProvider?.Name))
            {
                eduProvider = training.CourseProvider;
            }

            item = HeaderItem.Import(
                     training?.TrainingName,
                     training?.SubTitle,
                     "placeholderinfoevent.png",
                     training?.TrainingType,
                     eduProvider?.Name,
                     eduProvider?.Image?.OriginalString,
                     training?.AccessibilityAvailable,
                     training?.TrainingMode,
                     training?.IndividualStartDate);

            return true;
        }

        private bool TryCreateNavigationItem(string text, string route, string? data, out ObservableObject item)
        {
            item = null;
            if (string.IsNullOrWhiteSpace(data))
            {
                return false;
            }

            var parameters = new NavigationParameters();
            parameters.AddValue(NavigationParameter.Id, _training?.Id);
            parameters.AddValue(NavigationParameter.Data, data!);
            item = NavigationItem.Import(text, route, parameters, NavigateToRouteHandler, CanNavigateToRouteHandler);
            return true;
        }
    }
}
