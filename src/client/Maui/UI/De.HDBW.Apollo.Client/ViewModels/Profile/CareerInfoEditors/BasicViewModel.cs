// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using De.HDBW.Apollo.Client.Contracts;
using De.HDBW.Apollo.Client.Helper;
using De.HDBW.Apollo.SharedContracts.Repositories;
using De.HDBW.Apollo.SharedContracts.Services;
using Invite.Apollo.App.Graph.Common.Models.UserProfile;
using Microsoft.Extensions.Logging;

namespace De.HDBW.Apollo.Client.ViewModels.Profile.CareerInfoEditors
{
    public partial class BasicViewModel : AbstractProfileEditorViewModel<CareerInfo>
    {
        [ObservableProperty]
        private DateTime _start = DateTime.Today;

        [ObservableProperty]
        private DateTime? _end;

        [ObservableProperty]
        private string? _description;

        public BasicViewModel(
            IDispatcherService dispatcherService,
            INavigationService navigationService,
            IDialogService dialogService,
            ILogger<BasicViewModel> logger,
            IUserRepository userRepository,
            IUserService userService)
            : base(dispatcherService, navigationService, dialogService, logger, userRepository, userService)
        {
        }

        public bool HasEnd
        {
            get
            {
                return End.HasValue;
            }
        }

        protected override void RefreshCommands()
        {
            base.RefreshCommands();
            ClearEndCommand?.NotifyCanExecuteChanged();
        }

        protected override async Task<CareerInfo?> LoadDataAsync(User user, string? entryId, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            var currentData = user.Profile?.CareerInfos.FirstOrDefault(x => x.Id == entryId);
            await ExecuteOnUIThreadAsync(() => LoadonUIThread(currentData), token).ConfigureAwait(false);
            return currentData;
        }

        protected override CareerInfo CreateNewEntry(User user)
        {
            var entry = new CareerInfo();
            user.Profile!.CareerInfos.Add(entry);
            return entry;
        }

        protected override void ApplyChanges(CareerInfo entity)
        {
            entity.Start = Start.ToDTODate();
            entity.End = End.ToDTODate();
            entity.Description = Description;
        }

        partial void OnEndChanged(DateTime? value)
        {
            OnPropertyChanged(nameof(HasEnd));
            RefreshCommands();
        }

        private void LoadonUIThread(CareerInfo? careerInfo)
        {
            Start = careerInfo?.Start.ToUIDate() ?? Start;
            End = careerInfo?.End.ToUIDate();
            Description = careerInfo?.Description;
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

        protected override void DeleteEntry(User user, CareerInfo tU) => throw new NotImplementedException();
    }
}
