// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.ComponentModel.DataAnnotations;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using De.HDBW.Apollo.Client.Contracts;
using De.HDBW.Apollo.Client.Models;
using De.HDBW.Apollo.SharedContracts.Repositories;
using De.HDBW.Apollo.SharedContracts.Services;
using Invite.Apollo.App.Graph.Common.Models.UserProfile;
using Microsoft.Extensions.Logging;
using UserProfile = Invite.Apollo.App.Graph.Common.Models.UserProfile.Profile;

namespace De.HDBW.Apollo.Client.ViewModels.Profile.WebReferenceEditors
{
    public partial class WebReferenceViewModel : AbstractSaveDataViewModel
    {
        [ObservableProperty]
        [Required(ErrorMessageResourceType = typeof(Resources.Strings.Resources), ErrorMessageResourceName = nameof(Resources.Strings.Resources.GlobalError_PropertyRequired))]
        private string? _description;

        [ObservableProperty]
        [Required(ErrorMessageResourceType = typeof(Resources.Strings.Resources), ErrorMessageResourceName = nameof(Resources.Strings.Resources.GlobalError_PropertyRequired))]
        [Url(ErrorMessageResourceType = typeof(Resources.Strings.Resources), ErrorMessageResourceName = nameof(Resources.Strings.Resources.GlobalError_InvalidUrl))]
        private string? _url;

        private WebReference? _webReference;

        private User? _user;

        private string? _webReferenceId;

        public WebReferenceViewModel(
            IDispatcherService dispatcherService,
            INavigationService navigationService,
            IDialogService dialogService,
            ILogger<WebReferenceViewModel> logger,
            IUserRepository userRepository,
            IUserService userServic)
            : base(dispatcherService, navigationService, dialogService, logger)
        {
            UserRepository = userRepository;
            UserService = userServic;
        }

        private IUserRepository UserRepository { get; }

        private IUserService UserService { get; }

        public override async Task OnNavigatedToAsync()
        {
            using (var worker = ScheduleWork())
            {
                try
                {
                    var user = await UserRepository.GetItemAsync(worker.Token).ConfigureAwait(false);
                    var webReference = user?.Profile?.WebReferences?.FirstOrDefault(x => x.Id == _webReferenceId);
                    await ExecuteOnUIThreadAsync(() => LoadonUIThread(user, webReference), worker.Token);
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
            _webReferenceId = navigationParameters.GetValue<string?>(NavigationParameter.Id);
        }

        protected override async Task<bool> SaveAsync(CancellationToken token)
        {
            if (_user == null || !IsDirty)
            {
                return !IsDirty;
            }

            token.ThrowIfCancellationRequested();

            _user.Profile = _user.Profile ?? new UserProfile();
            var webReference = _webReference ?? new WebReference();
            webReference.Url = new Uri(Url?.Trim() ?? "about:blank");
            webReference.Title = Description?.Trim() ?? string.Empty;

            if (!_user.Profile.WebReferences.Contains(webReference))
            {
                _user.Profile.WebReferences.Add(webReference);
            }

            var response = await UserService.SaveAsync(_user, token).ConfigureAwait(false);
            if (string.IsNullOrWhiteSpace(response))
            {
                Logger.LogError($"Unable to save webReference remotely {nameof(SaveAsync)} in {GetType().Name}.");
                return !IsDirty;
            }

            _user.Id = response;
            var userResult = await UserService.GetUserAsync(_user.Id, token).ConfigureAwait(false);
            if (userResult == null || !await UserRepository.SaveAsync(userResult, CancellationToken.None).ConfigureAwait(false))
            {
                Logger.LogError($"Unable to save webReference locally {nameof(SaveAsync)} in {GetType().Name}.");
                return !IsDirty;
            }

            IsDirty = false;
            return !IsDirty;
        }

        protected override void RefreshCommands()
        {
            base.RefreshCommands();
            DeleteCommand?.NotifyCanExecuteChanged();
        }

        partial void OnUrlChanging(string? value)
        {
            value = string.IsNullOrWhiteSpace(value) ? null : value?.Trim();
        }

        partial void OnUrlChanged(string? value)
        {
            ValidateProperty(value, nameof(Url));
            IsDirty = true;
        }

        partial void OnDescriptionChanging(string? value)
        {
            value = string.IsNullOrWhiteSpace(value) ? null : value;
        }

        partial void OnDescriptionChanged(string? value)
        {
            IsDirty = true;
        }

        private void LoadonUIThread(User? user, WebReference? webReference)
        {
            _webReference = webReference;
            _user = user;
            Url = webReference?.Url?.AbsoluteUri;
            Description = webReference?.Title;
            IsDirty = false;
            ValidateCommand?.Execute(null);
        }

        [RelayCommand(AllowConcurrentExecutions = false, CanExecute = nameof(CanDelete))]
        private async Task Delete(CancellationToken token)
        {
            using (var worker = ScheduleWork(token))
            {
                try
                {
                    _user!.Profile!.WebReferences.Remove(_webReference!);
                    var response = await UserService.SaveAsync(_user, worker.Token).ConfigureAwait(false);
                    if (string.IsNullOrWhiteSpace(response))
                    {
                        Logger.LogError($"Unable to delete webReference remotely {nameof(Delete)} in {GetType().Name}.");
                        await ShowErrorAsync(Resources.Strings.Resources.GlobalError_UnableToSaveData, worker.Token).ConfigureAwait(false);
                        return;
                    }

                    if (!await UserRepository.SaveAsync(_user, CancellationToken.None).ConfigureAwait(false))
                    {
                        Logger.LogError($"Unable to save webReference locally {nameof(Delete)} in {GetType().Name}.");
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
            return !IsBusy && _webReference != null;
        }
    }
}
