// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Collections.ObjectModel;
using System.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using De.HDBW.Apollo.Client.Contracts;
using De.HDBW.Apollo.Client.Enums;
using De.HDBW.Apollo.Client.Models;
using De.HDBW.Apollo.Client.Models.Assessment;
using De.HDBW.Apollo.SharedContracts;
using De.HDBW.Apollo.SharedContracts.Helper;
using De.HDBW.Apollo.SharedContracts.Models;
using De.HDBW.Apollo.SharedContracts.Questions;
using De.HDBW.Apollo.SharedContracts.Repositories;
using De.HDBW.Apollo.SharedContracts.Services;
using Microsoft.Extensions.Logging;

namespace De.HDBW.Apollo.Client.ViewModels.Assessments
{
    public partial class EaconditionViewModel : AbstractQuestionViewModel<Eacondition, EaconditionEntry>
    {
        [ObservableProperty]
        private ObservableCollection<SelectableEaconditionEntry> _currentChoices = new ObservableCollection<SelectableEaconditionEntry>();

        [ObservableProperty]
        private ObservableCollection<SelectableEaconditionEntry> _selectedChoices = new ObservableCollection<SelectableEaconditionEntry>();

        public EaconditionViewModel(
            IAssessmentService service,
            IRawDataCacheRepository repository,
            IUserSecretsService userSecretsService,
            IAudioPlayerService audioPlayerService,
            IDispatcherService dispatcherService,
            INavigationService navigationService,
            IDialogService dialogService,
            ILogger<EaconditionViewModel> logger)
            : base(service, repository, userSecretsService, audioPlayerService, dispatcherService, navigationService, dialogService, logger)
        {
        }

        public string SelectedChoicesCount
        {
            get
            {
                return string.Format(this["TxtAssesmentsEaConditionViewFav"], SelectedChoices.Count);
            }
        }

        private List<string> FilterIds { get; set; } = new List<string>();

        private DrillDownMode DrillDownMode { get; set; }

        public override async Task OnNavigatedToAsync()
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

                    Session = await Service.GetSessionAsync(SessionId, Language, worker.Token).ConfigureAwait(false);
                    if (Session == null)
                    {
                        Logger.LogError($"Session not found in {OnNavigatedToAsync} in {GetType().Name}.");
                        return;
                    }

                    if (string.IsNullOrWhiteSpace(Session.RawDataOrder) ||
                        string.IsNullOrWhiteSpace(Session.ModuleId))
                    {
                        Logger.LogError($"Session not valid in {OnNavigatedToAsync} in {GetType().Name}.");
                        return;
                    }

                    switch (DrillDownMode)
                    {
                        case DrillDownMode.Unknown:
                        case DrillDownMode.Filtered:
                            if (!string.IsNullOrWhiteSpace(Session.CurrentRawDataId))
                            {
                                Session.CurrentRawDataId = null;
                                if (!await Service.UpdateSessionAsync(Session, worker.Token).ConfigureAwait(false))
                                {
                                    Logger.LogError($"Unabele to update session while {nameof(OnNavigatedToAsync)} in {GetType().Name}.");
                                    return;
                                }
                            }

                            break;
                    }

                    if (CurrentChoices.Any())
                    {
                        return;
                    }

                    var offset = 0;
                    RawDataIds = Session.RawDataOrder.Split(";").ToList();
                    var count = RawDataIds.Count;
                    if (DrillDownMode == DrillDownMode.Detail)
                    {
                        var rawdataId = Session.CurrentRawDataId;
                        if (string.IsNullOrWhiteSpace(rawdataId))
                        {
                            Logger.LogError($"Session not valid in DrillDownMode.Detail while {nameof(OnNavigatedToAsync)} in {GetType().Name}.");
                            return;
                        }

                        var cachedData = await RawDataCacheRepository.GetItemAsync(SessionId, rawdataId, worker.Token).ConfigureAwait(false);
                        var rawData = cachedData?.ToRawData();
                        if (cachedData == null ||
                            rawData == null ||
                            string.IsNullOrWhiteSpace(cachedData.RawDataId) ||
                            string.IsNullOrWhiteSpace(cachedData.ModuleId) ||
                            string.IsNullOrWhiteSpace(cachedData.AssesmentId))
                        {
                            Logger.LogError($"Rawdate invalid in DrillDownMode.Detail while {nameof(OnNavigatedToAsync)} in {GetType().Name}.");
                            return;
                        }

                        var question = CreateEntry(QuestionFactory.Create<Eacondition>(rawData, cachedData.RawDataId, cachedData.ModuleId, cachedData.AssesmentId, Culture));
                        await ExecuteOnUIThreadAsync(() => LoadonUIThread(new List<SelectableEaconditionEntry>(), question, offset, count), worker.Token);
                        return;
                    }

                    IEnumerable<CachedRawData>? cachedDatas = null;
                    switch (FilterIds.Count())
                    {
                        case 0:
                            cachedDatas = await RawDataCacheRepository.GetItemsAsync(SessionId, FilterIds.Count() == 0 ? RawDataIds : FilterIds, worker.Token).ConfigureAwait(false);
                            cachedDatas = cachedDatas?.Where(x => x.Data != null && x.Data.Contains($"{nameof(Reliants.reliant_0)}"));
                            break;
                        default:
                            cachedDatas = await RawDataCacheRepository.GetItemsAsync(SessionId, FilterIds, worker.Token).ConfigureAwait(false);
                            break;
                    }

                    if (!(cachedDatas?.Any() ?? false))
                    {
                        Logger.LogError($"No cached rawdata found while {nameof(OnNavigatedToAsync)} in {GetType().Name}.");
                        return;
                    }

                    var questions = new List<SelectableEaconditionEntry>();
                    foreach (var filteredItem in cachedDatas)
                    {
                        var rawData = filteredItem.ToRawData();
                        if (rawData == null ||
                            string.IsNullOrWhiteSpace(filteredItem.RawDataId) ||
                            string.IsNullOrWhiteSpace(filteredItem.ModuleId) ||
                            string.IsNullOrWhiteSpace(filteredItem.AssesmentId))
                        {
                            Logger.LogWarning($"Found rawdate with invalid ids while {nameof(OnNavigatedToAsync)} in {GetType().Name}.");
                            continue;
                        }

                        questions.Add(CreateSelectableEntry(QuestionFactory.Create<Eacondition>(rawData, filteredItem.RawDataId, filteredItem.ModuleId, filteredItem.AssesmentId, Culture)));
                    }

                    await ExecuteOnUIThreadAsync(() => LoadonUIThread(questions, null, offset, count), worker.Token);
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

        protected override EaconditionEntry CreateEntry(Eacondition data)
        {
            return EaconditionEntry.Import(data, MediaBasePath, Density, ImageSizeConfig[typeof(EaconditionEntry)]);
        }

        protected override void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);
            if (e.PropertyName == string.Empty)
            {
                OnPropertyChanged(nameof(SelectedChoicesCount));
            }
        }

        protected override void OnPrepare(NavigationParameters navigationParameters)
        {
            base.OnPrepare(navigationParameters);
            FilterIds = navigationParameters.GetValue<string?>(NavigationParameter.Filter)?.ToString()?.Split(";").ToList() ?? new List<string>();
            DrillDownMode = Enum.Parse<DrillDownMode>(navigationParameters.GetValue<string?>(NavigationParameter.Type)?.ToString() ?? nameof(DrillDownMode.Unknown));
        }

        protected override async Task Navigate(CancellationToken cancellationToken)
        {
            Logger.LogInformation($"Invoked {nameof(NavigateCommand)} in {GetType().Name}.");
            using (var worker = ScheduleWork(cancellationToken))
            {
                try
                {
                    if (DrillDownMode == DrillDownMode.Detail)
                    {
                        await NavigationService.PopAsync(worker.Token).ConfigureAwait(false);
                    }
                    else
                    {
                        if (string.IsNullOrWhiteSpace(ModuleId) ||
                           string.IsNullOrWhiteSpace(SessionId) ||
                           string.IsNullOrWhiteSpace(Language) ||
                           Session == null)
                        {
                            return;
                        }

                        var parameter = new NavigationParameters()
                    {
                        { NavigationParameter.Id, ModuleId },
                        { NavigationParameter.Data, SessionId },
                        { NavigationParameter.Language, Language },
                    };

                        var filterIds = SelectedChoices.SelectMany(x => x.Links.Select(l => l.id)).ToList();
                        if (!filterIds.Any() && DrillDownMode == DrillDownMode.Unknown)
                        {
                            Logger?.LogError($"Selection did not contain any link while {nameof(OnNavigatedToAsync)} in {GetType().Name}.");
                            return;
                        }

                        parameter.Add(NavigationParameter.Filter, string.Join(";", filterIds));
                        var route = string.Empty;
                        switch (DrillDownMode)
                        {
                            case DrillDownMode.Unknown:
                                route = Routes.EaconditionFilteredView;
                                parameter.Add(NavigationParameter.Type, DrillDownMode.Filtered.ToString());
                                break;
                            case DrillDownMode.Filtered:
                                foreach (var choice in CurrentChoices)
                                {
                                    choice.IsSelected = false;
                                }

                                route = Routes.EaconditionDetailView;
                                parameter.Add(NavigationParameter.Type, DrillDownMode.Detail.ToString());
                                var item = SelectedChoices[0];
                                if (item != null)
                                {
                                    Session.CurrentRawDataId = item.Export().RawDataId;
                                    await Service.UpdateSessionAsync(Session, worker.Token).ConfigureAwait(false);
                                }

                                break;
                        }

                        await NavigationService.NavigateAsync(route, worker.Token, parameter);
                    }
                }
                catch (OperationCanceledException)
                {
                    Logger?.LogDebug($"Canceled {nameof(Navigate)} in {GetType().Name}.");
                }
                catch (ObjectDisposedException)
                {
                    Logger?.LogDebug($"Canceled {nameof(Navigate)} in {GetType().Name}.");
                }
                catch (Exception ex)
                {
                    Logger?.LogError(ex, $"Unknown error while {nameof(Navigate)} in {GetType().Name}.");
                }
                finally
                {
                    UnscheduleWork(worker);
                }
            }
        }

        private SelectableEaconditionEntry CreateSelectableEntry(Eacondition data)
        {
            return SelectableEaconditionEntry.Import(data, SelectionChangedHandler, MediaBasePath, Density, ImageSizeConfig[typeof(EaconditionEntry)]);
        }

        private void SelectionChangedHandler(SelectableEaconditionEntry entry)
        {
            if (entry.IsSelected)
            {
                SelectedChoices.Add(entry);
                if (SelectedChoices.Count > 3)
                {
                    var item = SelectedChoices[0];
                    item.IsSelected = false;
                    SelectedChoices.Remove(item);
                }
            }
            else
            {
                SelectedChoices.Remove(entry);
            }

            OnPropertyChanged(nameof(SelectedChoicesCount));
            if (DrillDownMode == DrillDownMode.Filtered && SelectedChoices.Count > 0)
            {
                NavigateCommand.Execute(null);
            }
        }

        private void LoadonUIThread(List<SelectableEaconditionEntry> questions, EaconditionEntry? question, int offset, int count)
        {
            Count = count;
            Offset = offset;
            Question = question;
            CurrentChoices = new ObservableCollection<SelectableEaconditionEntry>(questions);
        }

        [RelayCommand(AllowConcurrentExecutions = false, CanExecute = nameof(CanNavigateBack))]
        private async Task NavigateBack(CancellationToken token)
        {
            Logger.LogInformation($"Invoked {nameof(NavigateBackCommand)} in {GetType().Name}.");
            using (var worker = ScheduleWork(token))
            {
                try
                {
                    await Service.CancelSessionAsync(SessionId!, worker.Token);
                    await NavigationService.PopAsync(worker.Token).ConfigureAwait(false);
                }
                catch (OperationCanceledException)
                {
                    Logger?.LogDebug($"Canceled {nameof(NavigateBack)} in {GetType().Name}.");
                }
                catch (ObjectDisposedException)
                {
                    Logger?.LogDebug($"Canceled {nameof(NavigateBack)} in {GetType().Name}.");
                }
                catch (Exception ex)
                {
                    Logger?.LogError(ex, $"Unknown error in {nameof(NavigateBack)} in {GetType().Name}.");
                }
                finally
                {
                    UnscheduleWork(worker);
                }
            }
        }

        private bool CanNavigateBack()
        {
            return !IsBusy && SessionId != null;
        }
    }
}
