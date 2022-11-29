// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using De.HDBW.Apollo.Client.Contracts;
using De.HDBW.Apollo.Client.Models.Assessment;
using De.HDBW.Apollo.Data.Repositories;
using De.HDBW.Apollo.Data.Services;
using De.HDBW.Apollo.SharedContracts.Repositories;
using Invite.Apollo.App.Graph.Common.Models;
using Invite.Apollo.App.Graph.Common.Models.Assessment;
using Invite.Apollo.App.Graph.Common.Models.Assessment.Enums;
using Invite.Apollo.App.Graph.Common.Models.UserProfile;
using Microsoft.Extensions.Logging;

namespace De.HDBW.Apollo.Client.ViewModels
{
    public partial class FeedbackViewModel : BaseViewModel
    {
        [ObservableProperty]
        private ObservableCollection<QuestionEntry> _questions = new ObservableCollection<QuestionEntry>();

        [ObservableProperty]
        private ObservableCollection<LayoutType> _questionLayouts = new ObservableCollection<LayoutType>() { LayoutType.Default, LayoutType.Overlay, LayoutType.Compare, LayoutType.UniformGrid, LayoutType.HorizontalList };

        [ObservableProperty]
        private ObservableCollection<LayoutType> _answerLayouts = new ObservableCollection<LayoutType>() { LayoutType.Default, LayoutType.Overlay, LayoutType.Compare, LayoutType.UniformGrid, LayoutType.HorizontalList };

        [ObservableProperty]
        private ObservableCollection<InteractionType> _interactions = new ObservableCollection<InteractionType>() { InteractionType.SingleSelect, InteractionType.MultiSelect, InteractionType.Input, InteractionType.Associate };

        [ObservableProperty]
        private QuestionEntry? _currentQuestion;

        [ObservableProperty]
        private bool _showSubmitButton;

        [ObservableProperty]
        private bool _showNextButton = true;

        private LayoutType? _answerLayout;
        private LayoutType? _questionLayout;
        private InteractionType? _interaction;

        private long _maxResultId;

        public FeedbackViewModel(
            IAssessmentItemRepository assessmentItemRepository,
            IQuestionItemRepository questionItemRepository,
            IAnswerItemRepository answerItemRepository,
            IMetaDataMetaDataRelationRepository metaDataMetaDataRelationRepository,
            IAnswerMetaDataRelationRepository answerMetaDataRelationRepository,
            IAnswerItemResultRepository answerItemResultRepository,
            IQuestionMetaDataRelationRepository questionMetaDataRelationRepository,
            IMetaDataRepository metaDataRepository,
            IDispatcherService dispatcherService,
            INavigationService navigationService,
            IDialogService dialogService,
            IFeedbackService feedbackService,
            ILogger<FeedbackViewModel> logger)
            : base(dispatcherService, navigationService, dialogService, logger)
        {
            ArgumentNullException.ThrowIfNull(assessmentItemRepository);
            ArgumentNullException.ThrowIfNull(questionItemRepository);
            ArgumentNullException.ThrowIfNull(answerItemRepository);
            ArgumentNullException.ThrowIfNull(answerItemResultRepository);
            ArgumentNullException.ThrowIfNull(metaDataMetaDataRelationRepository);
            ArgumentNullException.ThrowIfNull(answerMetaDataRelationRepository);
            ArgumentNullException.ThrowIfNull(questionMetaDataRelationRepository);
            ArgumentNullException.ThrowIfNull(metaDataRepository);
            ArgumentNullException.ThrowIfNull(feedbackService);

            AssessmentItemRepository = assessmentItemRepository;
            QuestionItemRepository = questionItemRepository;
            AnswerItemResultRepository = answerItemResultRepository;
            AnswerItemRepository = answerItemRepository;
            MetaDataMetaDataRelationRepository = metaDataMetaDataRelationRepository;
            AnswerMetaDataRelationRepository = answerMetaDataRelationRepository;
            QuestionMetaDataRelationRepository = questionMetaDataRelationRepository;
            MetaDataRepository = metaDataRepository;
            FeedbackService = feedbackService;
        }

        public LayoutType? AnswerLayout
        {
            get
            {
                return _answerLayout;
            }

            set
            {
                if (SetProperty(ref _answerLayout, value))
                {
                    if (CurrentQuestion != null)
                    {
                        CurrentQuestion.AnswerLayout = value ?? LayoutType.Default;
                    }

                    OnPropertyChanged(nameof(CurrentQuestion));
                }
            }
        }

        public double Progress
        {
            get
            {
                return CurrentQuestion != null && Questions.Count > 0 ? (((double)Questions.IndexOf(CurrentQuestion) + 1d) / (double)Questions.Count()) : 0d;
            }
        }

        public string DisplayProgress
        {
            get
            {
                return CurrentQuestion != null && Questions.Count > 0 ? $"{Questions.IndexOf(CurrentQuestion) + 1d} / {Questions.Count()}" : "0 / 0";
            }
        }

        public LayoutType? QuestionLayout
        {
            get
            {
                return _questionLayout;
            }

            set
            {
                if (SetProperty(ref _questionLayout, value))
                {
                    if (CurrentQuestion != null)
                    {
                        CurrentQuestion.QuestionLayout = value ?? LayoutType.Default;
                    }

                    OnPropertyChanged(nameof(CurrentQuestion));
                }
            }
        }

        public InteractionType? Interaction
        {
            get
            {
                return _interaction;
            }

            set
            {
                if (SetProperty(ref _interaction, value))
                {
                    if (CurrentQuestion != null)
                    {
                        CurrentQuestion.Interaction = value ?? InteractionType.Unknown;
                    }

                    OnPropertyChanged(nameof(CurrentQuestion));
                }
            }
        }

        private IFeedbackService FeedbackService { get; }

        private IAssessmentItemRepository AssessmentItemRepository { get; }

        private IQuestionItemRepository QuestionItemRepository { get; }

        private IAnswerItemResultRepository AnswerItemResultRepository { get; }

        private IAnswerItemRepository AnswerItemRepository { get; }

        private IMetaDataMetaDataRelationRepository MetaDataMetaDataRelationRepository { get; }

        private IAnswerMetaDataRelationRepository AnswerMetaDataRelationRepository { get; }

        private IQuestionMetaDataRelationRepository QuestionMetaDataRelationRepository { get; }

        private IMetaDataRepository MetaDataRepository { get; }

        public async override Task OnNavigatedToAsync()
        {
            using (var worker = ScheduleWork())
            {
                try
                {
                    var assessmentItems = await AssessmentItemRepository.GetItemByAssessmentTypeAsync(AssessmentType.Survey, worker.Token).ConfigureAwait(false);
                    var assessmentItem = assessmentItems?.FirstOrDefault();
                    if (assessmentItem == null)
                    {
                        return;
                    }

                    var questionItems = await QuestionItemRepository.GetItemsByForeignKeyAsync(assessmentItem.Id, worker.Token).ConfigureAwait(false);
                    questionItems = questionItems ?? new List<QuestionItem>();
                    var questionIds = questionItems.Select(q => q.Id);
                    var relatedMetaDataIds = new List<long>();
                    var questionMetaDataIds = new List<long>();
                    var answerMetaDataIds = new List<long>();

                    var answerItems = await AnswerItemRepository.GetItemsByForeignKeysAsync(questionIds, worker.Token).ConfigureAwait(false);
                    var answerIds = answerItems.Select(q => q.Id);

                    var answerItemResults = await AnswerItemResultRepository.GetItemsByForeignKeyAsync(assessmentItem.Id, worker.Token).ConfigureAwait(false);
                    _maxResultId = await AnswerItemResultRepository.GetNextIdAsync(worker.Token).ConfigureAwait(false);
                    var questionMetaDataRelations = await QuestionMetaDataRelationRepository.GetItemsByForeignKeysAsync(questionIds, worker.Token).ConfigureAwait(false);
                    questionMetaDataRelations = questionMetaDataRelations ?? new List<QuestionMetaDataRelation>();
                    questionMetaDataIds.AddRange(questionMetaDataRelations.Select(r => r.MetaDataId).Distinct());
                    relatedMetaDataIds.AddRange(questionMetaDataIds);

                    var answerMetaDataRelations = await AnswerMetaDataRelationRepository.GetItemsByForeignKeysAsync(answerIds, worker.Token).ConfigureAwait(false);
                    answerMetaDataRelations = answerMetaDataRelations ?? new List<AnswerMetaDataRelation>();
                    answerMetaDataIds.AddRange(answerMetaDataRelations.Select(r => r.MetaDataId).Distinct());
                    relatedMetaDataIds.AddRange(answerMetaDataIds);

                    relatedMetaDataIds = relatedMetaDataIds.Distinct().ToList();
                    var metaDataMetaDataRelationRelations = await MetaDataMetaDataRelationRepository.GetItemsBySourceIdsAsync(relatedMetaDataIds, worker.Token).ConfigureAwait(false);
                    metaDataMetaDataRelationRelations = metaDataMetaDataRelationRelations ?? new List<MetaDataMetaDataRelation>();

                    relatedMetaDataIds.AddRange(metaDataMetaDataRelationRelations.Select(r => r.TargetId));
                    relatedMetaDataIds.AddRange(metaDataMetaDataRelationRelations.Select(r => r.SourceId));
                    relatedMetaDataIds = relatedMetaDataIds.Distinct().ToList();

                    var relatedMetaDatas = await MetaDataRepository.GetItemsByIdsAsync(relatedMetaDataIds, worker.Token).ConfigureAwait(false);
                    relatedMetaDatas = relatedMetaDatas ?? new List<MetaDataItem>();

                    var questionQuestionMetaDatasMapping = new Dictionary<QuestionItem, IEnumerable<MetaDataItem>>();
                    var questionQuestionMetaDataMetaDatasMapping = new Dictionary<QuestionItem, Dictionary<MetaDataItem, IEnumerable<MetaDataItem>>>();

                    var questionAnswerItemsMapping = new Dictionary<QuestionItem, IEnumerable<AnswerItem>>();
                    var questionAnswerResultsMapping = new Dictionary<QuestionItem, IEnumerable<AnswerItemResult>>();
                    var questionAnswerItemMetaDatasMapping = new Dictionary<QuestionItem, Dictionary<AnswerItem, IEnumerable<MetaDataItem>>>();

                    foreach (var questionItem in questionItems)
                    {
                        var relationIds = questionMetaDataRelations.Where(r => r.QuestionId == questionItem.Id).Select(r => r.MetaDataId).ToList();
                        var questionMetaDatas = relatedMetaDatas.Where(m => relationIds.Contains(m.Id)).ToList();
                        questionQuestionMetaDatasMapping.Add(questionItem, questionMetaDatas);
                        var questionMetaDataMetaDatasMappings = metaDataMetaDataRelationRelations.Where(m => relationIds.Contains(m.SourceId)).ToList();
                        var metaDataMetaDataMappings = new Dictionary<MetaDataItem, IEnumerable<MetaDataItem>>();
                        foreach (var mapping in questionMetaDataMetaDatasMappings.GroupBy(m => m.SourceId))
                        {
                            var targetIds = mapping.Select(m => m.TargetId);
                            metaDataMetaDataMappings.Add(questionMetaDatas.First(m => m.Id == mapping.Key), relatedMetaDatas.Where(m => targetIds.Contains(m.Id)).ToList());
                        }

                        questionQuestionMetaDataMetaDatasMapping.Add(questionItem, metaDataMetaDataMappings);

                        var questionAnswers = answerItems.Where(a => a.QuestionId == questionItem.Id).ToList();
                        var questionAnswerIds = questionAnswers.Select(a => a.Id);
                        questionAnswerItemsMapping.Add(questionItem, questionAnswers);

                        var questionAnswerResults = answerItemResults.Where(a => questionAnswerIds.Contains(a.AnswerItemId)).ToList();
                        questionAnswerResultsMapping.Add(questionItem, questionAnswerResults);

                        var answerAnswerMetaDatasMapping = new Dictionary<AnswerItem, IEnumerable<MetaDataItem>>();
                        questionAnswerItemMetaDatasMapping.Add(questionItem, answerAnswerMetaDatasMapping);
                        foreach (var answerItem in questionAnswers)
                        {
                            var answerRelationIds = answerMetaDataRelations.Where(r => r.AnswerId == answerItem.Id).Select(r => r.MetaDataId).ToList();
                            var anserItemMetaDatas = relatedMetaDatas.Where(m => answerRelationIds.Contains(m.Id)).ToList();

                            answerAnswerMetaDatasMapping.Add(answerItem, anserItemMetaDatas);
                            var anserItemMetaDataMetaDatasMappings = metaDataMetaDataRelationRelations.Where(m => answerRelationIds.Contains(m.SourceId)).ToList();
                            foreach (var mapping in anserItemMetaDataMetaDatasMappings.GroupBy(m => m.SourceId))
                            {
                                var targetIds = mapping.Select(m => m.TargetId);
                                anserItemMetaDatas.AddRange(relatedMetaDatas.Where(m => targetIds.Contains(m.Id)).ToList());
                            }
                        }
                    }

                    await ExecuteOnUIThreadAsync(
                        () => LoadonUIThread(
                        questionItems,
                        questionQuestionMetaDatasMapping,
                        questionQuestionMetaDataMetaDatasMapping,
                        questionAnswerItemsMapping,
                        questionAnswerResultsMapping,
                        questionAnswerItemMetaDatasMapping), worker.Token);
                }
                catch (OperationCanceledException)
                {
                    Logger?.LogDebug($"Canceled {nameof(OnNavigatedToAsync)} in {GetType().Name}.");
                }
                catch (ObjectDisposedException)
                {
                    Logger?.LogDebug($"Canceled {nameof(OnNavigatedToAsync)} in {GetType().Name}.");
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

        protected override void RefreshCommands()
        {
            base.RefreshCommands();
            CancelCommand?.NotifyCanExecuteChanged();
            NextQuestionCommand?.NotifyCanExecuteChanged();
        }

        private void LoadonUIThread(
            IEnumerable<QuestionItem> questionItems,
            Dictionary<QuestionItem, IEnumerable<MetaDataItem>> questionMetaDatasMapping,
            Dictionary<QuestionItem, Dictionary<MetaDataItem, IEnumerable<MetaDataItem>>> questionMetaDataMetaDatasMappings,
            Dictionary<QuestionItem, IEnumerable<AnswerItem>> answerItemsMapping,
            Dictionary<QuestionItem, IEnumerable<AnswerItemResult>> answerResultsMapping,
            Dictionary<QuestionItem, Dictionary<AnswerItem, IEnumerable<MetaDataItem>>> answerItemMetaDatasMapping)
        {
            Questions = new ObservableCollection<QuestionEntry>(questionItems.Select(q => QuestionEntry.Import(q, questionMetaDatasMapping[q], questionMetaDataMetaDatasMappings[q], answerItemsMapping[q], answerResultsMapping[q], answerItemMetaDatasMapping[q], Logger)));
            CurrentQuestion = Questions?.FirstOrDefault();
            _questionLayout = CurrentQuestion?.QuestionLayout ?? LayoutType.Default;
            _answerLayout = CurrentQuestion?.AnswerLayout ?? LayoutType.Default;
            OnPropertyChanged(nameof(QuestionLayout));
            OnPropertyChanged(nameof(AnswerLayout));
            OnPropertyChanged(nameof(Interaction));
            OnPropertyChanged(nameof(Progress));
            OnPropertyChanged(nameof(DisplayProgress));
        }

        [RelayCommand(AllowConcurrentExecutions = false, FlowExceptionsToTaskScheduler = false, IncludeCancelCommand = true)]
        private async Task NextQuestion(CancellationToken token)
        {
            using (var worker = ScheduleWork(token))
            {
                try
                {
                    await SaveAssessmentAsync(worker.Token);
                    var questionResults = CurrentQuestion?.ExportResultes() ?? new List<AnswerItemResult>();
                    var currentIndex = CurrentQuestion != null ? Questions.IndexOf(CurrentQuestion) : 0;

                    var hasNextQuestion = currentIndex + 2 < Questions.Count;
                    if (!hasNextQuestion)
                    {
                        currentIndex = currentIndex + 1;
                        ShowSubmitButton = true;
                        ShowNextButton = false;
                        return;
                    }

                    currentIndex = currentIndex + 1;
                    CurrentQuestion = Questions[currentIndex];
                    _questionLayout = CurrentQuestion?.QuestionLayout ?? LayoutType.Default;
                    _answerLayout = CurrentQuestion?.AnswerLayout ?? LayoutType.Default;
                    _interaction = CurrentQuestion?.Interaction ?? InteractionType.Unknown;
                    OnPropertyChanged(nameof(QuestionLayout));
                    OnPropertyChanged(nameof(AnswerLayout));
                    OnPropertyChanged(nameof(Interaction));
                    OnPropertyChanged(nameof(Progress));
                    OnPropertyChanged(nameof(DisplayProgress));
                }
                catch (OperationCanceledException)
                {
                    Logger?.LogDebug($"Canceled {nameof(NextQuestion)} in {GetType().Name}.");
                }
                catch (ObjectDisposedException)
                {
                    Logger?.LogDebug($"Canceled {nameof(NextQuestion)} in {GetType().Name}.");
                }
                catch (Exception ex)
                {
                    Logger?.LogError(ex, $"Unknown error while {nameof(NextQuestion)} in {GetType().Name}.");
                }
                finally
                {
                    UnscheduleWork(worker);
                }
            }
        }

        private Task<bool> SaveAssessmentAsync(CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            var itemsToUpdate = new List<AnswerItemResult>();
            foreach (var question in Questions)
            {
                itemsToUpdate.AddRange(question.ExportResultes());
            }

            foreach (var item in itemsToUpdate.Where(i => i.Id < 0))
            {
                item.Id = _maxResultId;
                _maxResultId = _maxResultId + 1;
            }

            return AnswerItemResultRepository.AddOrUpdateItemsAsync(itemsToUpdate, token);
        }

        [RelayCommand(AllowConcurrentExecutions = false, CanExecute = nameof(CanCancel))]
        private async Task Cancel(CancellationToken token)
        {
            using (var worker = ScheduleWork(token))
            {
                try
                {
                    await SaveAssessmentAsync(worker.Token);
                    await NavigationService.PushToRootAsnc(worker.Token);
                }
                catch (OperationCanceledException)
                {
                    Logger?.LogDebug($"Canceled {nameof(Cancel)} in {GetType().Name}.");
                }
                catch (ObjectDisposedException)
                {
                    Logger?.LogDebug($"Canceled {nameof(Cancel)} in {GetType().Name}.");
                }
                catch (Exception ex)
                {
                    Logger?.LogError(ex, $"Unknown error in {nameof(Cancel)} in {GetType().Name}.");
                }
                finally
                {
                    UnscheduleWork(worker);
                }
            }
        }

        private bool CanCancel()
        {
            return !IsBusy;
        }

        [RelayCommand(AllowConcurrentExecutions = false, CanExecute = nameof(CanSubmit))]
        private async Task Submit(CancellationToken token)
        {
            using (var worker = ScheduleWork(token))
            {
                try
                {
                    await SaveAssessmentAsync(worker.Token);
                    var feedBack = string.Empty;
                    foreach (var question in Questions)
                    {
                        feedBack += $"######################### Frage #########################{Environment.NewLine}";
                        if (question.HasQuestion)
                        {
                            feedBack += $"Frage: {question.Question}{Environment.NewLine}";
                        }

                        var results = question.ExportResultes();
                        var answers = question.Answers.Select(a => a.Data).OfType<AnswerEntry>();
                        foreach (var answer in answers)
                        {
                            if (answer.HasText)
                            {
                                feedBack += $"Frage: {answer.Text}{Environment.NewLine}";
                            }

                            feedBack += $"Antwort: {results.FirstOrDefault(r => r.AnswerItemId == answer.Id)?.Value ?? "keine"}{Environment.NewLine}";
                        }

                        feedBack += $"--------------------------------------------------------{Environment.NewLine}";
                    }

                    await FeedbackService.SendFeedbackAsync(feedBack, worker.Token);
                    await NavigationService.PushToRootAsnc(worker.Token);
                }
                catch (OperationCanceledException)
                {
                    Logger?.LogDebug($"Submit {nameof(Cancel)} in {GetType().Name}.");
                }
                catch (ObjectDisposedException)
                {
                    Logger?.LogDebug($"Submit {nameof(Cancel)} in {GetType().Name}.");
                }
                catch (Exception ex)
                {
                    Logger?.LogError(ex, $"Unknown error in {nameof(Cancel)} in {GetType().Name}.");
                }
                finally
                {
                    UnscheduleWork(worker);
                }
            }
        }

        private bool CanSubmit()
        {
            return !IsBusy;
        }
    }
}
