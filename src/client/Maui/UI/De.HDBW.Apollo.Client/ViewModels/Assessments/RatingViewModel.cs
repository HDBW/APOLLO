// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using De.HDBW.Apollo.Client.Contracts;
using De.HDBW.Apollo.Client.Dialogs;
using De.HDBW.Apollo.Client.Helper;
using De.HDBW.Apollo.Client.Models;
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
            IRawDataCacheRepository repository,
            IUserSecretsService userSecretsService,
            IAudioPlayerService audioPlayerService,
            IDispatcherService dispatcherService,
            INavigationService navigationService,
            IDialogService dialogService,
            ILogger<RatingViewModel> logger)
            : base(service, repository, userSecretsService, audioPlayerService, dispatcherService, navigationService, dialogService, logger)
        {
        }

        public override string? Title
        {
            get
            {
                if (Culture == null)
                {
                    return string.Empty;
                }

                var formate = this["TxtAssesmentsGlobalTitlePage"];
                if (string.IsNullOrWhiteSpace(formate) || Count == 0)
                {
                    return string.Empty;
                }

                return string.Format(formate, Offset + 1, Count);
            }
        }

        public async override Task OnNavigatedToAsync()
        {
            using (var worker = ScheduleWork())
            {
                try
                {
                    if (WasShowingDialog)
                    {
                        return;
                    }

                    if (string.IsNullOrWhiteSpace(SessionId))
                    {
                        Logger.LogError($"Session not present in {OnNavigatedToAsync} in {GetType().Name}.");
                        return;
                    }

                    Session = await Service.GetSessionAsync(SessionId, Language, worker.Token).ConfigureAwait(false);
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
                    var count = (int)Math.Ceiling(((double)RawDataIds.Count) / 2d);
                    var ids = RawDataIds.Skip(offset).Take(2).ToList();
                    var cachedDatas = await RawDataCacheRepository.GetItemsAsync(SessionId, ids, worker.Token).ConfigureAwait(false);
                    if (cachedDatas == null)
                    {
                        Logger.LogError($"No cached rawdata found in {OnNavigatedToAsync} in {GetType().Name}.");
                        return;
                    }

                    offset = (int)Math.Floor((double)RawDataIds.IndexOf(Session.CurrentRawDataId) / 2d);
                    var questions = new List<RatingEntry>();
                    foreach (var data in cachedDatas)
                    {
                        if (string.IsNullOrWhiteSpace(data.RawDataId) || string.IsNullOrWhiteSpace(data.ModuleId) || string.IsNullOrWhiteSpace(data.AssesmentId))
                        {
                            continue;
                        }

                        var rawData = data.ToRawData();
                        questions.Add(CreateEntry(QuestionFactory.Create<Rating>(rawData!, data.RawDataId, data.ModuleId, data.AssesmentId, Culture)));
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

        protected override async Task Navigate(CancellationToken cancellationToken)
        {
            Logger.LogInformation($"Invoked {nameof(NavigateCommand)} in {GetType().Name}.");
            using (var worker = ScheduleWork(cancellationToken))
            {
                try
                {
                    if (Session?.SessionId == null || Session.CurrentRawDataId == null)
                    {
                        return;
                    }

                    var result = MultipleQuestions.Any(x => x.DidInteract);
                    if (!result)
                    {
                        WasShowingDialog = true;
                        var parameters = new NavigationParameters();
                        var resultParameters = await DialogService.ShowPopupAsync<SkipQuestionDialog, NavigationParameters, NavigationParameters>(parameters, worker.Token);
                        result = resultParameters?.GetValue<bool?>(NavigationParameter.Result) ?? false;
                    }

                    if (!result)
                    {
                        return;
                    }

                    var scores = MultipleQuestions.Select(x => x.GetScore());
                    var cachedData = await Service.AnswerAsync(Session.SessionId, Session.CurrentRawDataId, scores, worker.Token).ConfigureAwait(false);
                    if (cachedData == null && Offset != (Count - 1))
                    {
                        throw new NotSupportedException($"Unable to get next question while {nameof(Navigate)} in {GetType().Name}.");
                    }

                    var stack = Shell.Current.Navigation.NavigationStack.ToArray();
                    if (Offset == (Count - 1) && cachedData == null)
                    {
                        if (!await Service.FinishSessionAsync(Session.SessionId, cancellationToken).ConfigureAwait(false))
                        {
                            throw new NotSupportedException($"Unable to cancel session while {nameof(Navigate)} in {GetType().Name}.");
                        }

                        await ExecuteOnUIThreadAsync(
                            async () =>
                            {
                                Shell.Current.Navigation.RemovePage(stack.Last());
                                var parameters = new NavigationParameters();
                                parameters.AddValue(NavigationParameter.Id, ModuleId);
                                parameters.AddValue(NavigationParameter.Language, Language);
                                await NavigationService.NavigateAsync(Routes.ResultOverView, worker.Token, parameters);
                            }, cancellationToken);
                    }
                    else if (cachedData != null)
                    {
                        var rawData = cachedData.ToRawData();
                        string? route = rawData?.type.ToRoute();
                        if (string.IsNullOrWhiteSpace(route))
                        {
                            return;
                        }

                        await ExecuteOnUIThreadAsync(
                        async () =>
                        {
                            Shell.Current.Navigation.RemovePage(stack.Last());
                            var parameters = new NavigationParameters();
                            parameters.AddValue(NavigationParameter.Id, ModuleId);
                            parameters.AddValue(NavigationParameter.Data, SessionId);
                            parameters.AddValue(NavigationParameter.Language, Language);
                            await NavigationService.NavigateAsync(route, worker.Token, parameters);
                        }, worker.Token);
                    }
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
