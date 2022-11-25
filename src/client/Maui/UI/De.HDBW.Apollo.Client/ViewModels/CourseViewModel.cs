// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Collections.ObjectModel;
using System.Security.Policy;
using System.Xml;
using CommunityToolkit.Mvvm.ComponentModel;
using De.HDBW.Apollo.Client.Contracts;
using De.HDBW.Apollo.Client.Models;
using De.HDBW.Apollo.Client.Models.Interactions;
using De.HDBW.Apollo.SharedContracts.Repositories;
using Invite.Apollo.App.Graph.Common.Models.Course;
using Invite.Apollo.App.Graph.Common.Models.Course.Enums;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.Controls.Shapes;
using static System.Net.Mime.MediaTypeNames;

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
        private TimeSpan? _duration;

        [ObservableProperty]
        private string? _targetGroup;

        [ObservableProperty]
        private CourseType? _courseType;

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
        private ObservableCollection<InteractionEntry> _skills = new ObservableCollection<InteractionEntry>();

        [ObservableProperty]
        private string? _imagePath;

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

        private void LoadonUIThread(
            CourseItem? courseItem,
            IEnumerable<CourseAppointment> courseAppointments,
            IEnumerable<CourseContact> contacts,
            IEnumerable<EduProviderItem> eduProviders,
            IEnumerable<(string Link, string Text)> skills)
        {
            ImagePath = "placeholdercontinuingeducation.png";
            //*
            Title = courseItem?.Title;
            //*
            Description = courseItem?.Description;
            ShortDescription = courseItem?.ShortDescription;
            TargetGroup = courseItem?.TargetGroup;
            //*
            CourseType = courseItem?.CourseType;
            Availability = courseItem?.Availability;
            LatestUpdateFromProvider = courseItem?.LatestUpdateFromProvider;
            PreRequisitesDescription = courseItem?.PreRequisitesDescription;
            KeyPhrases = courseItem?.KeyPhrases;
            Duration = courseItem?.Duration;
            CourseUrl = courseItem?.CourseUrl;
            Occurrence = courseItem?.Occurrence;
            Language = courseItem?.Language;
            //*
            LearningOutcomes = courseItem?.LearningOutcomes;
            InstructorId = courseItem?.InstructorId;

            //*
            TrainingProviderId = courseItem?.TrainingProviderId;
            CourseProviderId = courseItem?.CourseProviderId;

            //*
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
            LoanOptions = courseItem?.LoanOptions;
            Skills = new ObservableCollection<InteractionEntry>(skills?.Select(s => InteractionEntry.Import(s.Text, s.Link, OpenSkill, CanOpenSkill)) ?? new List<InteractionEntry>());
        }

        private IEnumerable<(string Link, string Text)> ParseSkills(string? skills)
        {
            var result = new List<(string Link, string Text)>();
            try
            {
                if (!string.IsNullOrWhiteSpace(skills))
                {
                    var xml = new XmlDocument();
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
    }
}
