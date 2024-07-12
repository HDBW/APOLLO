// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Collections.ObjectModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using De.HDBW.Apollo.Client.Contracts;
using De.HDBW.Apollo.Client.Helper;
using De.HDBW.Apollo.Client.Models;
using De.HDBW.Apollo.Client.Models.Assessment;
using De.HDBW.Apollo.SharedContracts.Services;
using Invite.Apollo.App.Graph.Common.Models.Assessments;
using Microsoft.Extensions.Logging;

namespace De.HDBW.Apollo.Client.ViewModels.Assessments
{
    public partial class ResultOverViewModel : BaseViewModel
    {
        [ObservableProperty]
        private ObservableCollection<ObservableObject> _sections = new ObservableCollection<ObservableObject>();

        [ObservableProperty]
        private ObservableCollection<SegmentScore> _details = new ObservableCollection<SegmentScore>();

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(FlowDirection))]
        private CultureInfo _culture = new CultureInfo("de-DE");
        private string? _moduleId;
        private string? _assessmentId;
        private string? _sessionId;
        private string? _language;

        public ResultOverViewModel(
            ISheetService sheetService,
            IAssessmentService assessmentService,
            IDispatcherService dispatcherService,
            INavigationService navigationService,
            IDialogService dialogService,
            ILogger<ResultOverViewModel> logger)
            : base(dispatcherService, navigationService, dialogService, logger)
        {
            ArgumentNullException.ThrowIfNull(sheetService);
            ArgumentNullException.ThrowIfNull(assessmentService);
            SheetService = sheetService;
            AssessmentService = assessmentService;
        }

        public FlowDirection FlowDirection
        {
            get { return Culture?.TextInfo.IsRightToLeft ?? false ? FlowDirection.RightToLeft : FlowDirection.LeftToRight; }
        }

        private ISheetService SheetService { get; }

        private IAssessmentService AssessmentService { get; }

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
                    var details = new List<SegmentScore>();
                    Module? module = null;
                    if (!string.IsNullOrWhiteSpace(_moduleId))
                    {
                        module = await AssessmentService.GetModuleAsync(_moduleId, _language, worker.Token).ConfigureAwait(false);
                    }

                    _assessmentId = module?.AssessmentId;
                    _sessionId = module?.SessionId;
                    var quantity_Patter = "Quantity_{0}";
                    if (module != null)
                    {
                        switch (module.Type)
                        {
                            case AssessmentType.Sk:
                                sections.Add(DecoEntry.Import(module.Type));
                                sections.Add(HeadlineTextEntry.Import(this["TxtAssesmentsResultOverViewCongrats"]));
                                sections.Add(TextEntry.Import($"<p>{string.Format(this["TxtAssesmentsResultOverViewSkillsTestFinished"], module.JobId)}</p><p>{this["TxtAssesmentsResultOverViewSkillsTestFinishedDescription"]}</p>"));
                                foreach (var score in module.SegmentScores)
                                {
                                    details.Add(score);
                                }

                                sections.Add(ModuleScoreEntry.Import(module.ModuleScore, this[string.Format(quantity_Patter, module.ModuleScore.Quantity)], module.Type, HandleOpenDetails, CanHandleOpenDetails));

                                break;
                            case AssessmentType.Ea:
                                sections.Add(DecoEntry.Import(module.Type));
                                sections.Add(HeadlineTextEntry.Import(this["TxtAssesmentsResultOverViewCongrats"]));
                                sections.Add(TextEntry.Import($"<p>{string.Format(this["TxtAssesmentsResultOverViewExperienceTestFinished"], module.LocalizedJobName)}</p><p>{this["TxtAssesmentsResultOverViewExperienceTestFinishedDescription"]}</p>"));
                                foreach (var score in module.SegmentScores)
                                {
                                    sections.Add(ModuleScoreEntry.Import(score.ToModuleScore(), this[string.Format(quantity_Patter, score.Quantity)], module.Type));
                                }

                                break;
                            case AssessmentType.So:
                                sections.Add(DecoEntry.Import(module.Type));
                                sections.Add(SublineTextEntry.Import(module.Title));
                                var moduleScoreQuantity = this[string.Format(quantity_Patter, module.ModuleScore.Quantity)];
                                sections.Add(HeadlineTextEntry.Import(moduleScoreQuantity));
                                sections.Add(TextEntry.Import(string.Format(this["TxtAssesmentsResultOverViewSoftSkillsTestFinishedDescription"], moduleScoreQuantity)));
                                foreach (var score in module.SegmentScores)
                                {
                                    sections.Add(ModuleScoreEntry.Import(score.ToModuleScore(), this[string.Format(quantity_Patter, score.Quantity)], module.Type));
                                }

                                break;
                            case AssessmentType.Gl:
                                sections.Add(DecoEntry.Import(module.Type));
                                sections.Add(SublineTextEntry.Import(this["TxtAssesmentsResultOverviewGermanKnowledge"]));
                                sections.Add(HeadlineTextEntry.Import(this["TxtAssesmentsResultOverviewGermanYourResult"]));
                                foreach (var score in module.SegmentScores)
                                {
                                    sections.Add(ModuleScoreEntry.Import(score.ToModuleScore(), this[string.Format(quantity_Patter, score.Quantity)], module.Type));
                                }

                                break;
                            case AssessmentType.Be:
                                break;
                        }
                    }

                    await ExecuteOnUIThreadAsync(() => LoadonUIThread(sections, details), worker.Token);
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
            _language = navigationParameters.GetValue<string?>(NavigationParameter.Language) ?? "de-DE";

            Culture = new CultureInfo(_language);
            OnPropertyChanged(string.Empty);
        }

        protected override void RefreshCommands()
        {
            base.RefreshCommands();
            NavigateBackCommand?.NotifyCanExecuteChanged();
            foreach (var section in Sections.OfType<ModuleScoreEntry>())
            {
                section.RefreshCommands();
            }
        }

        private void LoadonUIThread(
            List<ObservableObject> sections, List<SegmentScore> details)
        {
            Sections = new ObservableCollection<ObservableObject>(sections);
            Details = new ObservableCollection<SegmentScore>(details);
        }

        [RelayCommand(AllowConcurrentExecutions = false, CanExecute = nameof(CanNavigateBack))]
        private async Task NavigateBack(CancellationToken token)
        {
            Logger.LogInformation($"Invoked {nameof(NavigateBackCommand)} in {GetType().Name}.");
            using (var worker = ScheduleWork(token))
            {
                try
                {
                    if (SheetService.IsShowingSheet)
                    {
                        await SheetService.CloseAsync<ResultDetailSheetViewModel>();
                    }

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
            return !IsBusy;
        }

        private bool CanHandleOpenDetails(ModuleScoreEntry entry)
        {
            return !IsBusy && Details != null;
        }

        private async Task HandleOpenDetails(ModuleScoreEntry entry, CancellationToken token)
        {
            using (var worker = ScheduleWork(token))
            {
                try
                {
                    var parameters = new NavigationParameters();
                    await SheetService.OpenAsync(Routes.ResultDetailSheet, worker.Token, parameters);
                }
                catch (OperationCanceledException)
                {
                    Logger?.LogDebug($"Canceled {nameof(HandleOpenDetails)} in {GetType().Name}.");
                }
                catch (ObjectDisposedException)
                {
                    Logger?.LogDebug($"Canceled {nameof(HandleOpenDetails)} in {GetType().Name}.");
                }
                catch (Exception ex)
                {
                    Logger?.LogError(ex, $"Unknown error while {nameof(HandleOpenDetails)} in {GetType().Name}.");
                }
                finally
                {
                    UnscheduleWork(worker);
                }
            }
        }
    }
}
