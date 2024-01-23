// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.ComponentModel.DataAnnotations;
using CommunityToolkit.Mvvm.Input;
using De.HDBW.Apollo.Client.Contracts;
using De.HDBW.Apollo.Client.Helper;
using De.HDBW.Apollo.Client.Models;
using De.HDBW.Apollo.SharedContracts.Repositories;
using De.HDBW.Apollo.SharedContracts.Services;
using Invite.Apollo.App.Graph.Common.Models.UserProfile;
using Microsoft.Extensions.Logging;
using UserProfile = Invite.Apollo.App.Graph.Common.Models.UserProfile.Profile;

namespace De.HDBW.Apollo.Client.ViewModels.Profile.QualificationEditors
{
    public partial class QualificationViewModel : AbstractSaveDataViewModel
    {
        private DateTime? _start = DateTime.Today;

        private DateTime? _end;

        private string? _description;

        private Qualification? _qualification;

        private User? _user;

        private string? _qualificationId;

        public QualificationViewModel(
            IDispatcherService dispatcherService,
            INavigationService navigationService,
            IDialogService dialogService,
            ILogger<QualificationViewModel> logger,
            IUserRepository userRepository,
            IUserService userService)
            : base(dispatcherService, navigationService, dialogService, logger)
        {
            UserRepository = userRepository;
            UserService = userService;
        }

        public bool HasEnd
        {
            get
            {
                return End.HasValue;
            }
        }

        [Required(ErrorMessageResourceType = typeof(Resources.Strings.Resources), ErrorMessageResourceName = nameof(Resources.Strings.Resources.GlobalError_PropertyRequired))]
        public string? Description
        {
            get
            {
                return _description;
            }

            set
            {
                if (SetProperty(ref _description, value))
                {
                    ValidateProperty(value);
                    IsDirty = true;
                }
            }
        }

        public DateTime? Start
        {
            get
            {
                return _start;
            }

            set
            {
                if (SetProperty(ref _start, value))
                {
                    IsDirty = true;
                }
            }
        }

        public DateTime? End
        {
            get
            {
                return _end;
            }

            set
            {
                if (SetProperty(ref _end, value))
                {
                    IsDirty = true;
                    OnPropertyChanged(nameof(HasEnd));
                    RefreshCommands();
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
                    var user = await UserRepository.GetItemAsync(worker.Token).ConfigureAwait(false);
                    var qualification = user?.Profile?.Qualifications?.FirstOrDefault(x => x.Id == _qualificationId);
                    await ExecuteOnUIThreadAsync(() => LoadonUIThread(user, qualification), worker.Token);
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
            _qualificationId = navigationParameters.GetValue<string?>(NavigationParameter.Id);
        }

        protected override async Task<bool> SaveAsync(CancellationToken token)
        {
            if (_user == null || !IsDirty)
            {
                return !IsDirty;
            }

            token.ThrowIfCancellationRequested();

            _user.Profile = _user.Profile ?? new UserProfile();
            var qualification = _qualification ?? new Qualification();
            qualification.Description = Description;
            qualification.ExpirationDate = End.ToDTODate();
            qualification.IssueDate = Start.ToDTODate();
            if (!_user.Profile.Qualifications.Contains(qualification))
            {
                _user.Profile.Qualifications.Add(qualification);
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

        protected override void RefreshCommands()
        {
            base.RefreshCommands();
            DeleteCommand?.NotifyCanExecuteChanged();
            ClearEndCommand?.NotifyCanExecuteChanged();
        }

        [RelayCommand(CanExecute = nameof(CanClearEnd))]
        private void ClearEnd()
        {
            End = null;
        }

        private bool CanClearEnd()
        {
            return !IsBusy && HasEnd;
        }

        private void LoadonUIThread(User? user, Qualification? qualification)
        {
            _qualification = qualification;
            _user = user;
            Description = qualification?.Description;
            Start = qualification?.IssueDate.ToUIDate();
            End = qualification?.ExpirationDate.ToUIDate();
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
                    _user!.Profile!.Qualifications.Remove(_qualification!);
                    var response = await UserService.SaveAsync(_user, worker.Token).ConfigureAwait(false);
                    if (string.IsNullOrWhiteSpace(response))
                    {
                        Logger.LogError($"Unable to delete user remotely {nameof(Delete)} in {GetType().Name}.");
                        await ShowErrorAsync(Resources.Strings.Resources.GlobalError_UnableToSaveData, worker.Token).ConfigureAwait(false);
                        return;
                    }

                    if (!await UserRepository.SaveAsync(_user, CancellationToken.None).ConfigureAwait(false))
                    {
                        Logger.LogError($"Unable to save user locally {nameof(Delete)} in {GetType().Name}.");
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
            return !IsBusy && _qualification != null;
        }
    }
}
