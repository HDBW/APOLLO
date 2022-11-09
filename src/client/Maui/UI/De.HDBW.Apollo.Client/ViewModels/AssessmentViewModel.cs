// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using De.HDBW.Apollo.Client.Contracts;
using De.HDBW.Apollo.Client.Models;
using De.HDBW.Apollo.Client.Models.Assessment;
using De.HDBW.Apollo.SharedContracts.Repositories;
using Invite.Apollo.App.Graph.Common.Models;
using Invite.Apollo.App.Graph.Common.Models.Assessment;
using Invite.Apollo.App.Graph.Common.Models.Assessment.Enums;
using Invite.Apollo.App.Graph.Common.Models.UserProfile;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.Controls;

namespace De.HDBW.Apollo.Client.ViewModels
{
    public partial class AssessmentViewModel : BaseViewModel
    {
        [ObservableProperty]
        private ObservableCollection<QuestionEntry> _questions = new ObservableCollection<QuestionEntry>();

        [ObservableProperty]
        private ObservableCollection<LayoutType> _questionLayouts = new ObservableCollection<LayoutType>() { LayoutType.Default, LayoutType.Overlay, LayoutType.Compare };

        [ObservableProperty]
        private ObservableCollection<LayoutType> _answerLayouts = new ObservableCollection<LayoutType>() { LayoutType.Default, LayoutType.HorizontalList, LayoutType.UniformGrid };

        [ObservableProperty]
        private ObservableCollection<InteractionType> _interactions = new ObservableCollection<InteractionType>() { InteractionType.SingleSelect, InteractionType.MultiSelect, InteractionType.Input, InteractionType.Associate };

        [ObservableProperty]
        private QuestionEntry? _currentQuestion;
        private LayoutType? _answerLayout;
        private LayoutType? _questionLayout;
        private InteractionType? _interaction;

        private long? _assessmentItemId;

        public AssessmentViewModel(
            IAssessmentItemRepository assessmentItemRepository,
            IQuestionItemRepository questionItemRepository,
            IAnswerItemResultRepository answerItemResultRepository,
            IAnswerItemRepository answerItemRepository,
            IMetaDataMetaDataRelationRepository metaDataMetaDataRelationRepository,
            IAnswerMetaDataRelationRepository answerMetaDataRelationRepository,
            IQuestionMetaDataRelationRepository questionMetaDataRelationRepository,
            IMetaDataRepository metaDataRepository,
            IDispatcherService dispatcherService,
            INavigationService navigationService,
            IDialogService dialogService,
            ILogger<AssessmentViewModel> logger)
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
            AssessmentItemRepository = assessmentItemRepository;
            QuestionItemRepository = questionItemRepository;
            AnswerItemResultRepository = answerItemResultRepository;
            AnswerItemRepository = answerItemRepository;
            MetaDataMetaDataRelationRepository = metaDataMetaDataRelationRepository;
            AnswerMetaDataRelationRepository = answerMetaDataRelationRepository;
            QuestionMetaDataRelationRepository = questionMetaDataRelationRepository;
            MetaDataRepository = metaDataRepository;
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

        private IAssessmentItemRepository AssessmentItemRepository { get; }

        private IQuestionItemRepository QuestionItemRepository { get; }

        private IAnswerItemResultRepository AnswerItemResultRepository { get; }

        private IAnswerItemRepository AnswerItemRepository { get; }

        private IMetaDataMetaDataRelationRepository MetaDataMetaDataRelationRepository { get; }

        private IAnswerMetaDataRelationRepository AnswerMetaDataRelationRepository { get; }

        private IQuestionMetaDataRelationRepository QuestionMetaDataRelationRepository { get; }

        private IMetaDataRepository MetaDataRepository { get; }

        [RelayCommand]
        public void NextQuestion()
        {
            var currentIndex = CurrentQuestion != null ? Questions.IndexOf(CurrentQuestion) : 0;
            currentIndex = currentIndex + 1 >= Questions.Count ? 0 : currentIndex + 1;
            CurrentQuestion = Questions[currentIndex];
            _questionLayout = CurrentQuestion?.QuestionLayout ?? LayoutType.Default;
            _answerLayout = CurrentQuestion?.AnswerLayout ?? LayoutType.Default;
            _interaction = CurrentQuestion?.Interaction ?? InteractionType.Unknown;
            OnPropertyChanged(nameof(QuestionLayout));
            OnPropertyChanged(nameof(AnswerLayout));
            OnPropertyChanged(nameof(Interaction));
            OnPropertyChanged(nameof(Progress));
        }

        public async override Task OnNavigatedToAsync()
        {
            if (!_assessmentItemId.HasValue)
            {
                return;
            }

            using (var worker = ScheduleWork())
            {
                try
                {
                    var assessmentItem = await AssessmentItemRepository.GetItemByIdAsync(_assessmentItemId.Value, worker.Token).ConfigureAwait(false);
                    var questionItems = await QuestionItemRepository.GetItemsByForeignKeyAsync(_assessmentItemId.Value, worker.Token).ConfigureAwait(false);
                    questionItems = questionItems ?? new List<QuestionItem>();
                    var questionIds = questionItems.Select(q => q.Id);
                    var relatedMetaDataIds = new List<long>();
                    var questionMetaDataIds = new List<long>();
                    var answerMetaDataIds = new List<long>();

                    var answerItems = await AnswerItemRepository.GetItemsByForeignKeysAsync(questionIds, worker.Token).ConfigureAwait(false);
                    var answerIds = answerItems.Select(q => q.Id);

                    var answerItemResults = await AnswerItemResultRepository.GetItemsByForeignKeyAsync(_assessmentItemId.Value, worker.Token).ConfigureAwait(false);

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
                    var questionAnswerItemMetaDataMetaDatasMapping = new Dictionary<QuestionItem, Dictionary<MetaDataItem, IEnumerable<MetaDataItem>>>();

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
                        metaDataMetaDataMappings = new Dictionary<MetaDataItem, IEnumerable<MetaDataItem>>();
                        foreach (var answerItem in questionAnswers)
                        {
                            var answerRelationIds = answerMetaDataRelations.Where(r => r.AnswerId == answerItem.Id).Select(r => r.MetaDataId).ToList();
                            var anserItemMetaDatas = relatedMetaDatas.Where(m => answerRelationIds.Contains(m.Id)).ToList();
                            answerAnswerMetaDatasMapping.Add(answerItem, anserItemMetaDatas);
                            var anserItemMetaDataMetaDatasMappings = metaDataMetaDataRelationRelations.Where(m => relationIds.Contains(m.SourceId)).ToList();
                            foreach (var mapping in anserItemMetaDataMetaDatasMappings.GroupBy(m => m.SourceId))
                            {
                                var targetIds = mapping.Select(m => m.TargetId);
                                metaDataMetaDataMappings.Add(anserItemMetaDatas.First(m => m.Id == mapping.Key), relatedMetaDatas.Where(m => targetIds.Contains(m.Id)).ToList());
                            }
                        }

                        questionAnswerItemMetaDataMetaDatasMapping.Add(questionItem, metaDataMetaDataMappings);
                    }

                    await ExecuteOnUIThreadAsync(
                        () => LoadonUIThread(
                        questionItems,
                        questionQuestionMetaDatasMapping,
                        questionQuestionMetaDataMetaDatasMapping,
                        questionAnswerItemsMapping,
                        questionAnswerResultsMapping,
                        questionAnswerItemMetaDatasMapping,
                        questionAnswerItemMetaDataMetaDatasMapping), worker.Token);
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
            _assessmentItemId = navigationParameters.GetValue<long?>(NavigationParameter.Id);
        }

        private void LoadonUIThread(
            IEnumerable<QuestionItem> questionItems,
            Dictionary<QuestionItem, IEnumerable<MetaDataItem>> questionMetaDatasMapping,
            Dictionary<QuestionItem, Dictionary<MetaDataItem, IEnumerable<MetaDataItem>>> questionMetaDataMetaDatasMappings,
            Dictionary<QuestionItem, IEnumerable<AnswerItem>> answerItemsMapping,
            Dictionary<QuestionItem, IEnumerable<AnswerItemResult>> answerResultsMapping,
            Dictionary<QuestionItem, Dictionary<AnswerItem, IEnumerable<MetaDataItem>>> answerItemMetaDatasMapping,
            Dictionary<QuestionItem, Dictionary<MetaDataItem, IEnumerable<MetaDataItem>>> answerItemMetaDataMetaDatasMapping)
        {
            Questions = new ObservableCollection<QuestionEntry>(questionItems.Select(q => QuestionEntry.Import(q, questionMetaDatasMapping[q], questionMetaDataMetaDatasMappings[q], answerItemsMapping[q], answerResultsMapping[q], answerItemMetaDatasMapping[q], answerItemMetaDataMetaDatasMapping[q], Logger)));
            CurrentQuestion = Questions?.FirstOrDefault();
            _questionLayout = CurrentQuestion?.QuestionLayout ?? LayoutType.Default;
            _answerLayout = CurrentQuestion?.AnswerLayout ?? LayoutType.Default;
            OnPropertyChanged(nameof(QuestionLayout));
            OnPropertyChanged(nameof(AnswerLayout));
            OnPropertyChanged(nameof(Interaction));
            OnPropertyChanged(nameof(Progress));
        }
    }
}
