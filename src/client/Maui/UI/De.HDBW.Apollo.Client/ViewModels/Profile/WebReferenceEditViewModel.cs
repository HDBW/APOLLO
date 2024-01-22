// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using De.HDBW.Apollo.Client.Contracts;
using De.HDBW.Apollo.Client.Models;
using De.HDBW.Apollo.Client.Models.Profile;
using De.HDBW.Apollo.SharedContracts.Repositories;
using De.HDBW.Apollo.SharedContracts.Services;
using Invite.Apollo.App.Graph.Common.Models.UserProfile;
using Microsoft.Extensions.Logging;

namespace De.HDBW.Apollo.Client.ViewModels.Profile
{
    public partial class WebReferenceEditViewModel : AbstractListViewModel<WebReferenceEntry, WebReference>
    {
        public WebReferenceEditViewModel(
            IDispatcherService dispatcherService,
            INavigationService navigationService,
            IDialogService dialogService,
            ILogger<WebReferenceEditViewModel> logger,
            IUserRepository userRepository,
            IUserService userService)
            : base(dispatcherService, navigationService, dialogService, logger, userRepository, userService, Routes.WebReferenceView)
        {
        }

        public override async Task OnNavigatedToAsync()
        {
            using (var worker = ScheduleWork())
            {
                try
                {
                    User = await UserRepository.GetItemAsync(worker.Token).ConfigureAwait(false);
                    var items = new List<WebReference>();
                    items.AddRange(User?.Profile?.WebReferences ?? new List<WebReference>());
                    items = items.OrderBy(x => x.Title).ToList();
                    await ExecuteOnUIThreadAsync(
                        () => LoadonUIThread(items.Select(x => WebReferenceEntry.Import(x, EditAsync, CanEdit, DeleteAsync, CanDelete))), worker.Token);
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

        protected override string? GetIdFromItem(AbstractProfileEntry<WebReference> entry)
        {
            return entry.Export().Id;
        }

        protected override void RemoveItemFromUser(User user, AbstractProfileEntry<WebReference> entry)
        {
            user.Profile?.WebReferences.Remove(entry.Export());
        }
    }
}
