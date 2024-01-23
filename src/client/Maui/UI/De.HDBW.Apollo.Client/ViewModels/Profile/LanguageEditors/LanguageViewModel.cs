// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using De.HDBW.Apollo.Client.Contracts;
using De.HDBW.Apollo.Client.Models;
using De.HDBW.Apollo.Client.Models.Interactions;
using De.HDBW.Apollo.Data.Helper;
using De.HDBW.Apollo.SharedContracts.Repositories;
using De.HDBW.Apollo.SharedContracts.Services;
using Invite.Apollo.App.Graph.Common.Models.UserProfile;
using Invite.Apollo.App.Graph.Common.Models.UserProfile.Enums;
using Microsoft.Extensions.Logging;
using UserProfile = Invite.Apollo.App.Graph.Common.Models.UserProfile.Profile;

namespace De.HDBW.Apollo.Client.ViewModels.Profile.LanguageEditors
{
    public partial class LanguageViewModel : AbstractSaveDataViewModel
    {
        [ObservableProperty]
        [Required(ErrorMessageResourceType = typeof(Resources.Strings.Resources), ErrorMessageResourceName = nameof(Resources.Strings.Resources.GlobalError_PropertyRequired))]
        private string? _languageName;

        [ObservableProperty]
        private ObservableCollection<InteractionEntry> _languageNiveaus = new ObservableCollection<InteractionEntry>();

        [ObservableProperty]
        [Required(ErrorMessageResourceType = typeof(Resources.Strings.Resources), ErrorMessageResourceName = nameof(Resources.Strings.Resources.GlobalError_PropertyRequired))]
        private InteractionEntry? _selectedLanguageNiveau;

        private Language? _language;

        private string? _languageId;

        private string? _editState;

        private string? _selectionResult;

        private CultureInfo? _code;

        private User? _user;

        public LanguageViewModel(
            IDispatcherService dispatcherService,
            INavigationService navigationService,
            IDialogService dialogService,
            ILogger<LanguageViewModel> logger,
            IUserService userService,
            IUserRepository userRepository)
            : base(dispatcherService, navigationService, dialogService, logger)
        {
            ArgumentNullException.ThrowIfNull(userRepository);
            ArgumentNullException.ThrowIfNull(userService);
            UserRepository = userRepository;
            UserService = userService;
        }

        private IUserRepository UserRepository { get; }

        private IUserService UserService { get; }

        public override async Task OnNavigatedToAsync()
        {
            using (var worker = ScheduleWork())
            {
                try
                {
                    // Loaddate
                    var user = await UserRepository.GetItemAsync(worker.Token).ConfigureAwait(false);
                    var language = user?.Profile?.LanguageSkills.FirstOrDefault(x => x.Id == _languageId);
                    var niveau = language?.Niveau;
                    var code = language?.Code;
                    var name = code?.DisplayName;
                    var isDirty = false;

                    // Restore edit state
                    if (!string.IsNullOrWhiteSpace(_editState))
                    {
                        var currentState = _editState.Deserialize<Language>();
                        if (currentState != null)
                        {
                            niveau = currentState.Niveau;
                            code = currentState.Name != null ? CultureInfo.GetCultures(CultureTypes.AllCultures).FirstOrDefault(c => c.Name == currentState.Name) : null;
                            name = currentState.Name;
                        }
                    }

                    // Override language selection
                    if (!string.IsNullOrWhiteSpace(_selectionResult))
                    {
                        code = CultureInfo.GetCultures(CultureTypes.AllCultures).FirstOrDefault(c => c.Name == _selectionResult);
                        name = code?.DisplayName;
                    }

                    isDirty = name != language?.Name || niveau != language?.Niveau;

                    var languageNiveaus = new List<InteractionEntry>();
                    languageNiveaus.Add(InteractionEntry.Import(Resources.Strings.Resources.LanguageNiveau_A1, LanguageNiveau.A1, (x) => { return Task.CompletedTask; }, (x) => { return true; }));
                    languageNiveaus.Add(InteractionEntry.Import(Resources.Strings.Resources.LanguageNiveau_A2, LanguageNiveau.A2, (x) => { return Task.CompletedTask; }, (x) => { return true; }));
                    languageNiveaus.Add(InteractionEntry.Import(Resources.Strings.Resources.LanguageNiveau_B1, LanguageNiveau.B1, (x) => { return Task.CompletedTask; }, (x) => { return true; }));
                    languageNiveaus.Add(InteractionEntry.Import(Resources.Strings.Resources.LanguageNiveau_B2, LanguageNiveau.B2, (x) => { return Task.CompletedTask; }, (x) => { return true; }));
                    languageNiveaus.Add(InteractionEntry.Import(Resources.Strings.Resources.LanguageNiveau_C1, LanguageNiveau.C1, (x) => { return Task.CompletedTask; }, (x) => { return true; }));
                    languageNiveaus.Add(InteractionEntry.Import(Resources.Strings.Resources.LanguageNiveau_C2, LanguageNiveau.C2, (x) => { return Task.CompletedTask; }, (x) => { return true; }));
                    var selectedNivau = languageNiveaus.FirstOrDefault(x => (LanguageNiveau?)x.Data == niveau);

                    await ExecuteOnUIThreadAsync(
                        () => LoadonUIThread(user, language, code, name, languageNiveaus, selectedNivau, isDirty), worker.Token);
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
            _languageId = navigationParameters.GetValue<string?>(NavigationParameter.Id);
            if (navigationParameters.ContainsKey(NavigationParameter.Data))
            {
                _editState = navigationParameters.GetValue<string?>(NavigationParameter.Data);
            }

            _selectionResult = navigationParameters.ContainsKey(NavigationParameter.Result) ? navigationParameters.GetValue<string?>(NavigationParameter.Result) : null;
        }

        protected override void RefreshCommands()
        {
            base.RefreshCommands();
            SearchLanguageCommand?.NotifyCanExecuteChanged();
        }

        protected override async Task<bool> SaveAsync(CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            if (_user == null || _code == null || !IsDirty)
            {
                return !IsDirty;
            }

            _user.Profile = _user.Profile ?? new UserProfile();
            _language = _language ?? new Language();
            _language.Code = _code;
            _language.Name = _language.Code.Name;
            _language.Niveau = (LanguageNiveau)SelectedLanguageNiveau!.Data!;

            if (!_user.Profile.LanguageSkills.Contains(_language))
            {
                _user.Profile.LanguageSkills.Add(_language);
            }

            var response = await UserService.SaveAsync(_user, token).ConfigureAwait(false);
            if (string.IsNullOrWhiteSpace(response))
            {
                Logger.LogError($"Unable to save user remotely {nameof(SaveAsync)} in {GetType().Name}.");
                return !IsDirty;
            }

            var userResult = await UserService.GetUserAsync(_user.Id, token).ConfigureAwait(false);
            if (userResult == null || !await UserRepository.SaveAsync(userResult, CancellationToken.None).ConfigureAwait(false))
            {
                Logger.LogError($"Unable to save user locally {nameof(SaveAsync)} in {GetType().Name}.");
                return !IsDirty;
            }

            _user = userResult;
            IsDirty = false;
            return !IsDirty;
        }

        partial void OnLanguageNameChanged(string? value)
        {
            ValidateProperty(value, nameof(LanguageName));
            IsDirty = true;
        }

        partial void OnSelectedLanguageNiveauChanged(InteractionEntry? value)
        {
            ValidateProperty(value, nameof(SelectedLanguageNiveau));
            IsDirty = true;
        }

        private void LoadonUIThread(User? user, Language? language, CultureInfo? code, string? name, List<InteractionEntry> languageNiveaus, InteractionEntry? selectedNivau, bool isDirty)
        {
            _user = user;
            _language = language;
            _code = code;
            LanguageName = name;
            LanguageNiveaus = new ObservableCollection<InteractionEntry>(languageNiveaus);
            SelectedLanguageNiveau = selectedNivau ?? LanguageNiveaus.FirstOrDefault();
            IsDirty = isDirty;
            ValidateCommand?.Execute(null);
        }

        [RelayCommand(AllowConcurrentExecutions = false, CanExecute = nameof(CanSearchLanguage))]
        private async Task SearchLanguage(CancellationToken token)
        {
            using (var worker = ScheduleWork(token))
            {
                try
                {
                    var language = new Language();
                    language.Id = _languageId;
                    language.Niveau = SelectedLanguageNiveau?.Data != null ? (LanguageNiveau)SelectedLanguageNiveau.Data : null;
                    language.Name = _code?.Name ?? string.Empty;

                    var data = language.Serialize();

                    // store state
                    _editState = data;
                    var parameters = new NavigationParameters();
                    parameters.AddValue(NavigationParameter.Data, data);
                    await NavigationService.NavigateAsync(Routes.LanguageSearchView, token, parameters);
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

        [RelayCommand(AllowConcurrentExecutions = false, CanExecute = nameof(CanDelete))]
        private async Task Delete(CancellationToken token)
        {
            using (var worker = ScheduleWork(token))
            {
                try
                {
                    _user!.Profile!.LanguageSkills.Remove(_language!);
                    var response = await UserService.SaveAsync(_user, worker.Token).ConfigureAwait(false);
                    if (string.IsNullOrWhiteSpace(response))
                    {
                        Logger.LogError($"Unable to delete language remotely {nameof(Delete)} in {GetType().Name}.");
                        await ShowErrorAsync(Resources.Strings.Resources.GlobalError_UnableToSaveData, worker.Token).ConfigureAwait(false);
                        return;
                    }

                    if (!await UserRepository.SaveAsync(_user, CancellationToken.None).ConfigureAwait(false))
                    {
                        Logger.LogError($"Unable to save language locally {nameof(Delete)} in {GetType().Name}.");
                        await ShowErrorAsync(Resources.Strings.Resources.GlobalError_UnableToSaveData, worker.Token).ConfigureAwait(false);
                        return;
                    }

                    IsDirty = false;
                    await NavigationService.PopAsync(worker.Token).ConfigureAwait(false);
                }
                catch (OperationCanceledException)
                {
                    Logger?.LogDebug($"Canceled {nameof(Delete)} in {GetType().Name}.");
                }
                catch (ObjectDisposedException)
                {
                    Logger?.LogDebug($"Canceled {nameof(Delete)} in {GetType().Name}.");
                }
                catch (Exception ex)
                {
                    Logger?.LogError(ex, $"Unknown error in {nameof(Delete)} in {GetType().Name}.");
                }
                finally
                {
                    UnscheduleWork(worker);
                }
            }
        }

        private bool CanDelete()
        {
            return !IsBusy && _user?.Profile != null && _language != null;
        }
    }
}
