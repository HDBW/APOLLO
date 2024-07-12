// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Collections.ObjectModel;
using System.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel;
using De.HDBW.Apollo.Client.Contracts;
using De.HDBW.Apollo.Client.Models;
using De.HDBW.Apollo.Client.Models.Interactions;
using De.HDBW.Apollo.SharedContracts.Services;
using Microsoft.Extensions.Logging;

namespace De.HDBW.Apollo.Client.ViewModels.Assessments
{
    public partial class JobSearchViewModel : BaseViewModel
    {
        [ObservableProperty]
        private ObservableCollection<InteractionEntry> _searchResults = new ObservableCollection<InteractionEntry>();

        [ObservableProperty]
        private string? _searchTerm;

        [ObservableProperty]
        private string? _descriptionText;

        private List<InteractionEntry> _allItems = new List<InteractionEntry>();
        private CancellationTokenSource? _cts;

        public JobSearchViewModel(
            IAssessmentService assessmentService,
            IDispatcherService dispatcherService,
            INavigationService navigationService,
            IDialogService dialogService,
            ILogger<JobSearchViewModel> logger)
            : base(dispatcherService, navigationService, dialogService, logger)
        {
            ArgumentNullException.ThrowIfNull(assessmentService);
            AssessmentService = assessmentService;
        }

        private IAssessmentService AssessmentService { get; }

        public override async Task OnNavigatedToAsync()
        {
            using (var worker = ScheduleWork())
            {
                try
                {
                    var jobs = await AssessmentService.GetJobsAsync(worker.Token).ConfigureAwait(false);
                    _allItems = new List<InteractionEntry>();
                    foreach (var job in jobs)
                    {
                        var data = new NavigationParameters
                        {
                            { NavigationParameter.Title, job.Title },
                            { NavigationParameter.Id, job.Id },
                        };
                        _allItems.Add(InteractionEntry.Import(job.Title, data, Interact, CanInteract));
                    }

                    await ExecuteOnUIThreadAsync(() => LoadonUIThread(_allItems), worker.Token).ConfigureAwait(false);
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
            foreach (var item in SearchResults)
            {
                item.RefreshCommands();
            }
        }

        protected override void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);
            if (e.PropertyName != nameof(SearchTerm))
            {
                return;
            }

            _cts?.Dispose();
            _cts = new CancellationTokenSource();
            var token = _cts.Token;
            StartSearchAsync(token);
        }

        private async void StartSearchAsync(CancellationToken token)
        {
            try
            {
                var term = SearchTerm;
                var result = await Task.Run(async () =>
                {
                    await Task.Delay(500, token).ConfigureAwait(false);
                    if (string.IsNullOrWhiteSpace(term))
                    {
                        return _allItems;
                    }

                    return _allItems.Where(x => x.Text != null && x.Text.Contains(term, StringComparison.CurrentCultureIgnoreCase)).ToList();
                });
                await ExecuteOnUIThreadAsync(() => LoadonUIThread(result), token);
            }
            catch (OperationCanceledException)
            {
                Logger?.LogDebug($"Canceled {nameof(StartSearchAsync)} in {GetType().Name}.");
            }
            catch (ObjectDisposedException)
            {
                Logger?.LogDebug($"Canceled {nameof(StartSearchAsync)} in {GetType().Name}.");
            }
            catch (Exception ex)
            {
                Logger?.LogError(ex, $"Unknown error while {nameof(StartSearchAsync)} in {GetType().Name}.");
            }
            finally
            {
                if (_cts != null && !token.IsCancellationRequested)
                {
                    _cts?.Dispose();
                    _cts = null;
                }
            }
        }

        private void LoadonUIThread(List<InteractionEntry> items)
        {
            SearchResults = new ObservableCollection<InteractionEntry>(items);
            DescriptionText = string.Format(Resources.Strings.Resources.TxtAssesmentsJobSearchViewDescription, _allItems.Count);
        }

        private bool CanInteract(InteractionEntry entry)
        {
            return !IsBusy && entry.Data != null;
        }

        private async Task Interact(InteractionEntry entry)
        {
            Logger.LogInformation($"Invoked {nameof(Interact)} in {GetType().Name}.");
            using (var worker = ScheduleWork())
            {
                try
                {
                    var parameters = entry.Data as NavigationParameters;
                    await NavigationService.NavigateAsync(Routes.JobSearchResultView, worker.Token, parameters);
                }
                catch (OperationCanceledException)
                {
                    Logger?.LogDebug($"Canceled {nameof(Interact)} in {GetType().Name}.");
                }
                catch (ObjectDisposedException)
                {
                    Logger?.LogDebug($"Canceled {nameof(Interact)} in {GetType().Name}.");
                }
                catch (Exception ex)
                {
                    Logger?.LogError(ex, $"Unknown error while {nameof(Interact)} in {GetType().Name}.");
                }
                finally
                {
                    UnscheduleWork(worker);
                    if (_cts != null)
                    {
                        _cts?.Dispose();
                        _cts = null;
                    }
                }
            }
        }
    }
}
