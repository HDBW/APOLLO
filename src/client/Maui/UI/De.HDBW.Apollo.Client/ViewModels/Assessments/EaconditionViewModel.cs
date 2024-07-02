// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Collections.ObjectModel;
using System.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel;
using De.HDBW.Apollo.Client.Contracts;
using De.HDBW.Apollo.Client.Enums;
using De.HDBW.Apollo.Client.Models;
using De.HDBW.Apollo.Client.Models.Assessment;
using De.HDBW.Apollo.SharedContracts;
using De.HDBW.Apollo.SharedContracts.Helper;
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
            ILocalAssessmentSessionRepository sessionRepository,
            IRawDataCacheRepository repository,
            IDispatcherService dispatcherService,
            INavigationService navigationService,
            IDialogService dialogService,
            ILogger<EaconditionViewModel> logger)
            : base(service, sessionRepository, repository, dispatcherService, navigationService, dialogService, logger)
        {
        }

        public string SelectedChoicesCount
        {
            get
            {
                return string.Format(this["TxtAssesmentsEaConditionFav"], SelectedChoices.Count);
            }
        }

        private List<string> FilterIds { get; set; } = new List<string>();

        private DrillDownMode DrillDownMode { get; set; }

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
                        string.IsNullOrWhiteSpace(Session.ModuleId))
                    {
                        Logger.LogError($"Session not valid in {OnNavigatedToAsync} in {GetType().Name}.");
                        return;
                    }

                    var offset = 0;
                    RawDataIds = Session.RawDataOrder.Split(";").ToList();
                    var count = RawDataIds.Count;
                    if (DrillDownMode == DrillDownMode.Detail)
                    {
                        var rawdataId = Session.CurrentRawDataId;
                        var cachedData = await RawDataCacheRepository.GetItemAsync(SessionId, rawdataId, worker.Token).ConfigureAwait(false);
                        var question = CreateEntry(QuestionFactory.Create<Eacondition>(cachedData.ToRawData()!, cachedData.RawDataId, cachedData.ModuleId, cachedData.AssesmentId, Culture));
                        await ExecuteOnUIThreadAsync(() => LoadonUIThread(new List<SelectableEaconditionEntry>(), question, offset, count), worker.Token);
                        return;
                    }

                    Session.CurrentRawDataId = null;
                    var cachedDatas = await RawDataCacheRepository.GetItemsAsync(SessionId, RawDataIds, worker.Token).ConfigureAwait(false);
                    if (cachedDatas == null)
                    {
                        Logger.LogError($"No cached rawdata found in {OnNavigatedToAsync} in {GetType().Name}.");
                        return;
                    }

                    var filteredItems = FilterIds.Count() == 0
                        ? cachedDatas.Where(x => x.Data != null && x.Data.Contains($"{nameof(Reliants.reliant_0)}")).ToList()
                        : cachedDatas.Where(x => x.RawDataId != null && FilterIds.Contains(x.RawDataId)).ToList();

                    var questions = new List<SelectableEaconditionEntry>();
                    foreach (var filteredItem in filteredItems)
                    {
                        questions.Add(CreateSelectableEntry(QuestionFactory.Create<Eacondition>(filteredItem.ToRawData()!, filteredItem.RawDataId, filteredItem.ModuleId, filteredItem.AssesmentId, Culture)));
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

        protected async override Task Navigate(CancellationToken cancellationToken)
        {
            using (var worker = ScheduleWork())
            {
                try
                {
                    if (DrillDownMode == DrillDownMode.Detail)
                    {
                        await Shell.Current.GoToAsync(new ShellNavigationState(".."), true);
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

                        var filterIds = SelectedChoices.SelectMany(x => x.Links.Select(l => l.id));
                        parameter.Add(NavigationParameter.Filter, string.Join(";", filterIds));
                        var route = string.Empty;
                        switch (DrillDownMode)
                        {
                            case DrillDownMode.Unknown:
                                route = Routes.EaconditionFilteredView;
                                parameter.Add(NavigationParameter.Type, DrillDownMode.Filtered.ToString());
                                break;
                            case DrillDownMode.Filtered:
                                route = Routes.EaconditionDetailView;
                                parameter.Add(NavigationParameter.Type, DrillDownMode.Detail.ToString());
                                var item = SelectedChoices[0];
                                if (item != null)
                                {
                                    Session.CurrentRawDataId = item.Export().RawDataId;
                                    await SessionRepository.UpdateItemAsync(Session, worker.Token).ConfigureAwait(false);
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
            if (DrillDownMode == DrillDownMode.Filtered)
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
    }
}
