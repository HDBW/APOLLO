// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Collections.ObjectModel;
using System.Globalization;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using De.HDBW.Apollo.Client.Contracts;
using De.HDBW.Apollo.Client.Converter;
using De.HDBW.Apollo.Client.Enums;
using De.HDBW.Apollo.Client.Models;
using De.HDBW.Apollo.Client.Models.Interactions;
using De.HDBW.Apollo.SharedContracts.Enums;
using De.HDBW.Apollo.SharedContracts.Repositories;
using De.HDBW.Apollo.SharedContracts.Services;
using Invite.Apollo.App.Graph.Common.Models.Assessment;
using Invite.Apollo.App.Graph.Common.Models.Assessment.Enums;
using Invite.Apollo.App.Graph.Common.Models.Course;
using Invite.Apollo.App.Graph.Common.Models.Course.Enums;
using Invite.Apollo.App.Graph.Common.Models.UserProfile;
using Microsoft.Extensions.Logging;

namespace De.HDBW.Apollo.Client.ViewModels
{
    public partial class StartViewModel : BaseViewModel
    {
        private readonly ObservableCollection<InteractionCategoryEntry> _interactionsCategories = new ObservableCollection<InteractionCategoryEntry>();

        private readonly Dictionary<InteractionEntry, object> _filtermappings = new Dictionary<InteractionEntry, object>();

        [ObservableProperty]
        private UserProfileEntry? _userProfile = UserProfileEntry.Import(new User());

        public StartViewModel(
            IPreferenceService preferenceService,
            IDispatcherService dispatcherService,
            INavigationService navigationService,
            IDialogService dialogService,
            ICourseItemRepository courseItemRepository,
            IAssessmentItemRepository assessmentItemRepository,
            IAnswerItemResultRepository answerItemResultRepository,
            IEduProviderItemRepository eduProviderItemRepository,
            ISessionService sessionService,
            ILogger<StartViewModel> logger)
            : base(dispatcherService, navigationService, dialogService, logger)
        {
            ArgumentNullException.ThrowIfNull(preferenceService);
            ArgumentNullException.ThrowIfNull(assessmentItemRepository);
            ArgumentNullException.ThrowIfNull(answerItemResultRepository);
            ArgumentNullException.ThrowIfNull(courseItemRepository);
            ArgumentNullException.ThrowIfNull(eduProviderItemRepository);
            ArgumentNullException.ThrowIfNull(sessionService);
            PreferenceService = preferenceService;
            AssessmentItemRepository = assessmentItemRepository;
            AnswerItemResultRepository = answerItemResultRepository;
            CourseItemRepository = courseItemRepository;
            EduProviderItemRepository = eduProviderItemRepository;
            SessionService = sessionService;
        }

        public ObservableCollection<InteractionCategoryEntry> InteractionCategories
        {
            get
            {
                return _interactionsCategories;
            }
        }

        public Action? UseCaseChangedHandler { get; set; }

        private IPreferenceService PreferenceService { get; }

        private IAssessmentItemRepository AssessmentItemRepository { get; }

        private IAnswerItemResultRepository AnswerItemResultRepository { get; }

        private ICourseItemRepository CourseItemRepository { get; }

        private IEduProviderItemRepository EduProviderItemRepository { get; }

        private ISessionService SessionService { get; }

        private UseCase? UseCase { get; set; }

        protected override void RefreshCommands()
        {
            base.RefreshCommands();
            foreach (var group in InteractionCategories)
            {
                group.RefreshCommands();
            }
        }

        [RelayCommand(AllowConcurrentExecutions = false, FlowExceptionsToTaskScheduler = false, IncludeCancelCommand = true)]
        private async Task LoadData(CancellationToken token)
        {
            using (var worker = ScheduleWork(token))
            {
                try
                {
                    var useCase = SessionService.UseCase;
                    var assessments = await AssessmentItemRepository.GetItemsAsync(worker.Token).ConfigureAwait(false);
                    var assessmentIds = assessments.Select(a => a.Id).ToList();
                    var assessmentResults = await AnswerItemResultRepository.GetItemsByForeignKeysAsync(assessmentIds, worker.Token).ConfigureAwait(false);
                    var notifyUseCaseChanged = SessionService.ChangedUseCase;
                    SessionService.ConfirmedUseCaseChanged();
                    UseCase = useCase;
                    var courses = await CourseItemRepository.GetItemsAsync(worker.Token).ConfigureAwait(false);
                    courses = courses.Where(c => !c.UnPublishingDate.HasValue).ToList();
                    User? userProfile = null;
                    var eduProviders = await EduProviderItemRepository.GetItemsAsync(worker.Token).ConfigureAwait(false);

                    await ExecuteOnUIThreadAsync(
                           () => LoadonUIThread(
                           assessments,
                           assessmentResults,
                           courses,
                           userProfile,
                           eduProviders,
                           notifyUseCaseChanged), worker.Token);

                    var taskList = new List<Task>();

                    /*
                    Task<NavigationParameters?>? dialogTask = null;
                    var isFirstTime = PreferenceService.GetValue(Preference.IsFirstTime, true);
                    if (isFirstTime)
                    {
                        PreferenceService.SetValue(Preference.IsFirstTime, false);
                        dialogTask = DialogService.ShowPopupAsync<FirstTimeDialog, NavigationParameters>(token);
                        taskList.Add(dialogTask);
                    }

                    if (taskList.Any())
                    {
                        await Task.WhenAll(taskList).ConfigureAwait(false);
                    }

                    var selection = dialogTask?.Result?.GetValue<bool>(NavigationParameter.Result) ?? false;
                    if (selection)
                    {
                        await NavigationService.NavigateAsnc(Routes.TutorialView, token);
                    }
                    */
                }
                catch (OperationCanceledException)
                {
                    Logger?.LogDebug($"Canceled {nameof(LoadData)} in {GetType().Name}.");
                }
                catch (ObjectDisposedException)
                {
                    Logger?.LogDebug($"Canceled {nameof(LoadData)} in {GetType().Name}.");
                }
                catch (Exception ex)
                {
                    Logger?.LogError(ex, $"Unknown error while {nameof(LoadData)} in {GetType().Name}.");
                }
                finally
                {
                    UnscheduleWork(worker);
                }
            }
        }

        private void LoadonUIThread(
           IEnumerable<AssessmentItem> assessmentItems,
           IEnumerable<AnswerItemResult> assessmentResults,
           IEnumerable<CourseItem> courseItems,
           User? userProfile,
           IEnumerable<EduProviderItem> eduProviderItems,
           bool notifyUseCaseChanged)
        {
            UserProfile = UserProfileEntry.Import(userProfile ?? new User());
            InteractionCategories.Clear();

            var interactions = new List<InteractionEntry>();
            var filters = new List<InteractionEntry>();
            IValueConverter converter = new AssessmentTypeToStringConverter();
            var favorites = new List<StartViewInteractionEntry>();
            foreach (var assesment in assessmentItems)
            {
                var filterText = converter.Convert(assesment, typeof(string), null, CultureInfo.CurrentUICulture)?.ToString() ?? string.Empty;
                if (assesment.AssessmentType == AssessmentType.Survey)
                {
                    continue;
                }

                if (!filters.Any(f => ((AssessmentType?)f.Data) == assesment.AssessmentType))
                {
                    filters.Add(FilterInteractionEntry.Import(filterText, assesment.AssessmentType, HandleFilter, CanHandleFilter));
                }

                var assemsmentData = new NavigationParameters();
                assemsmentData.AddValue<long?>(NavigationParameter.Id, assesment.Id);
                var data = new NavigationData(Routes.AssessmentDescriptionView, assemsmentData);

                var durationString = string.Format(Resources.Strings.Resources.Global_DurationFormat, !string.IsNullOrWhiteSpace(assesment.Duration) ? assesment.Duration : 0);
                var provider = !string.IsNullOrWhiteSpace(assesment.Publisher) ? assesment.Publisher : Resources.Strings.Resources.StartViewModel_UnknownProvider;
                var status = (assessmentResults?.Any(r => r.AssessmentItemId == assesment.Id) ?? false) ? Status.Processed : Status.Unknown;
                var interaction = StartViewInteractionEntry.Import<AssessmentItem>(assesment.Title, provider, Resources.Strings.Resources.AssessmentItem_DecoratorText, durationString, "placeholdertest.png", status, assesment.Id, data, HandleToggleIsFavorite, CanHandleToggleIsFavorite, HandleInteract, CanHandleInteract);
                ((StartViewInteractionEntry)interaction).IsFavorite = SessionService.GetFavorites().Any(f => f.Id == assesment.Id && f.Type == typeof(AssessmentItem));
                if (((StartViewInteractionEntry)interaction).IsFavorite)
                {
                    favorites.Add((StartViewInteractionEntry)interaction);
                }

                interactions.Add(interaction);
                _filtermappings.Add(interaction, assesment.AssessmentType);
            }

            converter = new CourseTagTypeToStringConverter();
            foreach (var course in courseItems)
            {
                var decoratorText = converter.Convert(course, typeof(string), null, CultureInfo.CurrentUICulture)?.ToString() ?? string.Empty;

                if (!filters.Any(f => ((CourseTagType?)f.Data) == course.CourseTagType))
                {
                    filters.Add(FilterInteractionEntry.Import(decoratorText, course.CourseTagType, HandleFilter, CanHandleFilter));
                }

                var courseData = new NavigationParameters();
                courseData.AddValue<long?>(NavigationParameter.Id, course.Id);
                var data = new NavigationData(Routes.TrainingView, courseData);

                var eduProvider = eduProviderItems?.FirstOrDefault(p => p.Id == course.CourseProviderId);

                var duration = course.Duration ?? string.Empty;
                var provider = !string.IsNullOrWhiteSpace(eduProvider?.Name) ? eduProvider.Name : Resources.Strings.Resources.StartViewModel_UnknownProvider;
                var image = "placeholdercontinuingeducation.png";
                switch (course.CourseTagType)
                {
                    case CourseTagType.InfoEvent:
                        image = "placeholderinfoevent.png";
                        break;
                }

                var interaction = StartViewInteractionEntry.Import<CourseItem>(course.Title, provider, decoratorText, duration, image, Status.Unknown, course.Id, data, HandleToggleIsFavorite, CanHandleToggleIsFavorite, HandleInteract, CanHandleInteract);
                ((StartViewInteractionEntry)interaction).IsFavorite = SessionService.GetFavorites().Any(f => f.Id == course.Id && f.Type == typeof(CourseItem));
                if (((StartViewInteractionEntry)interaction).IsFavorite)
                {
                    favorites.Add((StartViewInteractionEntry)interaction);
                }

                interactions.Add(interaction);
                _filtermappings.Add(interaction, course.CourseTagType);
            }

            var headline = string.Empty;
            if (UseCase.HasValue)
            {
                switch (UseCase.Value)
                {
                    case SharedContracts.Enums.UseCase.A:
                        headline = Resources.Strings.Resources.StartViewModel_UseCaseA_RecomondationsHeadline;
                        break;
                    case SharedContracts.Enums.UseCase.B:
                        headline = Resources.Strings.Resources.StartViewModel_UseCaseB_RecomondationsHeadline;
                        break;
                    case SharedContracts.Enums.UseCase.C:
                        headline = Resources.Strings.Resources.StartViewModel_UseCaseC_RecomondationsHeadline;
                        break;
                }
            }

            InteractionCategories.Add(InteractionCategoryEntry.Import(headline, Resources.Strings.Resources.StartViewModel_RecomondationsSubline, interactions, filters, null, HandleShowMore, CanHandleShowMore));

            interactions = new List<InteractionEntry>();
            filters = new List<InteractionEntry>();
            var favoriteInteractionCategoryEntry = FavoriteInteractionCategoryEntry.Import(Resources.Strings.Resources.StartViewModel_FavoritesHeadline, Resources.Strings.Resources.StartViewModel_FavoritesSubline, interactions, filters, null, HandleShowMore, CanHandleShowMore);
            AddFavorites((FavoriteInteractionCategoryEntry)favoriteInteractionCategoryEntry, favorites);
            InteractionCategories.Add(favoriteInteractionCategoryEntry);
            if (notifyUseCaseChanged)
            {
                UseCaseChangedHandler?.Invoke();
            }
        }

        private bool CanHandleFilter(InteractionEntry interaction)
        {
            return !IsBusy;
        }

        private Task HandleFilter(InteractionEntry interaction)
        {
            var filterEntry = interaction as FilterInteractionEntry;
            var group = InteractionCategories.FirstOrDefault(i => i.Filters.Contains(interaction));
            if (filterEntry == null || group == null)
            {
                return Task.CompletedTask;
            }

            filterEntry.IsSelected = !filterEntry.IsSelected;
            var selectedFilters = group.Filters.OfType<FilterInteractionEntry>().Where(f => f.IsSelected);
            var assessmentFilters = selectedFilters.Select(f => f.Data).OfType<AssessmentType>().ToList();
            var courseFilters = selectedFilters.Select(f => f.Data).OfType<CourseTagType>().ToList();
            var items = group.Interactions.OfType<StartViewInteractionEntry>();
            foreach (var item in items)
            {
                if (!selectedFilters.Any())
                {
                    item.IsFiltered = false;
                    continue;
                }

                if (item.EntityType == typeof(AssessmentItem) && assessmentFilters.Any())
                {
                    item.IsFiltered = IsFilteredByAssessmentType(_filtermappings[item] as AssessmentType?, assessmentFilters);
                }
                else if (item.EntityType == typeof(CourseItem) && courseFilters.Any())
                {
                    item.IsFiltered = IsFilteredByCourseTagType(_filtermappings[item] as CourseTagType?, courseFilters);
                }
                else
                {
                    item.IsFiltered = true;
                }
            }

            return Task.CompletedTask;
        }

        private bool IsFilteredByCourseTagType(CourseTagType? courseTagType, List<CourseTagType> selectedCourseTagType)
        {
            if (!courseTagType.HasValue || !selectedCourseTagType.Any())
            {
                return true;
            }

            return !selectedCourseTagType.Contains(courseTagType.Value);
        }

        private bool IsFilteredByAssessmentType(AssessmentType? assessmentType, IList<AssessmentType> selectedAssessmentTypes)
        {
            if (!assessmentType.HasValue || !selectedAssessmentTypes.Any())
            {
                return true;
            }

            return !selectedAssessmentTypes.Contains(assessmentType.Value);
        }

        private bool CanHandleInteract(InteractionEntry interaction)
        {
            return !IsBusy;
        }

        private async Task HandleInteract(InteractionEntry interaction)
        {
            switch (interaction.Data)
            {
                case NavigationData navigationData:
                    await NavigationService.NavigateAsync(navigationData.Route, CancellationToken.None, navigationData.Parameters);
                    break;
                default:
                    Logger.LogWarning($"Unknown interaction data {interaction?.Data ?? "null"} while {nameof(HandleInteract)} in {GetType().Name}.");
                    break;
            }
        }

        private Task HandleShowMore(InteractionCategoryEntry entry)
        {
            return Task.CompletedTask;
        }

        private bool CanHandleShowMore(InteractionCategoryEntry entry)
        {
            return true;
        }

        private bool CanHandleToggleIsFavorite(StartViewInteractionEntry entry)
        {
            return entry != null;
        }

        private Task HandleToggleIsFavorite(StartViewInteractionEntry entry)
        {
            if (entry.IsFavorite)
            {
                return HandleRemoveFavorite(entry);
            }
            else
            {
                return HandleMakeFavorite(entry);
            }
        }

        private bool CanHandleMakeFavorite(StartViewInteractionEntry entry)
        {
            return !entry.IsFavorite;
        }

        private Task HandleMakeFavorite(StartViewInteractionEntry entry)
        {
            entry.IsFavorite = true;
            SessionService.AddFavorite(entry.EntityId, entry.EntityType);
            var favoriteInteraction = InteractionCategories.OfType<FavoriteInteractionCategoryEntry>().FirstOrDefault();
            if (favoriteInteraction == null)
            {
                return Task.CompletedTask;
            }

            InteractionCategories.Remove(favoriteInteraction);
            var interaction = entry.Clone() as InteractionEntry;
            favoriteInteraction.AddInteraction(interaction!);
            InteractionCategories.Add(favoriteInteraction);
            return Task.CompletedTask;
        }

        private void AddFavorites(FavoriteInteractionCategoryEntry favoriteInteraction, IEnumerable<StartViewInteractionEntry> entries)
        {
            foreach (var entry in entries)
            {
                var interaction = entry.Clone() as InteractionEntry;
                favoriteInteraction.AddInteraction(interaction!);
            }
        }

        private bool CanHandleRemoveFavorite(StartViewInteractionEntry entry)
        {
            return !entry.IsFavorite;
        }

        private Task HandleRemoveFavorite(StartViewInteractionEntry entry)
        {
            var category = InteractionCategories.FirstOrDefault(c => c.Interactions.Contains(entry));
            var favoriteInteraction = InteractionCategories.OfType<FavoriteInteractionCategoryEntry>().FirstOrDefault();

            if (favoriteInteraction == null || category == null)
            {
                return Task.CompletedTask;
            }

            var sourceEntry = entry;
            if (favoriteInteraction == category)
            {
                category = InteractionCategories.FirstOrDefault(c => c.Interactions.OfType<StartViewInteractionEntry>().Any(i => i.Data == entry.Data));
                sourceEntry = category?.Interactions.OfType<StartViewInteractionEntry>().First(i => i.Data == entry.Data);
            }
            else
            {
                entry = favoriteInteraction.Interactions.OfType<StartViewInteractionEntry>().First(i => i.Data == entry.Data);
            }

            if (sourceEntry == null)
            {
                return Task.CompletedTask;
            }

            sourceEntry.IsFavorite = false;
            SessionService.RemoveFavorite(sourceEntry.EntityId, sourceEntry.EntityType);
            if (favoriteInteraction.Interactions.Contains(entry))
            {
                InteractionCategories.Remove(favoriteInteraction);
                favoriteInteraction.RemoveInteraction(entry);
                InteractionCategories.Add(favoriteInteraction);
            }

            return Task.CompletedTask;
        }
    }
}
