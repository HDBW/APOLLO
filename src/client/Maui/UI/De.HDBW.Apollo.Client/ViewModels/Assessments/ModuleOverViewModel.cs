// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.
using System.Collections.ObjectModel;
using System.Globalization;
using CommunityToolkit.Mvvm.ComponentModel;
using De.HDBW.Apollo.Client.Contracts;
using De.HDBW.Apollo.Client.Models;
using De.HDBW.Apollo.Client.Models.Assessment;
using De.HDBW.Apollo.SharedContracts.Services;
using Invite.Apollo.App.Graph.Common.Models.Assessments;
using Microsoft.Extensions.Logging;

namespace De.HDBW.Apollo.Client.ViewModels.Assessments
{
    public partial class ModuleOverViewModel : BaseViewModel
    {
        [ObservableProperty]
        private ObservableCollection<ModuleTileEntry> _modules = new ObservableCollection<ModuleTileEntry>();

        [ObservableProperty]
        private string? _title;

        private IEnumerable<string>? _moduleIds;

        private AssessmentType _assessmentType;

        public ModuleOverViewModel(
            IAssessmentService assessmentService,
            IDispatcherService dispatcherService,
            INavigationService navigationService,
            IDialogService dialogService,
            ILogger<ModuleOverViewModel> logger)
            : base(dispatcherService, navigationService, dialogService, logger)
        {
            ArgumentNullException.ThrowIfNull(assessmentService);
            AssessmentService = assessmentService;
        }

        private IAssessmentService AssessmentService { get; }

        protected override void OnPrepare(NavigationParameters navigationParameters)
        {
            base.OnPrepare(navigationParameters);
            if (navigationParameters.TryGetValue(NavigationParameter.Data, out object? moduleIds))
            {
                var idString = moduleIds.ToString() ?? string.Empty;
                _moduleIds = idString.Split(';');
            }

            if (navigationParameters.TryGetValue(NavigationParameter.Type, out object? type) && Enum.TryParse(typeof(AssessmentType), type?.ToString(), true, out object? enumValue))
            {
                _assessmentType = (AssessmentType)enumValue;
            }

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

        public override async Task OnNavigatedToAsync()
        {
            using (var worker = ScheduleWork())
            {
                try
                {
                    var sections = new List<ObservableObject>();
                    var moduleTiles = await AssessmentService.GetModuleTilesAsync(_moduleIds ?? new List<string>(), worker.Token).ConfigureAwait(false);
                    var modules = moduleTiles.Select((x) => ModuleTileEntry.Import(x, null, null, false, Interact, CanInteract, ToggleFavorite, CanToggleFavorite));
                    await ExecuteOnUIThreadAsync(
                        () => LoadonUIThread(modules), worker.Token);
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
        }

        private void LoadonUIThread(IEnumerable<ModuleTileEntry> modules)
        {
            Modules = new ObservableCollection<ModuleTileEntry>(modules);
        }

        private bool CanToggleFavorite(ModuleTileEntry entry)
        {
            return !IsBusy;
        }

        private async Task ToggleFavorite(ModuleTileEntry entry, CancellationToken token) => throw new NotImplementedException();

        private bool CanInteract(ModuleTileEntry entry)
        {
            return !IsBusy;
        }

        private async Task Interact(ModuleTileEntry entry, CancellationToken token) => throw new NotImplementedException();

    }
}
