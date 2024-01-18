// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using De.HDBW.Apollo.Client.Contracts;
using De.HDBW.Apollo.Client.Models;
using Invite.Apollo.App.Graph.Common.Models.UserProfile;
using Microsoft.Extensions.Logging;

namespace De.HDBW.Apollo.Client.ViewModels.Profile
{
    public partial class ContactInfoEditViewModel : BaseViewModel
    {
        [ObservableProperty]
        private ObservableCollection<Contact> _contacts = new ObservableCollection<Contact>();

        public ContactInfoEditViewModel(
            IDispatcherService dispatcherService,
            INavigationService navigationService,
            IDialogService dialogService,
            ILogger<PersonalInformationEditViewModel> logger)
            : base(dispatcherService, navigationService, dialogService, logger)
        {
        }

        public override async Task OnNavigatedToAsync()
        {
            using (var worker = ScheduleWork())
            {
                try
                {
                    var contacts = new List<Contact>();
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
            return !IsBusy;
        }

        private void LoadonUIThread(List<Contact> contacts)
        {
            Contacts = new ObservableCollection<Contact>(contacts);
        }
    }
}
