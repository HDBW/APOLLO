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
using Microsoft.Extensions.Logging;

namespace De.HDBW.Apollo.Client.ViewModels
{
    public partial class AssessmentViewModel : BaseViewModel
    {
        [ObservableProperty]
        private ObservableCollection<QuestionEntry> _questions = new ObservableCollection<QuestionEntry>();

        [ObservableProperty]
        private QuestionEntry? _currentQuestion;
        private long? _assessmentItemId;

        public AssessmentViewModel(
            IAssessmentItemRepository assessmentItemRepository,
            IQuestionItemRepository questiontItemRepository,
            IAnswerItemRepository answerItemRepository,
            IMetaDataMetaDataRelationRepository metaDataMetaDataRelationRepository,
            IAnswerMetaDataRelationRepository answerMetaDataRelationRepository,
            IQuestionMetaDataRelationRepository questionMetaDataRelationRepository,
            IMetaDataRepository metadataRepository,
            IDispatcherService dispatcherService,
            INavigationService navigationService,
            IDialogService dialogService,
            ILogger<AssessmentViewModel> logger)
            : base(dispatcherService, navigationService, dialogService, logger)
        {
            AssessmentItemRepository = assessmentItemRepository;
            QuestiontItemRepository = questiontItemRepository;
            AnswerItemRepository = answerItemRepository;
            MetaDataMetaDataRelationRepository = metaDataMetaDataRelationRepository;
            AnswerMetaDataRelationRepository = answerMetaDataRelationRepository;
            QuestionMetaDataRelationRepository = questionMetaDataRelationRepository;
            MetadataRepository = metadataRepository;
        }

        private IAssessmentItemRepository AssessmentItemRepository { get; }

        private IQuestionItemRepository QuestiontItemRepository { get; }

        private IAnswerItemRepository AnswerItemRepository { get; }

        private IMetaDataMetaDataRelationRepository MetaDataMetaDataRelationRepository { get; }

        private IAnswerMetaDataRelationRepository AnswerMetaDataRelationRepository { get; }

        private IQuestionMetaDataRelationRepository QuestionMetaDataRelationRepository { get; }

        private IMetaDataRepository MetadataRepository { get; }

        [RelayCommand]
        public void NextQuestion()
        {
            var currentIndex = CurrentQuestion != null ? Questions.IndexOf(CurrentQuestion) : 0;
            currentIndex = currentIndex + 1 >= Questions.Count ? 0 : currentIndex + 1;
            CurrentQuestion = Questions[currentIndex];
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
                    var questionItems = await QuestiontItemRepository.GetItemsByForeignKeyAsync(_assessmentItemId.Value, worker.Token).ConfigureAwait(false);
                    questionItems = questionItems ?? new List<QuestionItem>();
                    var questionIds = questionItems.Select(q => q.Id);
                    var relatedMetaDataIds = new List<long>();
                    var questionMetaDataIds = new List<long>();
                    var answerMetaDataIds = new List<long>();

                    var answerItems = await AnswerItemRepository.GetItemsByForeignKeysAsync(questionIds, worker.Token).ConfigureAwait(false);
                    var answerIds = answerItems.Select(q => q.Id);

                    var questionMetaDataRelations = await QuestionMetaDataRelationRepository.GetItemsByForeignKeysAsync(questionIds, worker.Token).ConfigureAwait(false);
                    questionMetaDataRelations = questionMetaDataRelations ?? new List<QuestionMetaDataRelation>();
                    questionMetaDataIds.AddRange(questionMetaDataRelations.Select(r => r.MetaDataId).Distinct());
                    relatedMetaDataIds.AddRange(questionMetaDataIds);

                    var answerMetaDataRelations = await AnswerMetaDataRelationRepository.GetItemsByIdsAsync(questionIds, worker.Token).ConfigureAwait(false);
                    answerMetaDataRelations = answerMetaDataRelations ?? new List<AnswerMetaDataRelation>();
                    answerMetaDataIds.AddRange(answerMetaDataRelations.Select(r => r.MetaDataId).Distinct());
                    relatedMetaDataIds.AddRange(answerMetaDataIds);

                    relatedMetaDataIds = relatedMetaDataIds.Distinct().ToList();
                    var metaDataMetaDataRelationRelations = await MetaDataMetaDataRelationRepository.GetItemsByIdsAsync(relatedMetaDataIds, worker.Token).ConfigureAwait(false);
                    metaDataMetaDataRelationRelations = metaDataMetaDataRelationRelations ?? new List<MetaDataMetaDataRelation>();

                    relatedMetaDataIds.AddRange(metaDataMetaDataRelationRelations.Select(r => r.TargetId));
                    relatedMetaDataIds = relatedMetaDataIds.Distinct().ToList();

                    var relatedMetas = await MetadataRepository.GetItemsByIdsAsync(relatedMetaDataIds, worker.Token).ConfigureAwait(false);
                    relatedMetas = relatedMetas ?? new List<MetaDataItem>();

                    var questionQuestionMetaDatasMapping = new Dictionary<QuestionItem, IEnumerable<MetaDataItem>>();
                    var questionAnswersMapping = new Dictionary<QuestionItem, IEnumerable<AnswerItem>>();
                    var questionAnswerAnswerMetaDatasMapping = new Dictionary<QuestionItem, Dictionary<AnswerItem, IEnumerable<MetaDataItem>>>();

                    foreach (var questionItem in questionItems)
                    {
                        var relationIds = questionMetaDataRelations.Where(r => r.QuestionId == questionItem.Id).Select(r => r.MetaDataId);
                        questionQuestionMetaDatasMapping.Add(questionItem, relatedMetas.Where(m => relationIds.Contains(m.Id)).ToList());

                        var questionAnswers = answerItems.Where(a => a.QuestionId == questionItem.Id).ToList();
                        questionAnswersMapping.Add(questionItem, questionAnswers);

                        var answerAnswerMetaDatasMapping = new Dictionary<AnswerItem, IEnumerable<MetaDataItem>>();
                        questionAnswerAnswerMetaDatasMapping.Add(questionItem, answerAnswerMetaDatasMapping);
                        foreach (var answerItem in questionAnswers)
                        {
                            var answerRelationIds = answerMetaDataRelations.Where(r => r.AnswerId == answerItem.Id).Select(r => r.MetaDataId);
                            answerAnswerMetaDatasMapping.Add(answerItem, relatedMetas.Where(m => answerRelationIds.Contains(m.Id)).ToList());
                        }
                    }

                    await ExecuteOnUIThreadAsync(
                        () => LoadonUIThread(
                        questionItems,
                        questionQuestionMetaDatasMapping,
                        questionAnswersMapping,
                        questionAnswerAnswerMetaDatasMapping), worker.Token);
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
            IEnumerable<QuestionItem> questions,
            Dictionary<QuestionItem, IEnumerable<MetaDataItem>> questionMetaData,
            Dictionary<QuestionItem, IEnumerable<AnswerItem>> answers,
            Dictionary<QuestionItem, Dictionary<AnswerItem, IEnumerable<MetaDataItem>>> answerMetaData)
        {
            Questions = new ObservableCollection<QuestionEntry>(questions.Select(q => QuestionEntry.Import(q, questionMetaData[q], answers[q], answerMetaData[q])));
            CurrentQuestion = Questions?.FirstOrDefault();
        }
    }
}
