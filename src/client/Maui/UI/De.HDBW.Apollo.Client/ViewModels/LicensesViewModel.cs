// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using CommunityToolkit.Mvvm.ComponentModel;
using De.HDBW.Apollo.Client.Contracts;
using De.HDBW.Apollo.Data.Repositories;
using De.HDBW.Apollo.SharedContracts.Models;
using Invite.Apollo.App.Graph.Common.Backend.Api;
using Invite.Apollo.App.Graph.Common.Models.Trainings;
using Microsoft.Extensions.Logging;

namespace De.HDBW.Apollo.Client.ViewModels
{
    public partial class LicensesViewModel : BaseViewModel
    {
        [ObservableProperty]
        private string? _licensText;

        public LicensesViewModel(
            IDispatcherService dispatcherService,
            INavigationService navigationService,
            IDialogService dialogService,
            ILogger<LicensesViewModel> logger)
            : base(dispatcherService, navigationService, dialogService, logger)
        {
        }

        public async override Task OnNavigatedToAsync()
        {
            using (var worker = ScheduleWork())
            {
                try
                {
                    var licenses = string.Empty;
                    using (var stream = await FileSystem.OpenAppPackageFileAsync("LICENSE"))
                    {
                        using (var reader = new StreamReader(stream))
                        {
                            licenses = reader.ReadToEnd();
                        }
                    }

                    await ExecuteOnUIThreadAsync(() => LoadonUIThread(licenses), worker.Token).ConfigureAwait(false);
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

        private void LoadonUIThread(string licenses)
        {
            LicensText = licenses;
        }
    }
}
