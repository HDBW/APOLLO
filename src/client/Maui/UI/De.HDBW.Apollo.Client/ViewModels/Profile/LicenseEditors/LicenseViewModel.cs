// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.ComponentModel.DataAnnotations;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using De.HDBW.Apollo.Client.Contracts;
using De.HDBW.Apollo.Client.Helper;
using De.HDBW.Apollo.SharedContracts.Repositories;
using De.HDBW.Apollo.SharedContracts.Services;
using Invite.Apollo.App.Graph.Common.Models.UserProfile;
using Microsoft.Extensions.Logging;

namespace De.HDBW.Apollo.Client.ViewModels.Profile.LicenseEditors
{
    public partial class LicenseViewModel : AbstractProfileEditorViewModel<License>
    {
        [ObservableProperty]
        private DateTime? _start;

        [ObservableProperty]
        private DateTime? _end;

        [ObservableProperty]
        [Required(ErrorMessageResourceType = typeof(Resources.Strings.Resources), ErrorMessageResourceName = nameof(Resources.Strings.Resources.GlobalError_PropertyRequired))]
        private string? _name;

        public LicenseViewModel(
            IDispatcherService dispatcherService,
            INavigationService navigationService,
            IDialogService dialogService,
            ILogger<LicenseViewModel> logger,
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

        public bool HasStart
        {
            get
            {
                return Start.HasValue;
            }
        }

        protected override void RefreshCommands()
        {
            base.RefreshCommands();
            ClearEndCommand?.NotifyCanExecuteChanged();
            ClearStartCommand?.NotifyCanExecuteChanged();
        }

        protected override async Task<License?> LoadDataAsync(User user, string? enityId, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            var entry = user.Profile?.Licenses?.FirstOrDefault(x => x.Id == enityId);
            await ExecuteOnUIThreadAsync(() => LoadonUIThread(entry), token).ConfigureAwait(false);
            return entry;
        }

        protected override License CreateNewEntry(User user)
        {
            var entry = new License();
            user.Profile!.Licenses.Add(entry);
            return entry;
        }

        protected override void DeleteEntry(User user, License entry)
        {
            user.Profile!.Licenses.Remove(entry);
        }

        protected override void ApplyChanges(License entity)
        {
            entity.Name = Name?.Trim() ?? string.Empty;
            entity.Expires = End.ToDTODate();
            entity.Granted = Start.ToDTODate();
        }

        partial void OnNameChanging(string? value)
        {
            value = string.IsNullOrWhiteSpace(value) ? null : value;
        }

        partial void OnNameChanged(string? value)
        {
            IsDirty = true;
            ValidateProperty(value, nameof(Name));
        }

        partial void OnStartChanged(DateTime? value)
        {
            IsDirty = true;
            OnPropertyChanged(nameof(HasStart));
            RefreshCommands();
        }

        partial void OnEndChanged(DateTime? value)
        {
            IsDirty = true;
            OnPropertyChanged(nameof(HasEnd));
            RefreshCommands();
        }

        [RelayCommand(CanExecute = nameof(CanClearStart))]
        private void ClearStart()
        {
            Start = null;
        }

        private bool CanClearStart()
        {
            return !IsBusy && HasStart;
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

        private void LoadonUIThread(License? license)
        {
            Name = license?.Name;
            Start = license?.Granted.ToUIDate();
            End = license?.Expires.ToUIDate();
            IsDirty = false;
            ValidateCommand?.Execute(null);
        }
    }
}
