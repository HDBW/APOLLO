// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Collections.ObjectModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using De.HDBW.Apollo.Client.Contracts;
using De.HDBW.Apollo.Client.Helper;
using De.HDBW.Apollo.Client.Messages;
using De.HDBW.Apollo.Client.Models;
using De.HDBW.Apollo.Client.Models.Assessment;
using De.HDBW.Apollo.SharedContracts.Helper;
using De.HDBW.Apollo.SharedContracts.Models;
using De.HDBW.Apollo.SharedContracts.Repositories;
using De.HDBW.Apollo.SharedContracts.Services;
using Invite.Apollo.App.Graph.Common.Models.Assessments;
using Microsoft.Extensions.Logging;

namespace De.HDBW.Apollo.Client.ViewModels.Assessments
{
    public partial class ModuleDetailViewModel : BaseViewModel
    {
        [ObservableProperty]
        private ObservableCollection<ObservableObject> _sections = new ObservableCollection<ObservableObject>();

        [ObservableProperty]
        private string? _title;

        [ObservableProperty]
        private bool _hasLanguageSelection;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(StartAssessmentCommand))]
        private bool _canStartTest;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(FlowDirection))]
        private CultureInfo _culture = new CultureInfo("de-DE");

        private string? _moduleId;

        private string? _assessmentId;

        private AssessmentType _assessmentType;

        private string? _language;

        public ModuleDetailViewModel(
            IAssessmentService assessmentService,
            IRawDataCacheRepository rawDataCacheRepository,
            IDispatcherService dispatcherService,
            INavigationService navigationService,
            IDialogService dialogService,
            ILogger<ModuleDetailViewModel> logger)
            : base(dispatcherService, navigationService, dialogService, logger)
        {
            ArgumentNullException.ThrowIfNull(assessmentService);
            ArgumentNullException.ThrowIfNull(rawDataCacheRepository);
            AssessmentService = assessmentService;
            RawDataCacheRepository = rawDataCacheRepository;
        }

        public FlowDirection FlowDirection
        {
            get { return Culture?.TextInfo.IsRightToLeft ?? false ? FlowDirection.RightToLeft : FlowDirection.LeftToRight; }
        }

        private IAssessmentService AssessmentService { get; }

        private IRawDataCacheRepository RawDataCacheRepository { get; }

        private List<string> SupportedLanaguages { get; set; } = new List<string>();

        [IndexerName("Item")]
        public new string this[string key]
        {
            get
            {
                var localizedResource = string.IsNullOrWhiteSpace(_language) ? null : Resources.Strings.Resources.ResourceManager.GetString($"{key}_{_language}");
                return localizedResource ?? Resources.Strings.Resources.ResourceManager.GetString(key) ?? string.Empty;
            }
        }

        public override async Task OnNavigatedToAsync()
        {
            using (var worker = ScheduleWork())
            {
                try
                {
                    var sections = new List<ObservableObject>();
                    Module? module = null;
                    if (!string.IsNullOrWhiteSpace(_moduleId))
                    {
                        module = await AssessmentService.GetModuleAsync(_moduleId, _language, worker.Token).ConfigureAwait(false);
                    }

                    _assessmentId = module?.AssessmentId;
                    if (module != null)
                    {
                        if (!string.IsNullOrWhiteSpace(module.Subtitle))
                        {
                            sections.Add(HeadlineTextEntry.Import(module.Subtitle));
                        }

                        switch (module.Type)
                        {
                            case AssessmentType.Gl:
                                sections.Add(ProviderDecoEntry.Import());
                                break;
                        }

                        if (!string.IsNullOrWhiteSpace(module.Description))
                        {
                            sections.Add(TextEntry.Import(module.Description));
                        }

                        //TODO: Add Sesssion
                        if ((module.Repeatable ?? 0) > 0)
                        {
                            sections.Add(SessionStateEntry.Import(module.Repeatable));
                        }

                        var format = this["ModuleDetailView_Minutes"];
                        sections.Add(IconTextEntry.Import(KnownIcons.Watch, string.Format(format, module.EstimateDuration)));
                    }

                    var languages = module?.Languages?.Select(x => x) ?? new List<string>();
                    await ExecuteOnUIThreadAsync(() => LoadonUIThread(sections, languages), worker.Token);
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

        protected override void OnPrepare(NavigationParameters navigationParameters)
        {
            base.OnPrepare(navigationParameters);
            _moduleId = navigationParameters.GetValue<string?>(NavigationParameter.Id);

            if (navigationParameters.TryGetValue(NavigationParameter.Type, out object? type) && Enum.TryParse(typeof(AssessmentType), type?.ToString(), true, out object? enumValue))
            {
                _assessmentType = (AssessmentType)enumValue;
            }

            _language = navigationParameters.GetValue<string?>(NavigationParameter.Data) ?? "de-DE";
            if (navigationParameters.ContainsKey(NavigationParameter.Type))
            {
                _language = navigationParameters.GetValue<string?>(NavigationParameter.Result) ?? _language;
            }

            Culture = new CultureInfo(_language);
            OnPropertyChanged(string.Empty);
            switch (_assessmentType)
            {
                case AssessmentType.Sk:
                    Title = Resources.Strings.Resources.AssessmentTypeSk;
                    break;
                case AssessmentType.Ea:
                    Title = Resources.Strings.Resources.AssessmentTypeEa;
                    break;
                case AssessmentType.So:
                    Title = Resources.Strings.Resources.AssessmentTypeSo;
                    break;
                case AssessmentType.Gl:
                    Title = Resources.Strings.Resources.AssessmentTypeGl;
                    break;
                case AssessmentType.Be:
                    Title = Resources.Strings.Resources.AssessmentTypeBe;
                    break;
            }
        }

        protected override void RefreshCommands()
        {
            base.RefreshCommands();
            OpenLanguageSelectionCommand?.NotifyCanExecuteChanged();
            StartAssessmentCommand?.NotifyCanExecuteChanged();
            Sections.OfType<SessionStateEntry>().FirstOrDefault()?.RefreshCommands();
        }

        private void LoadonUIThread(
            List<ObservableObject> sections,
            IEnumerable<string> supportedLanaguages)
        {
            Sections = new ObservableCollection<ObservableObject>(sections);
            SupportedLanaguages = supportedLanaguages?.ToList() ?? new List<string>();
            HasLanguageSelection = SupportedLanaguages.Count > 1;
            CanStartTest = !Sections.Any(x => x is TestSessionEntry);
            WeakReferenceMessenger.Default.Send(new UpdateToolbarMessage());
        }

        [RelayCommand(AllowConcurrentExecutions = false, CanExecute = nameof(CanOpenLanguageSelection))]
        private async Task OpenLanguageSelection(CancellationToken token)
        {
            using (var worker = ScheduleWork())
            {
                try
                {
                    var parameters = new NavigationParameters();
                    parameters.AddValue(NavigationParameter.Id, _moduleId);
                    parameters.AddValue(NavigationParameter.Data, _language);
                    parameters.AddValue(NavigationParameter.Result, string.Join(";", SupportedLanaguages));
                    await NavigationService.NavigateAsync(Routes.LanguageSelectionView!, worker.Token, parameters);
                }
                catch (OperationCanceledException)
                {
                    Logger?.LogDebug($"Canceled {nameof(OpenLanguageSelection)} in {GetType().Name}.");
                }
                catch (ObjectDisposedException)
                {
                    Logger?.LogDebug($"Canceled {nameof(OpenLanguageSelection)} in {GetType().Name}.");
                }
                catch (Exception ex)
                {
                    Logger?.LogError(ex, $"Unknown error while {nameof(OpenLanguageSelection)} in {GetType().Name}.");
                }
                finally
                {
                    UnscheduleWork(worker);
                }
            }
        }

        private bool CanOpenLanguageSelection()
        {
            return !IsBusy && HasLanguageSelection;
        }

        [RelayCommand(AllowConcurrentExecutions = false, CanExecute = nameof(CanStartAssessment))]
        private async Task StartAssessment(CancellationToken token)
        {
            using (var worker = ScheduleWork())
            {
                try
                {
                    if (_moduleId == null || _assessmentId == null)
                    {
                        Logger?.LogError($"Unable to create session while {nameof(StartAssessment)} in {GetType().Name}.");
                        return;
                    }

                    var session = await AssessmentService.CreateSessionAsync(_moduleId, _assessmentId, _language, worker.Token).ConfigureAwait(false);
                    if (session == null || session.SessionId == null || session.RawDataOrder == null || session.CurrentRawDataId == null)
                    {
                        Logger?.LogError($"Unable to create session while {nameof(StartAssessment)} in {GetType().Name}.");
                        return;
                    }

                    var questionIds = session.RawDataOrder.Split(";").ToList();
                    var sessionId = session.SessionId;
                    var assessmentId = session.AssessmentId;
                    var rawDataId = session.CurrentRawDataId;

                    var cachedData = await RawDataCacheRepository.GetItemAsync(sessionId, rawDataId, worker.Token).ConfigureAwait(false);
                    var rawData = cachedData.ToRawData();
                    string? route = rawData?.type.ToRoute();

                    if (string.IsNullOrWhiteSpace(route))
                    {
                        return;
                    }

                    var parameters = new NavigationParameters();
                    parameters.AddValue(NavigationParameter.Id, _moduleId);
                    parameters.AddValue(NavigationParameter.Data, sessionId);
                    parameters.AddValue(NavigationParameter.Language, _language);
                    await NavigationService.NavigateAsync(route, worker.Token, parameters);
                }
                catch (OperationCanceledException)
                {
                    Logger?.LogDebug($"Canceled {nameof(StartAssessment)} in {GetType().Name}.");
                }
                catch (ObjectDisposedException)
                {
                    Logger?.LogDebug($"Canceled {nameof(StartAssessment)} in {GetType().Name}.");
                }
                catch (Exception ex)
                {
                    Logger?.LogError(ex, $"Unknown error while {nameof(StartAssessment)} in {GetType().Name}.");
                }
                finally
                {
                    UnscheduleWork(worker);
                }
            }
        }

        private bool CanStartAssessment()
        {
            return !IsBusy && CanStartTest && !string.IsNullOrWhiteSpace(_moduleId) && !string.IsNullOrWhiteSpace(_assessmentId);
        }
    }
}
