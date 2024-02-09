// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.ComponentModel.DataAnnotations;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using De.HDBW.Apollo.Client.Contracts;
using De.HDBW.Apollo.Client.Helper;
using De.HDBW.Apollo.Client.Models;
using De.HDBW.Apollo.Data.Helper;
using De.HDBW.Apollo.SharedContracts.Repositories;
using De.HDBW.Apollo.SharedContracts.Services;
using Invite.Apollo.App.Graph.Common.Models.UserProfile;
using Invite.Apollo.App.Graph.Common.Models.UserProfile.Enums;
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
        [Required(ErrorMessageResourceType = typeof(Resources.Strings.Resources), ErrorMessageResourceName = nameof(Resources.Strings.Resources.GlobalError_PropertyRequired))]
        private string? _description;

        private CareerType? _type;

        private string? _savedState;

        private string? _selectionResult;

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

        public string Title
        {
            get
            {
                return _type.GetLocalizedString();
            }
        }

        public bool HasEnd
        {
            get
            {
                return End.HasValue;
            }
        }

        protected string? SelectionResult
        {
            get
            {
                return _selectionResult;
            }
        }

        protected CareerInfo? EditState { get; private set; }

        protected override void RefreshCommands()
        {
            base.RefreshCommands();
            ClearEndCommand?.NotifyCanExecuteChanged();
        }

        protected override async Task<CareerInfo?> LoadDataAsync(User user, string? entryId, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            var currentData = user.Profile?.CareerInfos?.FirstOrDefault(x => x.Id == entryId);
            EditState = _savedState.Deserialize<CareerInfo?>();

            var currentState = currentData.Serialize<CareerInfo?>();
            var isDirty = !string.Equals(currentState, _savedState);

            var start = currentData?.Start;
            var end = currentData?.End;
            var description = currentData?.Description;

            if (EditState != null)
            {
                start = EditState.Start;
                end = EditState.End;
                description = EditState.Description;
            }

            await ExecuteOnUIThreadAsync(() => LoadonUIThread(start, end, description, isDirty), token).ConfigureAwait(false);
            return currentData;
        }

        protected override CareerInfo CreateNewEntry(User user)
        {
            var entry = new CareerInfo();
#pragma warning disable SA1009 // Closing parenthesis should be spaced correctly
            entry.CareerType = (_type ?? CareerType.Unknown).ToApolloListItem()!;
#pragma warning restore SA1009 // Closing parenthesis should be spaced correctly
            user.Profile!.CareerInfos = user.Profile!.CareerInfos ?? new List<CareerInfo>();
            user.Profile!.CareerInfos.Add(entry);
            return entry;
        }

        protected override void DeleteEntry(User user, CareerInfo entry)
        {
            user.Profile!.CareerInfos!.Remove(entry);
        }

        protected override void OnPrepare(NavigationParameters navigationParameters)
        {
            base.OnPrepare(navigationParameters);
            _type = navigationParameters.GetValue<CareerType?>(NavigationParameter.Type);
            _savedState = navigationParameters.ContainsKey(NavigationParameter.SavedState) ? navigationParameters.GetValue<string>(NavigationParameter.SavedState) : null;
            _selectionResult = navigationParameters.ContainsKey(NavigationParameter.Result) ? navigationParameters.GetValue<string>(NavigationParameter.Result) : null;
        }

        protected override void ApplyChanges(CareerInfo entry)
        {
            entry.Start = Start.ToDTODate();
            entry.End = End.ToDTODate();
            entry.Description = Description;
        }

        partial void OnEndChanged(DateTime? value)
        {
            IsDirty = true;
            OnPropertyChanged(nameof(HasEnd));
            RefreshCommands();
        }

        partial void OnStartChanged(DateTime value)
        {
            IsDirty = true;
        }

        partial void OnDescriptionChanged(string? value)
        {
            ValidateProperty(value, nameof(Description));
            IsDirty = true;
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

        private void LoadonUIThread(DateTime? start, DateTime? end, string? description, bool isDirty)
        {
            Start = start.ToUIDate() ?? Start;
            End = end.ToUIDate();
            Description = description;
            IsDirty = isDirty;
            ValidateCommand.Execute(null);
        }
    }
}
