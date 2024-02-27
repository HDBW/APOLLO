// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using De.HDBW.Apollo.Client.Contracts;
using De.HDBW.Apollo.Client.Models;
using De.HDBW.Apollo.Client.Models.Interactions;
using De.HDBW.Apollo.Data.Helper;
using De.HDBW.Apollo.SharedContracts.Services;
using Invite.Apollo.App.Graph.Common.Models.Taxonomy;
using Microsoft.Extensions.Logging;
using OccupationGrpcService.Protos;

namespace De.HDBW.Apollo.Client.ViewModels.Profile
{
    public partial class OccupationSearchViewModel : BaseViewModel
    {
        private string? _searchText;

        private InteractionEntry? _selectedItem;

        [ObservableProperty]
        private ObservableCollection<InteractionEntry> _items = new ObservableCollection<InteractionEntry>();

        private List<Occupation> _occupations = new List<Occupation>();

        private NavigationParameters? _parameters;
        private CancellationTokenSource? _cts;

        public OccupationSearchViewModel(
            IDispatcherService dispatcherService,
            INavigationService navigationService,
            IDialogService dialogService,
            ILogger<OccupationSearchViewModel> logger,
            IOccupationService occupationService)
            : base(dispatcherService, navigationService, dialogService, logger)
        {
            ArgumentNullException.ThrowIfNull(occupationService);
            OccupationService = occupationService;
        }

        public string? SearchText
        {
            get
            {
                return _searchText;
            }

            set
            {
                if (SetProperty(ref _searchText, value))
                {
                    RefreshCommands();
                    Task.Run(() => DoSearchAsync(_searchText));
                }
            }
        }

        public InteractionEntry? SelectedItem
        {
            get
            {
                return _selectedItem;
            }

            set
            {
                if (SetProperty(ref _selectedItem, value))
                {
                    RefreshCommands();
                    if (_selectedItem != null)
                    {
                        ApplyAndClose();
                    }
                }
            }
        }

        private IOccupationService OccupationService { get; }

        public override Task OnNavigatedToAsync()
        {
            using (var worker = ScheduleWork())
            {
                try
                {
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

            return Task.CompletedTask;
        }

        protected override void OnPrepare(NavigationParameters navigationParameters)
        {
            base.OnPrepare(navigationParameters);
            _parameters = navigationParameters;
        }

        protected override void RefreshCommands()
        {
            base.RefreshCommands();
            SearchCommand?.NotifyCanExecuteChanged();
        }

        private void LoadonUIThread()
        {
        }

        [RelayCommand(AllowConcurrentExecutions = false, CanExecute = nameof(CanSearch), IncludeCancelCommand =true)]
        private async Task Search(string searchtext, CancellationToken token)
        {
            using (var worker = ScheduleWork(token))
            {
                try
                {
                    _cts?.Cancel();
                    worker.Token.ThrowIfCancellationRequested();
                    var result = string.IsNullOrWhiteSpace(searchtext)
                        ? null
                        : await OccupationService.SearchAsync(searchtext, worker.Token).ConfigureAwait(false);

                    var items = result == null
                         ? new List<InteractionEntry>()
                         : await Task.Run(() => { return result.Select(x => InteractionEntry.Import(x.Title, x, (x) => { return Task.CompletedTask; }, (x) => { return true; })).ToList(); }, worker.Token);
                    if (!items.Any() && !string.IsNullOrWhiteSpace(searchtext))
                    {
                        items.Add(InteractionEntry.Import(searchtext, new UnknownOccupation() { PreferedTerm = new List<string>() { searchtext } }, (x) => { return Task.CompletedTask; }, (x) => { return true; }));
                    }

                    worker.Token.ThrowIfCancellationRequested();
                    Items = new ObservableCollection<InteractionEntry>(items);
                }
                catch (OperationCanceledException)
                {
                    Logger?.LogDebug($"Canceled {nameof(Search)} in {GetType().Name}.");
                }
                catch (ObjectDisposedException)
                {
                    Logger?.LogDebug($"Canceled {nameof(Search)} in {GetType().Name}.");
                }
                catch (Exception ex)
                {
                    Logger?.LogError(ex, $"Unknown error while {nameof(Search)} in {GetType().Name}.");
                }
                finally
                {
                    UnscheduleWork(worker);
                }
            }
        }

        private bool CanSearch(string searchtext)
        {
            return !IsBusy;
        }

        private async void ApplyAndClose()
        {
            using (var worker = ScheduleWork())
            {
                try
                {
                    _parameters = _parameters ?? new NavigationParameters();
                    Occupation? occupation = null;
                    switch (SelectedItem?.Data)
                    {
                        case Occupation unknowOccupation:
                            occupation = await OccupationService.CreateAsync(unknowOccupation.PreferedTerm.First(), worker.Token).ConfigureAwait(false);
                            break;
                        case OccupationTerm term:
                            ArgumentNullException.ThrowIfNull(term.OccupationId);
                            occupation = await OccupationService.GetItemByIdAsync(term.OccupationId, worker.Token).ConfigureAwait(false);
                            break;
                    }

                    if (occupation == null)
                    {
                        throw new NotSupportedException("Unable to get or create new occupation.");
                    }

                    _parameters.AddValue(NavigationParameter.Result, occupation.Serialize());
                    await NavigationService.PopAsync(worker.Token, _parameters);
                }
                catch (OperationCanceledException)
                {
                    Logger?.LogDebug($"Canceled {nameof(ApplyAndClose)} in {GetType().Name}.");
                }
                catch (ObjectDisposedException)
                {
                    Logger?.LogDebug($"Canceled {nameof(ApplyAndClose)} in {GetType().Name}.");
                }
                catch (Exception ex)
                {
                    Logger?.LogError(ex, $"Unknown error while {nameof(ApplyAndClose)} in {GetType().Name}.");
                }
                finally
                {
                    UnscheduleWork(worker);
                }
            }
        }

        private async Task DoSearchAsync(string? searchtext)
        {
            CancellationToken? token = null;
            try
            {
                _cts?.Cancel();
                _cts?.Dispose();
                _cts = new CancellationTokenSource();
                token = _cts?.Token;
                if (!token.HasValue)
                {
                    return;
                }

                await Task.Delay(500, token.Value);
                token.Value.ThrowIfCancellationRequested();

                var result = string.IsNullOrWhiteSpace(searchtext)
                    ? null
                    : await OccupationService.SearchAsync(searchtext, token.Value).ConfigureAwait(false);

                var items = result == null
                    ? new List<InteractionEntry>()
                    : result.Select(x => InteractionEntry.Import(x.Title, x, (x) => { return Task.CompletedTask; }, (x) => { return true; })).ToList();

                if (!items.Any() && !string.IsNullOrWhiteSpace(searchtext))
                {
                    items.Add(InteractionEntry.Import(searchtext, new UnknownOccupation() { PreferedTerm = new List<string>() { searchtext } }, (x) => { return Task.CompletedTask; }, (x) => { return true; }));
                }

                token.Value.ThrowIfCancellationRequested();
                await ExecuteOnUIThreadAsync(
                    () =>
                    {
                        Items = new ObservableCollection<InteractionEntry>(items);
                    }, token.Value).ConfigureAwait(false);
            }
            catch (OperationCanceledException)
            {
                Logger?.LogDebug($"Canceled {nameof(DoSearchAsync)} in {GetType().Name}.");
            }
            catch (ObjectDisposedException)
            {
                Logger?.LogDebug($"Canceled {nameof(DoSearchAsync)} in {GetType().Name}.");
            }
            catch (Exception ex)
            {
                Logger?.LogError(ex, $"Unknown error while {nameof(DoSearchAsync)} in {GetType().Name}.");
            }
            finally
            {
                var current = _cts;
                _cts = null;
                if (!(token?.IsCancellationRequested ?? false) && current?.Token == token)
                {
                    current?.Dispose();
                    current = null;
                }
            }
        }
    }
}
