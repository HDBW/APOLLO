// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Collections.ObjectModel;
using System.Globalization;
using System.Xml;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using De.HDBW.Apollo.Client.Contracts;
using De.HDBW.Apollo.Client.Converter;
using De.HDBW.Apollo.Client.Dialogs;
using De.HDBW.Apollo.Client.Models;
using De.HDBW.Apollo.Client.Models.Course;
using De.HDBW.Apollo.Client.Models.Interactions;
using De.HDBW.Apollo.SharedContracts.Repositories;
using Invite.Apollo.App.Graph.Common.Models.Course;
using Invite.Apollo.App.Graph.Common.Models.Course.Enums;
using Microsoft.Extensions.Logging;

namespace De.HDBW.Apollo.Client.ViewModels
{
    public partial class CourseViewModel : BaseViewModel
    {
        private long? _courseItemId;

        [ObservableProperty]
        private string? _title;

        [ObservableProperty]
        private string? _description;

        [ObservableProperty]
        private string? _shortDescription;

        [ObservableProperty]
        private string? _provider;

        [ObservableProperty]
        private string? _duration;

        [ObservableProperty]
        private string? _targetGroup;

        [ObservableProperty]
        private CourseType? _courseType;

        [ObservableProperty]
        private string? _displayCourseType;

        [ObservableProperty]
        private CourseAvailability? _availability;

        [ObservableProperty]
        private DateTime? _latestUpdateFromProvider;

        [ObservableProperty]
        private string? _preRequisitesDescription;

        [ObservableProperty]
        private string? _keyPhrases;

        [ObservableProperty]
        private Uri? _courseUrl;

        [ObservableProperty]
        private OccurrenceType? _occurrence;

        [ObservableProperty]
        private string? _displayOccurrence;

        [ObservableProperty]
        private string? _language;

        [ObservableProperty]
        private string? _learningOutcomes;

        [ObservableProperty]
        private long? _instructorId;

        [ObservableProperty]
        private long? _trainingProviderId;

        [ObservableProperty]
        private long? _courseProviderId;

        [ObservableProperty]
        private string? _benefits;

        [ObservableProperty]
        private DateTime? _publishingDate;

        [ObservableProperty]
        private DateTime? _latestUpdate;

        [ObservableProperty]
        private DateTime? _deprecation;

        [ObservableProperty]
        private string? _deprecationReason;

        [ObservableProperty]
        private DateTime? _unPublishingDate;

        [ObservableProperty]
        private DateTime? _deleted;

        [ObservableProperty]
        private long? _successorId;

        [ObservableProperty]
        private long? _predecessorId;

        [ObservableProperty]
        private CourseTagType? _courseTagType;

        [ObservableProperty]
        private decimal? _price;

        [ObservableProperty]
        private string? _currency;

        [ObservableProperty]
        private string? _loanOptions;

        [ObservableProperty]
        private EduProviderItem? _trainingProvider;

        [ObservableProperty]
        private EduProviderItem? _courseProvider;

        [ObservableProperty]
        private CourseContact? _instructor;

        [ObservableProperty]
        private CourseContact? _contact;

        [ObservableProperty]
        private ObservableCollection<InteractionEntry> _skills = new ObservableCollection<InteractionEntry>();

        [ObservableProperty]
        private ObservableCollection<CourseAppointmentEntry> _courseAppointments = new ObservableCollection<CourseAppointmentEntry>();

        [ObservableProperty]
        private string? _imagePath;

        [ObservableProperty]
        private string? _decoratorText;

        public CourseViewModel(
            IDispatcherService dispatcherService,
            INavigationService navigationService,
            IDialogService dialogService,
            ICourseItemRepository courseItemRepository,
            ICourseAppointmentRepository courseAppointmentRepository,
            ICourseContactRelationRepository courseContactRelationRepository,
            ICourseContactRepository courseContactRepository,
            IEduProviderItemRepository eduProviderItemRepository,
            ILogger<CourseViewModel> logger)
            : base(
                dispatcherService,
                navigationService,
                dialogService,
                logger)
        {
            ArgumentNullException.ThrowIfNull(courseItemRepository);
            ArgumentNullException.ThrowIfNull(courseAppointmentRepository);
            ArgumentNullException.ThrowIfNull(courseContactRelationRepository);
            ArgumentNullException.ThrowIfNull(courseContactRepository);
            ArgumentNullException.ThrowIfNull(eduProviderItemRepository);
            CourseItemRepository = courseItemRepository;
            CourseAppointmentRepository = courseAppointmentRepository;
            CourseContactRelationRepository = courseContactRelationRepository;
            CourseContactRepository = courseContactRepository;
            EduProviderItemRepository = eduProviderItemRepository;
        }

        public bool HasImage
        {
            get
            {
                return !string.IsNullOrWhiteSpace(ImagePath);
            }
        }

        public string DisplayPrice
        {
            get
            {
                var parts = new List<string>();
                parts.Add("ab ");
                parts.Add((Price ?? 0).ToString());
                parts.Add(Currency ?? string.Empty);
                return string.Join(string.Empty, parts.Where(s => !string.IsNullOrWhiteSpace(s)));
            }
        }

        public bool HasPrice
        {
            get
            {
                return Price.HasValue && Price.Value > 0;
            }
        }

        public bool HasInstructor
        {
            get
            {
                return Instructor != null;
            }
        }

        public bool HasTrainingProvider
        {
            get
            {
                return TrainingProvider != null;
            }
        }

        public string? DisplayTrainingProvider
        {
            get
            {
                return TrainingProvider?.Name;
            }
        }

        public bool HasCourseProvider
        {
            get
            {
                return CourseProvider != null;
            }
        }

        public bool HasContact
        {
            get
            {
                return Contact != null;
            }
        }

        public bool HasCourseAppointments
        {
            get
            {
                return CourseAppointments?.Any() ?? false;
            }
        }

        public bool HasSkills
        {
            get
            {
                return Skills?.Any() ?? false;
            }
        }

        public bool HasLearningOutcomes
        {
            get
            {
                return !string.IsNullOrWhiteSpace(LearningOutcomes);
            }
        }

        public bool HasPreRequisitesDescription
        {
            get
            {
                return !string.IsNullOrWhiteSpace(PreRequisitesDescription);
            }
        }

        public bool HasCourseType
        {
            get
            {
                return !string.IsNullOrWhiteSpace(DisplayCourseType);
            }
        }

        private ICourseItemRepository CourseItemRepository { get; }

        private ICourseAppointmentRepository CourseAppointmentRepository { get; }

        private ICourseContactRelationRepository CourseContactRelationRepository { get; }

        private ICourseContactRepository CourseContactRepository { get; }

        private IEduProviderItemRepository EduProviderItemRepository { get; }

        public async override Task OnNavigatedToAsync()
        {
            if (!_courseItemId.HasValue)
            {
                return;
            }

            using (var worker = ScheduleWork())
            {
                try
                {
                    var courseItem = await CourseItemRepository.GetItemByIdAsync(_courseItemId.Value, worker.Token).ConfigureAwait(false);
                    if (courseItem == null)
                    {
                        return;
                    }

                    var courseAppointments = await CourseAppointmentRepository.GetItemsByForeignKeyAsync(_courseItemId.Value, worker.Token).ConfigureAwait(false);
                    var relations = await CourseContactRelationRepository.GetItemsByForeignKeyAsync(_courseItemId.Value, worker.Token).ConfigureAwait(false);
                    var contacts = await CourseContactRepository.GetItemsByIdsAsync(relations.Select(r => r.CourseContactId), worker.Token).ConfigureAwait(false);
                    var eduProviders = await EduProviderItemRepository.GetItemsByIdsAsync(new List<long>() { courseItem.CourseProviderId, courseItem.TrainingProviderId, courseItem.InstructorId }, worker.Token).ConfigureAwait(false);
                    var skills = ParseSkills(courseItem?.Skills);

                    await ExecuteOnUIThreadAsync(
                        () => LoadonUIThread(
                        courseItem,
                        courseAppointments,
                        contacts,
                        eduProviders,
                        skills), worker.Token);
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
            _courseItemId = navigationParameters.GetValue<long?>(NavigationParameter.Id);
        }

        protected override void RefreshCommands()
        {
            base.RefreshCommands();
            ShowLoanOptionsCommand?.NotifyCanExecuteChanged();
            OpenCourseCommand?.NotifyCanExecuteChanged();
            OpenMailCommand?.NotifyCanExecuteChanged();
            OpenDailerCommand?.NotifyCanExecuteChanged();
        }

        private void LoadonUIThread(
            CourseItem? courseItem,
            IEnumerable<CourseAppointment> courseAppointments,
            IEnumerable<CourseContact> contacts,
            IEnumerable<EduProviderItem> eduProviders,
            IEnumerable<(string Link, string Text)> skills)
        {
            switch (courseItem?.CourseTagType)
            {
                case Invite.Apollo.App.Graph.Common.Models.Course.Enums.CourseTagType.InfoEvent:
                    ImagePath = "placeholderinfoevent.png";
                    break;
                default:
                    ImagePath = "placeholdercontinuingeducation.png";
                    break;
            }

            IValueConverter converter = new CourseTagTypeToStringConverter();
            DecoratorText = converter.Convert(courseItem, typeof(string), null, CultureInfo.CurrentUICulture)?.ToString();
            OnPropertyChanged(nameof(HasImage));
            Title = courseItem?.Title;
            Description = courseItem?.Description?.Trim();
            ShortDescription = courseItem?.ShortDescription.Trim();
            TargetGroup = courseItem?.TargetGroup;
            CourseAppointments = new ObservableCollection<CourseAppointmentEntry>(courseAppointments?.Select(e => CourseAppointmentEntry.Import(e)) ?? new List<CourseAppointmentEntry>());
            OnPropertyChanged(nameof(HasCourseAppointments));

            CourseType = courseItem?.CourseType;
            converter = new CourseTypeToStringConverter();
            DisplayCourseType = converter.Convert(courseItem, typeof(string), null, CultureInfo.CurrentUICulture)?.ToString();
            OnPropertyChanged(nameof(HasCourseType));

            Availability = courseItem?.Availability;
            LatestUpdateFromProvider = courseItem?.LatestUpdateFromProvider;
            PreRequisitesDescription = courseItem?.PreRequisitesDescription;
            OnPropertyChanged(nameof(HasPreRequisitesDescription));
            KeyPhrases = courseItem?.KeyPhrases;
            Duration = courseItem?.Duration;

            CourseUrl = courseItem?.CourseUrl;
            Occurrence = courseItem?.Occurrence;
            converter = new OccurrenceTypeToStringConverter();
            DisplayOccurrence = converter.Convert(courseItem, typeof(string), null, CultureInfo.CurrentUICulture)?.ToString();
            Language = courseItem?.Language;

            LearningOutcomes = courseItem?.LearningOutcomes;
            OnPropertyChanged(nameof(HasLearningOutcomes));
            InstructorId = courseItem?.InstructorId;
            Instructor = contacts.FirstOrDefault(c => c.Id == InstructorId);
            OnPropertyChanged(nameof(HasInstructor));

            TrainingProviderId = courseItem?.TrainingProviderId;
            TrainingProvider = eduProviders.FirstOrDefault(c => c.Id == TrainingProviderId);
            OnPropertyChanged(nameof(HasTrainingProvider));
            OnPropertyChanged(nameof(DisplayTrainingProvider));
            CourseProviderId = courseItem?.CourseProviderId;
            CourseProvider = eduProviders.FirstOrDefault(c => c.Id == CourseProviderId);
            OnPropertyChanged(nameof(HasCourseProvider));

            Benefits = courseItem?.Benefits;
            PublishingDate = courseItem?.PublishingDate;
            LatestUpdate = courseItem?.LatestUpdate;
            Deprecation = courseItem?.Deprecation;
            DeprecationReason = courseItem?.DeprecationReason;
            UnPublishingDate = courseItem?.UnPublishingDate;
            Deleted = courseItem?.Deleted;
            SuccessorId = courseItem?.SuccessorId;
            PredecessorId = courseItem?.PredecessorId;
            CourseTagType = courseItem?.CourseTagType;

            Price = courseItem?.Price;
            Currency = courseItem?.Currency;
            OnPropertyChanged(nameof(DisplayPrice));
            OnPropertyChanged(nameof(HasPrice));
            Contact = contacts.FirstOrDefault(c => c != Instructor);
            OnPropertyChanged(nameof(HasContact));

            LoanOptions = courseItem?.LoanOptions;
            Skills = new ObservableCollection<InteractionEntry>(skills?.Select(s => InteractionEntry.Import(s.Text, s.Link, OpenSkill, CanOpenSkill)) ?? new List<InteractionEntry>());
            OnPropertyChanged(nameof(HasSkills));
        }

        private IEnumerable<(string Link, string Text)> ParseSkills(string? skills)
        {
            var result = new List<(string Link, string Text)>();
            try
            {
                if (!string.IsNullOrWhiteSpace(skills))
                {
                    var xml = new XmlDocument();
                    skills = System.Text.RegularExpressions.Regex.Unescape(skills);
                    xml.LoadXml(skills);
                    foreach (var node in xml.FirstChild?.ChildNodes.OfType<XmlNode>() ?? new List<XmlNode>())
                    {
                        var content = $"<html>{node.InnerText.Trim()}</html>";
                        var linkList = new XmlDocument();
                        linkList.LoadXml(content);
                        var links = linkList.FirstChild?.ChildNodes.OfType<XmlElement>() ?? new List<XmlElement>();
                        foreach (var link in links)
                        {
                            var url = link.Attributes["href"]?.Value;
                            var text = link.InnerText;
                            if (!string.IsNullOrWhiteSpace(url) && !string.IsNullOrWhiteSpace(text))
                            {
                                result.Add((url, text));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger?.LogError(ex, $"Unknown error in {nameof(ParseSkills)} in {GetType().Name}.");
            }

            return result;
        }

        private bool CanOpenSkill(InteractionEntry entry)
        {
            return true;
        }

        private async Task OpenSkill(InteractionEntry entry)
        {
            try
            {
                Logger?.LogDebug($"Try to opening url {entry.Data?.ToString()}.");
                var uri = new Uri(entry.Data?.ToString() ?? string.Empty, UriKind.Absolute);
                await Browser.Default.OpenAsync(uri, BrowserLaunchMode.SystemPreferred);
            }
            catch (Exception ex)
            {
                Logger?.LogError(ex, $"Unknown error in {nameof(OpenSkill)} in {GetType().Name}.");
            }
        }

        [RelayCommand(AllowConcurrentExecutions = false, CanExecute = nameof(CanOpenCourse))]
        private async Task OpenCourse(CancellationToken token)
        {
            using (var worker = ScheduleWork(token))
            {
                try
                {
                    await Browser.Default.OpenAsync(CourseUrl!, BrowserLaunchMode.SystemPreferred);
                }
                catch (OperationCanceledException)
                {
                    Logger?.LogDebug($"Canceled {nameof(OpenCourse)} in {GetType().Name}.");
                }
                catch (ObjectDisposedException)
                {
                    Logger?.LogDebug($"Canceled {nameof(OpenCourse)} in {GetType().Name}.");
                }
                catch (Exception ex)
                {
                    Logger?.LogError(ex, $"Unknown error in {nameof(OpenCourse)} in {GetType().Name}.");
                }
                finally
                {
                    UnscheduleWork(worker);
                }
            }
        }

        private bool CanOpenCourse()
        {
            return !IsBusy && CourseUrl != null;
        }

        [RelayCommand(AllowConcurrentExecutions = false, CanExecute = nameof(CanShowLoanOptions))]
        private async Task ShowLoanOptions(CancellationToken token)
        {
            using (var worker = ScheduleWork(token))
            {
                try
                {
                    var parameters = new NavigationParameters();
                    parameters.AddValue(NavigationParameter.Data, LoanOptions);
                    await DialogService.ShowPopupAsync<MessageDialog, NavigationParameters, NavigationParameters>(parameters, worker.Token);
                }
                catch (OperationCanceledException)
                {
                    Logger?.LogDebug($"Canceled {nameof(ShowLoanOptions)} in {GetType().Name}.");
                }
                catch (ObjectDisposedException)
                {
                    Logger?.LogDebug($"Canceled {nameof(ShowLoanOptions)} in {GetType().Name}.");
                }
                catch (Exception ex)
                {
                    Logger?.LogError(ex, $"Unknown error in {nameof(ShowLoanOptions)} in {GetType().Name}.");
                }
                finally
                {
                    UnscheduleWork(worker);
                }
            }
        }

        private bool CanShowLoanOptions()
        {
            return !IsBusy && !string.IsNullOrWhiteSpace(LoanOptions);
        }

        [RelayCommand(AllowConcurrentExecutions = false, CanExecute = nameof(CanOpenMail))]
        private async Task OpenMail(CancellationToken token)
        {
            using (var worker = ScheduleWork(token))
            {
                try
                {
                    if (Email.Default.IsComposeSupported)
                    {
                        string[] recipients = new[] { Contact!.ContactMail };

                        var message = new EmailMessage
                        {
                            Subject = string.Empty,
                            Body = string.Empty,
                            BodyFormat = EmailBodyFormat.PlainText,
                            To = new List<string>(recipients),
                        };

                        await Email.Default.ComposeAsync(message);
                    }
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

        private bool CanOpenMail()
        {
            return !IsBusy && !string.IsNullOrWhiteSpace(Contact?.ContactMail);
        }

        [RelayCommand(CanExecute = nameof(CanOpenDailer))]
        private void OpenDailer()
        {
            try
            {
                if (PhoneDialer.Default.IsSupported)
                {
                    PhoneDialer.Default.Open(Contact!.ContactPhone);
                }
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
        }

        private bool CanOpenDailer()
        {
            return !IsBusy && !string.IsNullOrWhiteSpace(Contact?.ContactPhone);
        }
    }
}
