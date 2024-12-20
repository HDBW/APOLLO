﻿// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using CommunityToolkit.Mvvm.Input;
using De.HDBW.Apollo.Client.Contracts;
using De.HDBW.Apollo.Client.Models;
using De.HDBW.Apollo.Data.Helper;
using De.HDBW.Apollo.SharedContracts.Repositories;
using De.HDBW.Apollo.SharedContracts.Services;
using Microsoft.Extensions.Logging;
using User = Invite.Apollo.App.Graph.Common.Models.UserProfile.User;
using UserProfile = Invite.Apollo.App.Graph.Common.Models.UserProfile.Profile;

namespace De.HDBW.Apollo.Client.ViewModels.Profile
{
    public abstract partial class AbstractProfileEditorViewModel<TU> : AbstractSaveDataViewModel
    {
        private string? _entryId;

        protected AbstractProfileEditorViewModel(
            IDispatcherService dispatcherService,
            INavigationService navigationService,
            IDialogService dialogService,
            ILogger logger,
            IUserRepository userRepository,
            IUserService userService)
            : base(dispatcherService, navigationService, dialogService, logger)
        {
            ArgumentNullException.ThrowIfNull(userRepository);
            ArgumentNullException.ThrowIfNull(userService);
            UserRepository = userRepository;
            UserService = userService;
        }

        protected string? EntryId
        {
            get
            {
                return _entryId;
            }
        }

        private User? User { get; set; }

        private IUserRepository UserRepository { get; }

        private TU? Entry { get; set; }

        private IUserService UserService { get; }

        public override async Task OnNavigatedToAsync()
        {
            if (IsShowingDialog)
            {
                return;
            }

            using (var worker = ScheduleWork())
            {
                try
                {
                    User = await UserRepository.GetItemAsync(worker.Token).ConfigureAwait(false);
                    Entry = await LoadDataAsync(User!, _entryId, worker.Token).ConfigureAwait(false);
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
            _entryId = navigationParameters.GetValue<string?>(NavigationParameter.Id);
        }

        protected abstract Task<TU?> LoadDataAsync(User user, string? enityId, CancellationToken token);

        protected override async Task<bool> SaveAsync(CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            if (User == null || !IsDirty)
            {
                return !IsDirty;
            }

            User.Profile = User.Profile ?? new UserProfile();
            TU entry = Entry ?? CreateNewEntry(User);
            ApplyChanges(entry);

            var response = await UserService.SaveAsync(User, token).ConfigureAwait(false);
            if (string.IsNullOrWhiteSpace(response))
            {
                Logger.LogError($"Unable to save {nameof(TU)} remotely {nameof(SaveAsync)} in {GetType().Name}.");
                return !IsDirty;
            }

            User.Id = response;
            var userResult = await UserService.GetAsync(User.Id, token).ConfigureAwait(false);
            if (userResult == null || !await UserRepository.SaveAsync(userResult, CancellationToken.None).ConfigureAwait(false))
            {
                Logger.LogError($"Unable to save {nameof(TU)} locally {nameof(SaveAsync)} in {GetType().Name}.");
                return !IsDirty;
            }

            IsDirty = false;
            return !IsDirty;
        }

        protected abstract TU CreateNewEntry(User user);

        protected abstract void DeleteEntry(User user, TU entry);

        protected abstract void ApplyChanges(TU entry);

        protected string? GetCurrentState()
        {
            var model = Entry ?? Activator.CreateInstance<TU>();
            ApplyChanges(model);
            return model.Serialize();
        }

        protected override void RefreshCommands()
        {
            base.RefreshCommands();
            DeleteCommand?.NotifyCanExecuteChanged();
        }

        [RelayCommand(AllowConcurrentExecutions = false, CanExecute = nameof(CanDelete))]
        private async Task Delete(CancellationToken token)
        {
            Logger.LogInformation($"Invoked {nameof(DeleteCommand)} in {GetType().Name}.");
            using (var worker = ScheduleWork(token))
            {
                try
                {
                    var user = User!;
                    var entry = Entry!;

                    DeleteEntry(user, entry!);
                    Entry = default;

                    var response = await UserService.SaveAsync(user, worker.Token).ConfigureAwait(false);
                    if (string.IsNullOrWhiteSpace(response))
                    {
                        Logger.LogError($"Unable to {nameof(Delete)} in {GetType().Name}.");
                        await ShowErrorAsync(Resources.Strings.Resources.GlobalError_UnableToSaveData, worker.Token).ConfigureAwait(false);
                        return;
                    }

                    if (!await UserRepository.SaveAsync(user, CancellationToken.None).ConfigureAwait(false))
                    {
                        Logger.LogError($"Unable to save contact locally {nameof(Delete)} in {GetType().Name}.");
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
                    await ShowErrorAsync(Resources.Strings.Resources.GlobalError_UnableToSaveData, worker.Token).ConfigureAwait(false);
                }
                finally
                {
                    UnscheduleWork(worker);
                }
            }
        }

        private bool CanDelete()
        {
            return !IsBusy && User != null && Entry != null;
        }
    }
}
