// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using De.HDBW.Apollo.Client.Contracts;
using De.HDBW.Apollo.Client.Models;
using De.HDBW.Apollo.Client.Models.Interactions;
using Invite.Apollo.App.Graph.Common.Models.UserProfile;
using Invite.Apollo.App.Graph.Common.Models.UserProfile.Enums;
using Microsoft.Extensions.Logging;

namespace De.HDBW.Apollo.Client.ViewModels.Profile.LanguageEditors
{
    public partial class LanguageViewModel : BaseViewModel
    {
        [ObservableProperty]
        private string? _languageName;

        [ObservableProperty]
        private ObservableCollection<InteractionEntry> _languageNiveaus = new ObservableCollection<InteractionEntry>();

        [ObservableProperty]
        private InteractionEntry? _selectedLanguageNiveau;

        private Language? _language;

        public LanguageViewModel(
            IDispatcherService dispatcherService,
            INavigationService navigationService,
            IDialogService dialogService,
            ILogger<LanguageViewModel> logger)
            : base(dispatcherService, navigationService, dialogService, logger)
        {
        }

        public override async Task OnNavigatedToAsync()
        {
            using (var worker = ScheduleWork())
            {
                try
                {
                    var languageNiveaus = new List<InteractionEntry>();
                    languageNiveaus.Add(InteractionEntry.Import(Resources.Strings.Resources.LanguageNiveau_A1, LanguageNiveau.A1, (x) => { return Task.CompletedTask; }, (x) => { return true; }));
                    languageNiveaus.Add(InteractionEntry.Import(Resources.Strings.Resources.LanguageNiveau_A2, LanguageNiveau.A2, (x) => { return Task.CompletedTask; }, (x) => { return true; }));
                    languageNiveaus.Add(InteractionEntry.Import(Resources.Strings.Resources.LanguageNiveau_B1, LanguageNiveau.B1, (x) => { return Task.CompletedTask; }, (x) => { return true; }));
                    languageNiveaus.Add(InteractionEntry.Import(Resources.Strings.Resources.LanguageNiveau_B2, LanguageNiveau.B2, (x) => { return Task.CompletedTask; }, (x) => { return true; }));
                    languageNiveaus.Add(InteractionEntry.Import(Resources.Strings.Resources.LanguageNiveau_C1, LanguageNiveau.C1, (x) => { return Task.CompletedTask; }, (x) => { return true; }));
                    languageNiveaus.Add(InteractionEntry.Import(Resources.Strings.Resources.LanguageNiveau_C2, LanguageNiveau.C2, (x) => { return Task.CompletedTask; }, (x) => { return true; }));

                    await ExecuteOnUIThreadAsync(
                        () => LoadonUIThread(languageNiveaus), worker.Token);
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
            SearchLanguageCommand?.NotifyCanExecuteChanged();
        }

        private void LoadonUIThread(List<InteractionEntry> languageNiveaus)
        {
            LanguageNiveaus = new ObservableCollection<InteractionEntry>(languageNiveaus);
            SelectedLanguageNiveau = LanguageNiveaus.FirstOrDefault();
        }

        [RelayCommand(AllowConcurrentExecutions = false, CanExecute = nameof(CanSearchLanguage))]
        private async Task SearchLanguage(CancellationToken token)
        {
            using (var worker = ScheduleWork(token))
            {
                try
                {
                    await NavigationService.NavigateAsync(Routes.LanguageSearchView, token);
                }
                catch (OperationCanceledException)
                {
                    Logger?.LogDebug($"Canceled {nameof(SearchLanguage)} in {GetType().Name}.");
                }
                catch (ObjectDisposedException)
                {
                    Logger?.LogDebug($"Canceled {nameof(SearchLanguage)} in {GetType().Name}.");
                }
                catch (Exception ex)
                {
                    Logger?.LogError(ex, $"Unknown error in {nameof(SearchLanguage)} in {GetType().Name}.");
                }
                finally
                {
                    UnscheduleWork(worker);
                }
            }
        }

        private bool CanSearchLanguage()
        {
            return !IsBusy;
        }
    }
}
