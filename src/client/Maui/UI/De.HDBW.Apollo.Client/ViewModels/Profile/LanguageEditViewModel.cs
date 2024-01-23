// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using De.HDBW.Apollo.Client.Contracts;
using De.HDBW.Apollo.Client.Models;
using De.HDBW.Apollo.Client.Models.Profile;
using De.HDBW.Apollo.SharedContracts.Repositories;
using De.HDBW.Apollo.SharedContracts.Services;
using Invite.Apollo.App.Graph.Common.Models.UserProfile;
using Microsoft.Extensions.Logging;

namespace De.HDBW.Apollo.Client.ViewModels.Profile
{
    public partial class LanguageEditViewModel : AbstractListViewModel<LanguageEntry, Language>
    {
        public LanguageEditViewModel(
            IDispatcherService dispatcherService,
            INavigationService navigationService,
            IDialogService dialogService,
            ILogger<LanguageEditViewModel> logger,
            IUserRepository userRepository,
            IUserService userService)
            : base(dispatcherService, navigationService, dialogService, logger, userRepository, userService, Routes.LanguageView)
        {
        }

        public override async Task OnNavigatedToAsync()
        {
            using (var worker = ScheduleWork())
            {
                try
                {
                    User = await UserRepository.GetItemAsync(worker.Token).ConfigureAwait(false);
                    var items = new List<Language>();
                    items.AddRange(User?.Profile?.LanguageSkills ?? new List<Language>());
                    items = items.OrderBy(x => x.Name).ToList();
                    await ExecuteOnUIThreadAsync(
                        () => LoadonUIThread(items.Select(x => LanguageEntry.Import(x, EditAsync, CanEdit, DeleteAsync, CanDelete))), worker.Token);
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

        protected override string? GetIdFromItem(AbstractProfileEntry<Language> entry)
        {
            return entry.Export().Id;
        }

        protected override void RemoveItemFromUser(User user, AbstractProfileEntry<Language> entry)
        {
            user.Profile!.LanguageSkills.Remove(entry.Export());
        }
    }
}
