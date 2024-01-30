// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using De.HDBW.Apollo.Client.Contracts;
using De.HDBW.Apollo.Client.Models;
using De.HDBW.Apollo.Client.Models.Interactions;
using De.HDBW.Apollo.Data.Helper;
using Invite.Apollo.App.Graph.Common.Models.Taxonomy;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.Controls;

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
            ILogger<OccupationSearchViewModel> logger)
            : base(dispatcherService, navigationService, dialogService, logger)
        {
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
                    Task.Run(() => DoSearchAsync(value));
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

        public override async Task OnNavigatedToAsync()
        {
            using (var worker = ScheduleWork())
            {
                try
                {
                    using (var stream = await FileSystem.OpenAppPackageFileAsync("CareerInfo_JobTtitle_filtered.txt"))
                    {
                        using (var reader = new StreamReader(stream))
                        {
                            var occupationNames = ReadLines(reader).Order().ToList();
                            _occupations = occupationNames.Select(x => new KldbOccupation() { Id = Guid.NewGuid().ToString("N"), PreferedTerm = new List<string>() { x } }).OfType<Occupation>().ToList();
                        }
                    }

                    await ExecuteOnUIThreadAsync(() => LoadonUIThread(), worker.Token);
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
                    var items = string.IsNullOrWhiteSpace(searchtext)
                         ? new List<InteractionEntry>()
                         : await Task.Run(() => { return _occupations.Where(x => x.PreferedTerm?.FirstOrDefault()?.Contains(searchtext, StringComparison.InvariantCultureIgnoreCase) ?? false).Select(x => InteractionEntry.Import(x.PreferedTerm?.FirstOrDefault(), x, (x) => { return Task.CompletedTask; }, (x) => { return true; })).ToList(); }, token);
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

        private IEnumerable<string> ReadLines(StreamReader reader)
        {
            string line;
            while ((line = reader?.ReadLine()) != null)
            {
                yield return line;
            }
        }

        private async void ApplyAndClose()
        {
            using (var worker = ScheduleWork())
            {
                try
                {
                    _parameters = _parameters ?? new NavigationParameters();
                    var occupation = SelectedItem?.Data as Occupation;
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
                _cts?.Dispose();
                _cts = new CancellationTokenSource();
                await Task.Delay(500);
                token = _cts?.Token;
                if (!token.HasValue)
                {
                    return;
                }

                var items = string.IsNullOrWhiteSpace(searchtext)
                    ? new List<InteractionEntry>()
                    : _occupations.Where(x => x.PreferedTerm?.FirstOrDefault()?.Contains(searchtext, StringComparison.InvariantCultureIgnoreCase) ?? false).Select(x => InteractionEntry.Import(x.PreferedTerm?.FirstOrDefault(), x, (x) => { return Task.CompletedTask; }, (x) => { return true; })).ToList();

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
                if (!(token?.IsCancellationRequested ?? false))
                {
                    _cts?.Dispose();
                    _cts = null;
                }
            }
        }
    }
}
