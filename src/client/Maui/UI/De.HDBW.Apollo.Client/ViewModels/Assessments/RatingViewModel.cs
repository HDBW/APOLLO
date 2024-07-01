// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using De.HDBW.Apollo.Client.Contracts;
using De.HDBW.Apollo.Client.Models.Assessment;
using De.HDBW.Apollo.SharedContracts.Helper;
using De.HDBW.Apollo.SharedContracts.Questions;
using De.HDBW.Apollo.SharedContracts.Repositories;
using De.HDBW.Apollo.SharedContracts.Services;
using Microsoft.Extensions.Logging;

namespace De.HDBW.Apollo.Client.ViewModels.Assessments
{
    public partial class RatingViewModel : AbstractQuestionViewModel<Rating, RatingEntry>
    {
        [ObservableProperty]
        private ObservableCollection<RatingEntry> _multipleQuestions = new ObservableCollection<RatingEntry>();

        public RatingViewModel(
            IAssessmentService service,
            ILocalAssessmentSessionRepository sessionRepository,
            IRawDataCacheRepository repository,
            IDispatcherService dispatcherService,
            INavigationService navigationService,
            IDialogService dialogService,
            ILogger<RatingViewModel> logger)
            : base(service, sessionRepository, repository, dispatcherService, navigationService, dialogService, logger)
        {
        }

        public async override Task OnNavigatedToAsync()
        {
            using (var worker = ScheduleWork())
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(SessionId))
                    {
                        Logger.LogError($"Session not present in {OnNavigatedToAsync} in {GetType().Name}.");
                        return;
                    }

                    Session = await SessionRepository.GetItemBySessionIdAsync(SessionId, worker.Token).ConfigureAwait(false);
                    if (Session == null)
                    {
                        Logger.LogError($"Session not found in {OnNavigatedToAsync} in {GetType().Name}.");
                        return;
                    }

                    if (string.IsNullOrWhiteSpace(Session.RawDataOrder) ||
                        string.IsNullOrWhiteSpace(Session.CurrentRawDataId) ||
                        string.IsNullOrWhiteSpace(Session.ModuleId))
                    {
                        Logger.LogError($"Session not valid in {OnNavigatedToAsync} in {GetType().Name}.");
                        return;
                    }

                    RawDataIds = Session.RawDataOrder.Split(";").ToList();
                    var offset = RawDataIds.IndexOf(Session.CurrentRawDataId);
                    var count = RawDataIds.Count;
                    var ids = RawDataIds.Skip(offset).Take(2).ToList();
                    var cachedDatas = await RawDataCacheRepository.GetItemsAsync(SessionId, ids, worker.Token).ConfigureAwait(false);
                    if (cachedDatas == null)
                    {
                        Logger.LogError($"No cached rawdata found in {OnNavigatedToAsync} in {GetType().Name}.");
                        return;
                    }

                    if (cachedDatas.Count() > 1)
                    {
                        offset = offset + (cachedDatas.Count() - 1);
                    }

                    var questions = new List<RatingEntry>();
                    foreach (var data in cachedDatas)
                    {
                        var rawData = data.ToRawData();
                        questions.Add(CreateEntry(QuestionFactory.Create<Rating>(rawData!, Culture)));
                    }

                    await ExecuteOnUIThreadAsync(() => LoadonUIThread(questions, offset, count), worker.Token);
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

        protected override RatingEntry CreateEntry(Rating data)
        {
            return RatingEntry.Import(data);
        }

        private void LoadonUIThread(List<RatingEntry> questions, int offset, int count)
        {
            Offset = offset;
            Count = count;
            MultipleQuestions = new ObservableCollection<RatingEntry>(questions);
            Question = MultipleQuestions.FirstOrDefault();
            Questions = new ObservableCollection<Rating>();
        }
    }
}
