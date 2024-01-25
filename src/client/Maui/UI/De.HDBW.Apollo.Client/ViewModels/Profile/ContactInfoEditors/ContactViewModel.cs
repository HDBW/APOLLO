// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using CommunityToolkit.Mvvm.ComponentModel;
using De.HDBW.Apollo.Client.Contracts;
using De.HDBW.Apollo.Client.Models.Interactions;
using De.HDBW.Apollo.SharedContracts.Repositories;
using De.HDBW.Apollo.SharedContracts.Services;
using Invite.Apollo.App.Graph.Common.Models.UserProfile;
using Invite.Apollo.App.Graph.Common.Models.UserProfile.Enums;
using Microsoft.Extensions.Logging;
using Contact = Invite.Apollo.App.Graph.Common.Models.Contact;

namespace De.HDBW.Apollo.Client.ViewModels.Profile.ContactInfoEditors
{
    public partial class ContactViewModel : AbstractProfileEditorViewModel<Contact>
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

        public ContactViewModel(
            IDispatcherService dispatcherService,
            INavigationService navigationService,
            IDialogService dialogService,
            ILogger<ContactViewModel> logger,
            IUserService userService,
            IUserRepository userRepository)
            : base(dispatcherService, navigationService, dialogService, logger, userRepository, userService)
        {
        }

        protected override async Task<Contact?> LoadDataAsync(User user, string? entryId, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            var contact = user.ContactInfos.FirstOrDefault(x => x.Id == entryId);
            var contactTypes = new List<InteractionEntry>();
            contactTypes.Add(InteractionEntry.Import(Resources.Strings.Resources.ContactType_Private, ContactType.Private, (x) => { return Task.CompletedTask; }, (x) => { return true; }));
            contactTypes.Add(InteractionEntry.Import(Resources.Strings.Resources.ContactType_Professional, ContactType.Professional, (x) => { return Task.CompletedTask; }, (x) => { return true; }));
            await ExecuteOnUIThreadAsync(() => LoadonUIThread(contact, contactTypes), token).ConfigureAwait(false);
            return contact;
        }

        protected override Contact CreateNewEntry(User user)
        {
            var entry = new Contact();
            user.ContactInfos.Add(entry);
            return entry;
        }

        protected override void ApplyChanges(Contact entry)
        {
            entry.ContactType = (ContactType)(SelectedContactType?.Data ?? ContactType.Unknown);
            entry.Address = Address?.Trim() ?? string.Empty;
            entry.Phone = Phone?.Trim() ?? string.Empty;
            entry.Mail = Email?.Trim() ?? string.Empty;
            entry.City = City?.Trim() ?? string.Empty;
            entry.ZipCode = ZipCode?.Trim() ?? string.Empty;
            entry.Country = Country?.Trim() ?? string.Empty;
            entry.Region = Region?.Trim() ?? string.Empty;
        }

        protected override void DeleteEntry(User user, Contact entry)
        {
            user!.ContactInfos.Remove(entry);
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

        private void LoadonUIThread(Contact? contact, List<InteractionEntry> contactTypes)
        {
            Address = contact?.Address;
            Phone = contact?.Phone;
            Email = contact?.Mail;
            City = contact?.City;
            ZipCode = contact?.ZipCode;
            Country = contact?.Country;
            Region = contact?.Region;

            ContactTypes = new ObservableCollection<InteractionEntry>(contactTypes);
            SelectedContactType = (contact?.ContactType != null) ? ContactTypes.FirstOrDefault(x => ((ContactType?)x.Data) == contact.ContactType) : ContactTypes.FirstOrDefault();
            IsDirty = false;
            ValidateCommand?.Execute(null);
        }
    }
}
