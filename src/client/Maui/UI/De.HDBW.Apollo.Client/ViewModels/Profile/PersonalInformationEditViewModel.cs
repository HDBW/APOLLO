// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.ComponentModel.DataAnnotations;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using De.HDBW.Apollo.Client.Contracts;
using De.HDBW.Apollo.SharedContracts.Repositories;
using De.HDBW.Apollo.SharedContracts.Services;
using Invite.Apollo.App.Graph.Common.Models.UserProfile;
using Microsoft.Extensions.Logging;

namespace De.HDBW.Apollo.Client.ViewModels.Profile
{
    public partial class PersonalInformationEditViewModel : AbstractSaveDataViewModel
    {
        private string? _name;

        private DateTime? _birthDate;

        private bool _disabilities;

        private User? _user;

        public PersonalInformationEditViewModel(
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

        [Required(ErrorMessageResourceType = typeof(Resources.Strings.Resources), ErrorMessageResourceName = nameof(Resources.Strings.Resources.GlobalError_NameIsRequired))]
        [MinLength(4, ErrorMessageResourceType = typeof(Resources.Strings.Resources), ErrorMessageResourceName = nameof(Resources.Strings.Resources.GlobalError_NameMinLength))]
        public string? Name
        {
            get
            {
                return _name;
            }

            set
            {
                value = value?.Replace(" ", string.Empty);
                if (SetProperty(ref _name, value))
                {
                    IsDirty = true;
                }
            }
        }

        public DateTime? BirthDate
        {
            get
            {
                return _birthDate;
            }

            set
            {
                if (SetProperty(ref _birthDate, value))
                {
                    IsDirty = true;
                }
            }
        }

        public bool Disabilities
        {
            get
            {
                return _disabilities;
            }

            set
            {
                if (SetProperty(ref _disabilities, value))
                {
                    IsDirty = true;
                }
            }
        }

        private IUserRepository UserRepository { get; }

        private IUserService UserService { get; }

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

        protected override void RefreshCommands()
        {
            base.RefreshCommands();
            ToggleDisabilitiesCommand?.NotifyCanExecuteChanged();
        }

        protected override async Task<bool> SaveAsync(CancellationToken token)
        {
            if (_user == null || !IsDirty)
            {
                return !IsDirty;
            }

            token.ThrowIfCancellationRequested();
            _user.Birthdate = BirthDate != null ? new DateTime(BirthDate.Value.Year, BirthDate.Value.Month, BirthDate.Value.Day, 0, 0, 0, DateTimeKind.Utc) : null;
            _user.Name =  Name ?? string.Empty;
            _user.Disabilities = Disabilities;
            var response = await UserService.SaveAsync(_user, token).ConfigureAwait(false);
            if (string.IsNullOrWhiteSpace(response))
            {
                Logger.LogError($"Unable to save user remotely {nameof(SaveAsync)} in {GetType().Name}.");
                return !IsDirty;
            }

            _user.Id = response;
            if (!await UserRepository.SaveAsync(_user, token).ConfigureAwait(false))
            {
                Logger.LogError($"Unable to save user locally {nameof(SaveAsync)} in {GetType().Name}.");
                return !IsDirty;
            }

            IsDirty = false;
            return !IsDirty;
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
            IsDirty = false;
            ValidateCommand?.Execute(null);
        }
    }
}
