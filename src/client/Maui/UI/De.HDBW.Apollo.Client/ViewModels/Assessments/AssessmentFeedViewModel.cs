// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using De.HDBW.Apollo.Client.Contracts;
using De.HDBW.Apollo.Client.Models;
using De.HDBW.Apollo.Client.Models.Assessment;
using De.HDBW.Apollo.SharedContracts.Services;
using Microsoft.Extensions.Logging;

namespace De.HDBW.Apollo.Client.ViewModels.Assessments
{
    public partial class AssessmentFeedViewModel : BaseViewModel
    {
        [ObservableProperty]
        private ObservableCollection<ObservableObject> _sections = new ObservableCollection<ObservableObject>();

        public AssessmentFeedViewModel(
            IAssessmentService assessmentService,
            IDispatcherService dispatcherService,
            INavigationService navigationService,
            IDialogService dialogService,
            ILogger<AssessmentFeedViewModel> logger)
            : base(dispatcherService, navigationService, dialogService, logger)
        {
            ArgumentNullException.ThrowIfNull(assessmentService);
            AssessmentService = assessmentService;
        }

        public string FavoriteIcon
        {
            get
            {
                return KnownIcons.IsFavorite;
            }
        }

        private IAssessmentService AssessmentService { get; }

        public override async Task OnNavigatedToAsync()
        {
            using (var worker = ScheduleWork())
            {
                try
                {
                    var sections = new List<ObservableObject>();
                    var assessmentTiles = await AssessmentService.GetAssessmentTilesAsync(worker.Token).ConfigureAwait(false);
                    var tiles = assessmentTiles.GroupBy(x => x.Grouping).OrderBy(x => x.Key);
                    foreach (var tile in tiles)
                    {
                        if (string.IsNullOrWhiteSpace(tile.Key))
                        {
                            sections.Add(HeadlineTextEntry.Import(Resources.Strings.Resources.AssessmentFeed_Title));
                            sections.Add(SublineTextEntry.Import(Resources.Strings.Resources.AssessmentFeed_Subline));
                        }
                        else
                        {
                            sections.Add(TextEntry.Import(tile.Key));
                        }

                        foreach (var item in tile.ToList())
                        {
                            sections.Add(AssessmentTileEntry.Import(item));
                        }
                    }

                    await ExecuteOnUIThreadAsync(
                        () => LoadonUIThread(sections), worker.Token);
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

        private void LoadonUIThread(List<ObservableObject> sections)
        {
            Sections = new ObservableCollection<ObservableObject>(sections);
        }

        private bool CanOpenFavorite()
        {
            return !IsBusy;
        }

        [RelayCommand(AllowConcurrentExecutions = false, CanExecute = nameof(CanOpenFavorite))]
        private async Task OpenFavorite(CancellationToken token)
        {
            try
            {
                Logger.LogInformation($"Invoked {nameof(OpenFavorite)} in {GetType().Name}.");
                token.ThrowIfCancellationRequested();
            }
            catch (Exception ex)
            {
                Logger?.LogError(ex, $"Unknown error while {nameof(OpenFavorite)} in {GetType().Name}.");
            }
        }
    }
}
