// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using De.HDBW.Apollo.Client.Contracts;
using De.HDBW.Apollo.Client.Models.Interactions;
using Invite.Apollo.App.Graph.Common.Models.UserProfile;
using Invite.Apollo.App.Graph.Common.Models.UserProfile.Enums;
using Microsoft.Extensions.Logging;

namespace De.HDBW.Apollo.Client.ViewModels.Profile.ContactInfoEditors
{
    public partial class ContactViewModel : BaseViewModel
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

        public ContactViewModel(
            IDispatcherService dispatcherService,
            INavigationService navigationService,
            IDialogService dialogService,
            ILogger<ContactViewModel> logger)
            : base(dispatcherService, navigationService, dialogService, logger)
        {
        }

        public override async Task OnNavigatedToAsync()
        {
            using (var worker = ScheduleWork())
            {
                try
                {
                    var contactTypes = new List<InteractionEntry>();
                    contactTypes.Add(InteractionEntry.Import(Resources.Strings.Resources.ContactType_Private, ContactType.Private, (x) => { return Task.CompletedTask; }, (x) => { return true; }));
                    contactTypes.Add(InteractionEntry.Import(Resources.Strings.Resources.ContactType_Professional, ContactType.Professional, (x) => { return Task.CompletedTask; }, (x) => { return true; }));
                    await ExecuteOnUIThreadAsync(
                        () => LoadonUIThread(contactTypes), worker.Token);
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

        private void LoadonUIThread(List<InteractionEntry> contactTypes)
        {
            ContactTypes = new ObservableCollection<InteractionEntry>(contactTypes);
            SelectedContactType = ContactTypes.FirstOrDefault();
        }
    }
}
