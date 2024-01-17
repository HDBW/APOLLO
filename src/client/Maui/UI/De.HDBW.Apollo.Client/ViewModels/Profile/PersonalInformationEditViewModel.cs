// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using De.HDBW.Apollo.Client.Contracts;
using De.HDBW.Apollo.SharedContracts.Repositories;
using Invite.Apollo.App.Graph.Common.Models.UserProfile;
using Microsoft.Extensions.Logging;

namespace De.HDBW.Apollo.Client.ViewModels.Profile
{
    public partial class PersonalInformationEditViewModel : BaseViewModel
    {
        [ObservableProperty]
        private string? _name;

        [ObservableProperty]
        private DateTime? _birthDate;

        [ObservableProperty]
        private bool? _disabilities;

        private User? _user;

        public PersonalInformationEditViewModel(
            IDispatcherService dispatcherService,
            INavigationService navigationService,
            IDialogService dialogService,
            ILogger<PersonalInformationEditViewModel> logger,
            IUserRepository userRepository)
            : base(dispatcherService, navigationService, dialogService, logger)
        {
            ArgumentNullException.ThrowIfNull(userRepository);
            UserRepository = userRepository;
        }

        private IUserRepository UserRepository { get; }

        public override async Task OnNavigatedToAsync()
        {
            using (var worker = ScheduleWork())
            {
                try
                {
                    var sections = new List<ObservableObject>();
                    var user = await UserRepository.GetItemAsync(worker.Token).ConfigureAwait(false);
                    await ExecuteOnUIThreadAsync( () => LoadonUIThread(user), worker.Token);
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

        public override async Task OnNavigatingFromAsync()
        {
            if (_user == null)
            {
                return;
            }

            using (var worker = ScheduleWork())
            {
                try
                {
                    _user.Birthdate = BirthDate != null ? new DateTime(BirthDate.Value.Year, BirthDate.Value.Month, BirthDate.Value.Day, 0, 0, 0, DateTimeKind.Utc) : null;
                    _user.Name = Name ?? string.Empty;
                    _user.Disabilities = Disabilities;
                    if (!await UserRepository.SaveAsync(_user, worker.Token).ConfigureAwait(false))
                    {

                    }
                }
                catch (OperationCanceledException)
                {
                    Logger?.LogDebug($"Canceled {nameof(OnNavigatingFromAsync)} in {GetType().Name}.");
                }
                catch (ObjectDisposedException)
                {
                    Logger?.LogDebug($"Canceled {nameof(OnNavigatingFromAsync)} in {GetType().Name}.");
                }
                catch (Exception ex)
                {
                    Logger?.LogError(ex, $"Unknown error while {nameof(OnNavigatingFromAsync)} in {GetType().Name}.");
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
            ToggleDisabilitiesCommand?.NotifyCanExecuteChanged();
        }

        [RelayCommand(AllowConcurrentExecutions = false, CanExecute = nameof(CanToggleDisabilities))]
        private Task ToggleDisabilities(CancellationToken token)
        {
            Disabilities = !Disabilities;
            return Task.CompletedTask;
        }

        private bool CanToggleDisabilities()
        {
            return !IsBusy;
        }

        private void LoadonUIThread(User? user)
        {
            _user = user;
            Name = user?.Name;
            BirthDate = user?.Birthdate != null ? new DateTime(user.Birthdate.Value.Year, user.Birthdate.Value.Month, user.Birthdate.Value.Day, 0, 0, 0, DateTimeKind.Local) : null;
            Disabilities = user?.Disabilities ?? false;
        }
    }
}
