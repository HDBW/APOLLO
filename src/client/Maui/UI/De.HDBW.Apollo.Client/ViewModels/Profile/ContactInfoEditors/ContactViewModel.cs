﻿// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Collections.ObjectModel;
using System.Diagnostics.Metrics;
using CommunityToolkit.Mvvm.ComponentModel;
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
        private string? _address;

        [ObservableProperty]
        private string? _city;

        [ObservableProperty]
        private string? _zipCode;

        [ObservableProperty]
        private string? _region;

        [ObservableProperty]
        private string? _country;

        [ObservableProperty]
        private string? _email;

        [ObservableProperty]
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
            _contact.Address = Address ?? string.Empty;
            _contact.Phone = Phone ?? string.Empty;
            _contact.Mail = Email ?? string.Empty;
            _contact.City = City ?? string.Empty;
            _contact.ZipCode = ZipCode ?? string.Empty;
            if (string.IsNullOrWhiteSpace(_contact.Id))
            {
                _user.ContactInfos.Add(_contact);
            }

            // TODO: Wait for new Contract
            //_contact.Country = Country ?? string.Empty;
            //_contact.Region = Region ?? string.Empty;

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

        partial void OnSelectedContactTypeChanged(InteractionEntry? value)
        {
            IsDirty = true;
        }

        partial void OnAddressChanged(string? value)
        {
            IsDirty = true;
        }

        partial void OnCityChanged(string? value)
        {
            IsDirty = true;
        }

        partial void OnZipCodeChanged(string? value)
        {
            IsDirty = true;
        }

        partial void OnRegionChanged(string? value)
        {
            IsDirty = true;
        }

        partial void OnEmailChanged(string? value)
        {
            IsDirty = true;
        }

        partial void OnPhoneChanged(string? value)
        {
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
            // TODO: Wait for new Contract
            //Country = _contact?.Country;
            //Region = _contact?.Region;
            ContactTypes = new ObservableCollection<InteractionEntry>(contactTypes);
            SelectedContactType = (_contact?.ContactType != null) ? ContactTypes.FirstOrDefault(x => x.Data == (object)_contact.ContactType) : ContactTypes.FirstOrDefault();
        }
    }
}
