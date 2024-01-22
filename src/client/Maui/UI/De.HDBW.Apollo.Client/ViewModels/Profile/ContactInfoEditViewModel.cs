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
using Contact = Invite.Apollo.App.Graph.Common.Models.Contact;

namespace De.HDBW.Apollo.Client.ViewModels.Profile
{
    public partial class ContactInfoEditViewModel : BaseViewModel
    {
        [ObservableProperty]
        private ObservableCollection<ContactEntry> _contacts = new ObservableCollection<ContactEntry>();
        private User? _user;

        public ContactInfoEditViewModel(
            IDispatcherService dispatcherService,
            INavigationService navigationService,
            IDialogService dialogService,
            ILogger<PersonalInformationEditViewModel> logger,
            IUserRepository userRepository,
            IUserService userService)
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
                    _user = await UserRepository.GetItemAsync(worker.Token).ConfigureAwait(false);
                    var contacts = new List<Contact>();
                    contacts.AddRange(_user?.ContactInfos ?? new List<Contact>());
                    contacts = contacts.OrderBy(x => x.ContactType).ToList();
                    await ExecuteOnUIThreadAsync(
                        () => LoadonUIThread(contacts), worker.Token);
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
            AddCommand?.NotifyCanExecuteChanged();
            foreach (var contact in Contacts)
            {
                contact.DeleteCommand?.NotifyCanExecuteChanged();
            }

            base.RefreshCommands();
        }

        [RelayCommand(AllowConcurrentExecutions = false, CanExecute = nameof(CanAdd))]
        private async Task Add(CancellationToken token)
        {
            using (var worker = ScheduleWork(token))
            {
                try
                {
                    await NavigationService.NavigateAsync(Routes.ContactInfoContactView, worker.Token);
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

        private bool CanAdd()
        {
            return !IsBusy && _user != null;
        }

        [RelayCommand(AllowConcurrentExecutions = false, CanExecute = nameof(CanEdit))]
        private async Task Edit(ContactEntry entry, CancellationToken token)
        {
            using (var worker = ScheduleWork(token))
            {
                try
                {
                    var id = entry.Export().Id;
                    if (string.IsNullOrWhiteSpace(id))
                    {
                        return;
                    }

                    var parameter = new NavigationParameters();
                    parameter.AddValue<string>(NavigationParameter.Id, id);
                    await NavigationService.NavigateAsync(Routes.ContactInfoContactView, worker.Token, parameter);
                }
                catch (OperationCanceledException)
                {
                    Logger?.LogDebug($"Canceled {nameof(Edit)} in {GetType().Name}.");
                }
                catch (ObjectDisposedException)
                {
                    Logger?.LogDebug($"Canceled {nameof(Edit)} in {GetType().Name}.");
                }
                catch (Exception ex)
                {
                    Logger?.LogError(ex, $"Unknown error in {nameof(Edit)} in {GetType().Name}.");
                }
                finally
                {
                    UnscheduleWork(worker);
                }
            }
        }

        private bool CanEdit(ContactEntry entry)
        {
            return !IsBusy && _user != null && entry != null;
        }

        private void LoadonUIThread(List<Contact> contacts)
        {
            Contacts = new ObservableCollection<ContactEntry>(contacts.Select(x => ContactEntry.Import(x, DeleteAsync, CanDelete)));
        }

        private bool CanDelete(AbstractProfileEntry<Contact> entry)
        {
            return !IsBusy && _user != null;
        }

        private async Task DeleteAsync(AbstractProfileEntry<Contact> entry)
        {
            using (var worker = ScheduleWork())
            {
                try
                {
                    _user!.ContactInfos.Remove(entry.Export());
                    var response = await UserService.SaveAsync(_user, worker.Token).ConfigureAwait(false);
                    if (string.IsNullOrWhiteSpace(response))
                    {
                        Logger.LogError($"Unable to save user remotely {nameof(DeleteAsync)} in {GetType().Name}.");
                        await ShowErrorAsync(Resources.Strings.Resources.GlobalError_UnableToSaveData, worker.Token).ConfigureAwait(false);
                        return;
                    }

                    if (!await UserRepository.SaveAsync(_user, CancellationToken.None).ConfigureAwait(false))
                    {
                        Logger.LogError($"Unable to save user locally {nameof(DeleteAsync)} in {GetType().Name}.");
                        await ShowErrorAsync(Resources.Strings.Resources.GlobalError_UnableToSaveData, worker.Token).ConfigureAwait(false);
                        return;
                    }

                    await ExecuteOnUIThreadAsync(() => Contacts.Remove((ContactEntry)entry), CancellationToken.None);
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
    }
}
