// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using De.HDBW.Apollo.Client.Contracts;
using De.HDBW.Apollo.Client.Models;
using De.HDBW.Apollo.Client.Models.Profile;
using De.HDBW.Apollo.SharedContracts.Repositories;
using De.HDBW.Apollo.SharedContracts.Services;
using Invite.Apollo.App.Graph.Common.Models.UserProfile;
using Microsoft.Extensions.Logging;

namespace De.HDBW.Apollo.Client.ViewModels.Profile
{
    public abstract partial class AbstractListViewModel<TU, TV> : BaseViewModel
        where TU : AbstractProfileEntry<TV>
    {
        [ObservableProperty]
        private ObservableCollection<TU> _items = new ObservableCollection<TU>();

        public AbstractListViewModel(
            IDispatcherService dispatcherService,
            INavigationService navigationService,
            IDialogService dialogService,
            ILogger logger,
            IUserRepository userRepository,
            IUserService userService,
            string editRoute)
            : base(dispatcherService, navigationService, dialogService, logger)
        {
            ArgumentNullException.ThrowIfNull(userRepository);
            ArgumentNullException.ThrowIfNull(userService);
            ArgumentNullException.ThrowIfNullOrWhiteSpace(editRoute);
            UserRepository = userRepository;
            UserService = userService;
            EditRoute = editRoute;
        }

        protected string EditRoute { get; }

        protected IUserRepository UserRepository { get; }

        protected IUserService UserService { get; }

        protected User? User { get; set; }

        protected override void RefreshCommands()
        {
            AddCommand?.NotifyCanExecuteChanged();
            foreach (var item in Items)
            {
                item.DeleteCommand?.NotifyCanExecuteChanged();
                item.EditCommand?.NotifyCanExecuteChanged();
            }

            base.RefreshCommands();
        }

        [RelayCommand(AllowConcurrentExecutions = false, CanExecute = nameof(CanAdd))]
        protected async Task Add(CancellationToken token)
        {
            using (var worker = ScheduleWork(token))
            {
                try
                {
                    await NavigationService.NavigateAsync(EditRoute, worker.Token);
                }
                catch (OperationCanceledException)
                {
                    Logger?.LogDebug($"Canceled {nameof(Add)} in {GetType().Name}.");
                }
                catch (ObjectDisposedException)
                {
                    Logger?.LogDebug($"Canceled {nameof(Add)} in {GetType().Name}.");
                }
                catch (Exception ex)
                {
                    Logger?.LogError(ex, $"Unknown error in {nameof(Add)} in {GetType().Name}.");
                }
                finally
                {
                    UnscheduleWork(worker);
                }
            }
        }

        protected bool CanAdd()
        {
            return !IsBusy && User != null;
        }

        protected void LoadonUIThread(IEnumerable<TU> items)
        {
            Items = new ObservableCollection<TU>(items);
        }

        protected bool CanDelete(AbstractProfileEntry<TV> entry)
        {
            return !IsBusy && User != null && entry != null;
        }

        protected async Task DeleteAsync(AbstractProfileEntry<TV> entry)
        {
            using (var worker = ScheduleWork())
            {
                try
                {
                    var user = User!;
                    RemoveItemFromUser(user, entry);
                    var response = await UserService.SaveAsync(user, worker.Token).ConfigureAwait(false);
                    if (string.IsNullOrWhiteSpace(response))
                    {
                        Logger.LogError($"Unable to save user remotely {nameof(DeleteAsync)} in {GetType().Name}.");
                        await ShowErrorAsync(Resources.Strings.Resources.GlobalError_UnableToSaveData, worker.Token).ConfigureAwait(false);
                        return;
                    }

                    if (!await UserRepository.SaveAsync(user, CancellationToken.None).ConfigureAwait(false))
                    {
                        Logger.LogError($"Unable to save user locally {nameof(DeleteAsync)} in {GetType().Name}.");
                        await ShowErrorAsync(Resources.Strings.Resources.GlobalError_UnableToSaveData, worker.Token).ConfigureAwait(false);
                        return;
                    }

                    await ExecuteOnUIThreadAsync(() => Items.Remove((TU)entry), CancellationToken.None);
                }
                catch (OperationCanceledException)
                {
                    Logger?.LogDebug($"Canceled {nameof(DeleteAsync)} in {GetType().Name}.");
                }
                catch (ObjectDisposedException)
                {
                    Logger?.LogDebug($"Canceled {nameof(DeleteAsync)} in {GetType().Name}.");
                }
                catch (Exception ex)
                {
                    Logger?.LogError(ex, $"Unknown error in {nameof(DeleteAsync)} in {GetType().Name}.");
                }
                finally
                {
                    UnscheduleWork(worker);
                }
            }
        }

        protected abstract void RemoveItemFromUser(User user, AbstractProfileEntry<TV> entry);

        protected abstract string? GetIdFromItem(AbstractProfileEntry<TV> entry);

        protected bool CanEdit(AbstractProfileEntry<TV> entry)
        {
            return !IsBusy && User != null && entry != null;
        }

        protected async Task EditAsync(AbstractProfileEntry<TV> entry)
        {
            using (var worker = ScheduleWork())
            {
                try
                {
                    var id = GetIdFromItem(entry);
                    if (string.IsNullOrWhiteSpace(id))
                    {
                        return;
                    }

                    var parameter = new NavigationParameters();
                    parameter.AddValue<string>(NavigationParameter.Id, id);
                    await NavigationService.NavigateAsync(EditRoute, worker.Token, parameter);
                }
                catch (OperationCanceledException)
                {
                    Logger?.LogDebug($"Canceled {nameof(EditAsync)} in {GetType().Name}.");
                }
                catch (ObjectDisposedException)
                {
                    Logger?.LogDebug($"Canceled {nameof(EditAsync)} in {GetType().Name}.");
                }
                catch (Exception ex)
                {
                    Logger?.LogError(ex, $"Unknown error in {nameof(EditAsync)} in {GetType().Name}.");
                }
                finally
                {
                    UnscheduleWork(worker);
                }
            }
        }
    }
}
