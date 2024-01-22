// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using CommunityToolkit.Mvvm.ComponentModel;
using De.HDBW.Apollo.Client.Contracts;
using De.HDBW.Apollo.SharedContracts.Repositories;
using De.HDBW.Apollo.SharedContracts.Services;
using Invite.Apollo.App.Graph.Common.Models.UserProfile;
using Microsoft.Extensions.Logging;

namespace De.HDBW.Apollo.Client.ViewModels.Profile.QualificationEditors
{
    public partial class QualificationViewModel : AbstractSaveDataViewModel
    {
        [ObservableProperty]
        private DateTime _start = DateTime.Today;

        [ObservableProperty]
        private DateTime? _end;

        private string? _description;

        private Qualification? _qualification;

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

        private IUserRepository UserRepository { get; }

        private IUserService UserService { get; }

        public override async Task OnNavigatedToAsync()
        {
            using (var worker = ScheduleWork())
            {
                try
                {
                    var user = await UserRepository.GetItemAsync(worker.Token).ConfigureAwait(false);
                    var tempQualification = user?.Profile?.Qualifications.FirstOrDefault();

                    await ExecuteOnUIThreadAsync(() => LoadonUIThread(tempQualification), worker.Token);
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

        protected override async Task<bool> SaveAsync(CancellationToken token)
        {
            if (_qualification == null || !IsDirty)
            {
                return !IsDirty;
            }

            token.ThrowIfCancellationRequested();

            IsDirty = false;
            return !IsDirty;
        }

        private void LoadonUIThread(Qualification? qualification)
        {
            _qualification = qualification;
            Description = qualification?.Description;

            IsDirty = false;
            ValidateCommand?.Execute(null);
        }
    }
}
