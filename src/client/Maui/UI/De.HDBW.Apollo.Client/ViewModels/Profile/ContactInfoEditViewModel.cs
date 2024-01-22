// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using De.HDBW.Apollo.Client.Contracts;
using De.HDBW.Apollo.Client.Models;
using De.HDBW.Apollo.Client.Models.Profile;
using De.HDBW.Apollo.SharedContracts.Repositories;
using De.HDBW.Apollo.SharedContracts.Services;
using Invite.Apollo.App.Graph.Common.Models.UserProfile;
using Microsoft.Extensions.Logging;
using Contact = Invite.Apollo.App.Graph.Common.Models.Contact;

namespace De.HDBW.Apollo.Client.ViewModels.Profile
{
    public partial class ContactInfoEditViewModel : AbstractListViewModel<ContactEntry, Contact>
    {
        public ContactInfoEditViewModel(
            IDispatcherService dispatcherService,
            INavigationService navigationService,
            IDialogService dialogService,
            ILogger<ContactInfoEditViewModel> logger,
            IUserRepository userRepository,
            IUserService userService)
            : base(dispatcherService, navigationService, dialogService, logger, userRepository, userService, Routes.ContactInfoContactView)
        {
        }

        public override async Task OnNavigatedToAsync()
        {
            using (var worker = ScheduleWork())
            {
                try
                {
                    User = await UserRepository.GetItemAsync(worker.Token).ConfigureAwait(false);
                    var contacts = new List<Contact>();
                    contacts.AddRange(User?.ContactInfos ?? new List<Contact>());
                    contacts = contacts.OrderBy(x => x.ContactType).ToList();
                    await ExecuteOnUIThreadAsync(
                        () => LoadonUIThread(contacts.Select(x => ContactEntry.Import(x, EditAsync, CanEdit, DeleteAsync, CanDelete))), worker.Token);
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

        protected override string? GetIdFromItem(AbstractProfileEntry<Contact> entry)
        {
            return entry.Export().Id;
        }

        protected override void RemoveItemFromUser(User user, AbstractProfileEntry<Contact> entry)
        {
             user.ContactInfos.Remove(entry.Export());
        }
    }
}
