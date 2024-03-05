// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.ComponentModel.DataAnnotations;
using CommunityToolkit.Mvvm.Input;
using De.HDBW.Apollo.Client.Contracts;
using De.HDBW.Apollo.Client.Helper;
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
                    OnPropertyChanged(nameof(HasBirthDate));
                    IsDirty = true;
                    RefreshCommands();
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

        public bool HasBirthDate
        {
            get
            {
                return BirthDate.HasValue;
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
                    var user = await UserRepository.GetItemAsync(worker.Token).ConfigureAwait(false);
                    await ExecuteOnUIThreadAsync(() => LoadonUIThread(user), worker.Token);
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
            ClearBirthDateCommand?.NotifyCanExecuteChanged();
        }

        protected override async Task<bool> SaveAsync(CancellationToken token)
        {
            if (_user == null || !IsDirty)
            {
                return !IsDirty;
            }

            token.ThrowIfCancellationRequested();
            _user.Birthdate = BirthDate.ToDTODate();
            _user.Name = Name ?? string.Empty;
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
            Logger.LogInformation($"Invoked {nameof(ToggleDisabilitiesCommand)} in {GetType().Name}.");
            Disabilities = !Disabilities;
            return Task.CompletedTask;
        }

        private bool CanToggleDisabilities()
        {
            return !IsBusy;
        }

        [RelayCommand(CanExecute = nameof(CanClearBirthDate))]
        private void ClearBirthDate()
        {
            Logger.LogInformation($"Invoked {nameof(ClearBirthDateCommand)} in {GetType().Name}.");
            BirthDate = null;
        }

        private bool CanClearBirthDate()
        {
            return !IsBusy && HasBirthDate;
        }

        private void LoadonUIThread(User? user)
        {
            _user = user;
            Name = user?.Name;
            BirthDate = user?.Birthdate.ToUIDate();
            Disabilities = user?.Disabilities ?? false;
            IsDirty = false;
            ValidateCommand?.Execute(null);
        }
    }
}
