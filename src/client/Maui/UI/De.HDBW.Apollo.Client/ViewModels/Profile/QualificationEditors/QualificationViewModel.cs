// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using CommunityToolkit.Mvvm.Input;
using De.HDBW.Apollo.Client.Contracts;
using De.HDBW.Apollo.Client.Models;
using De.HDBW.Apollo.SharedContracts.Repositories;
using De.HDBW.Apollo.SharedContracts.Services;
using Invite.Apollo.App.Graph.Common.Models.UserProfile;
using Microsoft.Extensions.Logging;

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
                    if (user != null && user.Profile == null)
                    {
                        user.Profile = new Invite.Apollo.App.Graph.Common.Models.UserProfile.Profile();
                    }

                    await ExecuteOnUIThreadAsync(() => LoadonUIThread(user, qualification ?? new Qualification()), worker.Token);
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
            if (_user == null || _user.Profile == null || _qualification == null || !IsDirty)
            {
                return !IsDirty;
            }

            token.ThrowIfCancellationRequested();
            _qualification.Description = Description ?? string.Empty;
            _qualification.ExpirationDate = End != null ? new DateTime(End.Value.Year, End.Value.Month, End.Value.Day, 0, 0, 0, DateTimeKind.Utc) : null;
            _qualification.IssueDate = Start != null ? new DateTime(Start.Value.Year, Start.Value.Month, Start.Value.Day, 0, 0, 0, DateTimeKind.Utc) : null;
            var toUpdate = _user?.Profile?.Qualifications?.FirstOrDefault(f => f.Id == _qualification.Id);
            if (toUpdate != null)
            {
                toUpdate = _qualification;
            }
            else
            {
                _user.Profile.Qualifications = new List<Qualification>();
                _user.Profile.Qualifications.Add(_qualification);
            }

            var response = await UserService.SaveAsync(_user, token).ConfigureAwait(false);
            if (string.IsNullOrWhiteSpace(response))
            {
                Logger.LogError($"Unable to save user remotely {nameof(SaveAsync)} in {GetType().Name}.");
                return !IsDirty;
            }

            _user.Id = response;
            var userResult = await UserService.GetUserAsync(_user.Id, token).ConfigureAwait(false);
            if (userResult == null || !await UserRepository.SaveAsync(userResult, CancellationToken.None).ConfigureAwait(false))
            {
                Logger.LogError($"Unable to save user locally {nameof(SaveAsync)} in {GetType().Name}.");
                return !IsDirty;
            }

            IsDirty = false;
            return !IsDirty;
        }

        protected override void RefreshCommands()
        {
            base.RefreshCommands();
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
            Start = qualification?.IssueDate;
            End = qualification?.ExpirationDate;
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
            return !IsBusy && _user != null && _user.Profile != null && _qualification != null;
        }
    }
}
