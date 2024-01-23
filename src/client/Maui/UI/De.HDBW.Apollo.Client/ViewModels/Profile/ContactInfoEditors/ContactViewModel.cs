// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using De.HDBW.Apollo.Client.Contracts;
using De.HDBW.Apollo.Client.Models;
using De.HDBW.Apollo.Client.Models.Interactions;
using De.HDBW.Apollo.SharedContracts.Repositories;
using De.HDBW.Apollo.SharedContracts.Services;
using Invite.Apollo.App.Graph.Common.Models.UserProfile;
using Invite.Apollo.App.Graph.Common.Models.UserProfile.Enums;
using Microsoft.Extensions.Logging;
using Contact = Invite.Apollo.App.Graph.Common.Models.Contact;

namespace De.HDBW.Apollo.Client.ViewModels.Profile.ContactInfoEditors
{
    public partial class ContactViewModel : AbstractSaveDataViewModel
    {
        [ObservableProperty]
        private ObservableCollection<InteractionEntry> _contactTypes = new ObservableCollection<InteractionEntry>();

        [ObservableProperty]
        private InteractionEntry? _selectedContactType;

        [ObservableProperty]
        [Required(ErrorMessageResourceType = typeof(Resources.Strings.Resources), ErrorMessageResourceName = nameof(Resources.Strings.Resources.GlobalError_PropertyRequired))]
        private string? _address;

        [ObservableProperty]
        [Required(ErrorMessageResourceType = typeof(Resources.Strings.Resources), ErrorMessageResourceName = nameof(Resources.Strings.Resources.GlobalError_PropertyRequired))]
        private string? _city;

        [ObservableProperty]
        [Required(ErrorMessageResourceType = typeof(Resources.Strings.Resources), ErrorMessageResourceName = nameof(Resources.Strings.Resources.GlobalError_PropertyRequired))]
        [RegularExpression("^[0-9]{5}$", ErrorMessageResourceType = typeof(Resources.Strings.Resources), ErrorMessageResourceName = nameof(Resources.Strings.Resources.GlobalError_InvalidZipCode))]
        private string? _zipCode;

        [ObservableProperty]
        private string? _region;

        [ObservableProperty]
        private string? _country;

        [ObservableProperty]
        [EmailAddress(ErrorMessageResourceType = typeof(Resources.Strings.Resources), ErrorMessageResourceName = nameof(Resources.Strings.Resources.GlobalError_InvalidEmail))]
        private string? _email;

        [ObservableProperty]
        [Phone(ErrorMessageResourceType = typeof(Resources.Strings.Resources), ErrorMessageResourceName = nameof(Resources.Strings.Resources.GlobalError_InvalidPhoneNumber))]
        private string? _phone;

        private Contact? _contact;

        private User? _user;

        private string? _contactId;

        public ContactViewModel(
            IDispatcherService dispatcherService,
            INavigationService navigationService,
            IDialogService dialogService,
            ILogger<ContactViewModel> logger,
            IUserService userService,
            IUserRepository userRepository)
            : base(dispatcherService, navigationService, dialogService, logger)
        {
            ArgumentNullException.ThrowIfNull(userRepository);
            ArgumentNullException.ThrowIfNull(userService);
            UserRepository = userRepository;
            UserService = userService;
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
                    var contact = user?.ContactInfos.FirstOrDefault(x => x.Id == _contactId);
                    var contactTypes = new List<InteractionEntry>();
                    contactTypes.Add(InteractionEntry.Import(Resources.Strings.Resources.ContactType_Private, ContactType.Private, (x) => { return Task.CompletedTask; }, (x) => { return true; }));
                    contactTypes.Add(InteractionEntry.Import(Resources.Strings.Resources.ContactType_Professional, ContactType.Professional, (x) => { return Task.CompletedTask; }, (x) => { return true; }));
                    await ExecuteOnUIThreadAsync(
                        () => LoadonUIThread(user, contact, contactTypes), worker.Token);
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
            _contactId = navigationParameters.GetValue<string?>(NavigationParameter.Id);
        }

        protected override async Task<bool> SaveAsync(CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            if (_user == null || !IsDirty)
            {
                return !IsDirty;
            }

            _contact = _contact ?? new Contact();
            _contact.ContactType = (ContactType)(SelectedContactType?.Data ?? ContactType.Unknown);
            _contact.Address = Address?.Trim() ?? string.Empty;
            _contact.Phone = Phone?.Trim() ?? string.Empty;
            _contact.Mail = Email?.Trim() ?? string.Empty;
            _contact.City = City?.Trim() ?? string.Empty;
            _contact.ZipCode = ZipCode?.Trim() ?? string.Empty;
            _contact.Country = Country?.Trim() ?? string.Empty;
            _contact.Region = Region?.Trim() ?? string.Empty;
            if (string.IsNullOrWhiteSpace(_contact.Id))
            {
                _user.ContactInfos.Add(_contact);
            }

            var response = await UserService.SaveAsync(_user, token).ConfigureAwait(false);
            if (string.IsNullOrWhiteSpace(response))
            {
                Logger.LogError($"Unable to contact user remotely {nameof(SaveAsync)} in {GetType().Name}.");
                return !IsDirty;
            }

            _user.Id = response;
            var userResult = await UserService.GetUserAsync(_user.Id, token).ConfigureAwait(false);
            if (userResult == null || !await UserRepository.SaveAsync(userResult, CancellationToken.None).ConfigureAwait(false))
            {
                Logger.LogError($"Unable to contact user locally {nameof(SaveAsync)} in {GetType().Name}.");
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
        }

        partial void OnSelectedContactTypeChanged(InteractionEntry? value)
        {
            IsDirty = true;
        }

        partial void OnAddressChanged(string? value)
        {
            ValidateProperty(value, nameof(Address));
            IsDirty = true;
        }

        partial void OnCityChanged(string? value)
        {
            ValidateProperty(value, nameof(City));
            IsDirty = true;
        }

        partial void OnZipCodeChanged(string? value)
        {
            ValidateProperty(value, nameof(ZipCode));
            IsDirty = true;
        }

        partial void OnRegionChanged(string? value)
        {
            IsDirty = true;
        }

        partial void OnEmailChanging(string? value)
        {
            value = string.IsNullOrWhiteSpace(value) ? null : value?.Trim();
        }

        partial void OnEmailChanged(string? value)
        {
            ValidateProperty(value, nameof(Email));
            IsDirty = true;
        }

        partial void OnPhoneChanging(string? value)
        {
            value = string.IsNullOrWhiteSpace(value) ? null : value?.Trim();
        }

        partial void OnPhoneChanged(string? value)
        {
            ValidateProperty(value, nameof(Phone));
            IsDirty = true;
        }

        private void LoadonUIThread(User? user, Contact? contact, List<InteractionEntry> contactTypes)
        {
            _user = user;
            _contact = contact;
            Address = _contact?.Address;
            Phone = _contact?.Phone;
            Email = _contact?.Mail;
            City = _contact?.City;
            ZipCode = _contact?.ZipCode;
            Country = _contact?.Country;
            Region = _contact?.Region;

            ContactTypes = new ObservableCollection<InteractionEntry>(contactTypes);
            SelectedContactType = (_contact?.ContactType != null) ? ContactTypes.FirstOrDefault(x => ((ContactType?)x.Data) == _contact.ContactType) : ContactTypes.FirstOrDefault();
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
                    _user!.ContactInfos.Remove(_contact!);
                    var response = await UserService.SaveAsync(_user, worker.Token).ConfigureAwait(false);
                    if (string.IsNullOrWhiteSpace(response))
                    {
                        Logger.LogError($"Unable to delete contact remotely {nameof(Delete)} in {GetType().Name}.");
                        await ShowErrorAsync(Resources.Strings.Resources.GlobalError_UnableToSaveData, worker.Token).ConfigureAwait(false);
                        return;
                    }

                    if (!await UserRepository.SaveAsync(_user, CancellationToken.None).ConfigureAwait(false))
                    {
                        Logger.LogError($"Unable to save contact locally {nameof(Delete)} in {GetType().Name}.");
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
            return !IsBusy && _user != null && _contact != null;
        }
    }
}
