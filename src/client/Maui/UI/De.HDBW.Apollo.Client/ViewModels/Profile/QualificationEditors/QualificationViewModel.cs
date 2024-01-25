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

namespace De.HDBW.Apollo.Client.ViewModels.Profile.QualificationEditors
{
    public partial class QualificationViewModel : AbstractProfileEditorViewModel<Qualification>
    {
        private DateTime? _start;

        private DateTime? _end;

        private string? _description;

        private string? _name;

        public QualificationViewModel(
            IDispatcherService dispatcherService,
            INavigationService navigationService,
            IDialogService dialogService,
            ILogger<QualificationViewModel> logger,
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

        [Required(ErrorMessageResourceType = typeof(Resources.Strings.Resources), ErrorMessageResourceName = nameof(Resources.Strings.Resources.GlobalError_PropertyRequired))]
        public string? Name
        {
            get
            {
                return _name;
            }

            set
            {
                if (SetProperty(ref _name, value))
                {
                    ValidateProperty(value);
                    IsDirty = true;
                }
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
                    OnPropertyChanged(nameof(HasStart));
                    RefreshCommands();
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

        protected override void RefreshCommands()
        {
            base.RefreshCommands();
            ClearEndCommand?.NotifyCanExecuteChanged();
            ClearStartCommand?.NotifyCanExecuteChanged();
        }

        protected override async Task<Qualification?> LoadDataAsync(User user, string? enityId, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            var enity = user?.Profile?.Qualifications?.FirstOrDefault(x => x.Id == enityId);
            await ExecuteOnUIThreadAsync(() => LoadonUIThread(enity), token).ConfigureAwait(false);
            return enity;
        }

        protected override Qualification CreateNewEntry(User user)
        {
            var entry = new Qualification();
            user.Profile!.Qualifications.Add(entry);
            return entry;
        }

        protected override void DeleteEntry(User user, Qualification entry)
        {
            user.Profile!.Qualifications.Remove(entry!);
        }

        protected override void ApplyChanges(Qualification entry)
        {
            entry.Name = Name!.Trim();
            entry.Description = Description?.Trim();
            entry.ExpirationDate = End.ToDTODate();
            entry.IssueDate = Start.ToDTODate();
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

        [RelayCommand(CanExecute = nameof(CanClearStart))]
        private void ClearStart()
        {
            Start = null;
        }

        private bool CanClearStart()
        {
            return !IsBusy && HasStart;
        }

        private void LoadonUIThread(Qualification? qualification)
        {
            Name = qualification?.Name;
            Description = qualification?.Description;
            Start = qualification?.IssueDate.ToUIDate();
            End = qualification?.ExpirationDate.ToUIDate();
            IsDirty = false;
            ValidateCommand?.Execute(null);
        }
    }
}
