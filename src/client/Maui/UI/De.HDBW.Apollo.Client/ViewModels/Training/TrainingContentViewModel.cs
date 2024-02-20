// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using De.HDBW.Apollo.Client.Contracts;
using De.HDBW.Apollo.Client.Models;
using De.HDBW.Apollo.Data.Helper;
using Microsoft.Extensions.Logging;

namespace De.HDBW.Apollo.Client.ViewModels.Training
{
    public partial class TrainingContentViewModel : BaseViewModel
    {
        [ObservableProperty]
        private ObservableCollection<string> _sections = new ObservableCollection<string>();

        private List<string>? _items;

        public TrainingContentViewModel(
            IDispatcherService dispatcherService,
            INavigationService navigationService,
            IDialogService dialogService,
            ILogger<TrainingContentViewModel> logger)
            : base(dispatcherService, navigationService, dialogService, logger)
        {
        }

        public async override Task OnNavigatedToAsync()
        {
            using (var worker = ScheduleWork())
            {
                try
                {
                    var sections = _items?.Where(x => !string.IsNullOrWhiteSpace(x)).ToList() ?? new List<string>();
                    await ExecuteOnUIThreadAsync(() => LoadonUIThread(sections), worker.Token).ConfigureAwait(false);
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
            base.OnPrepare(navigationParameters);
            var data = navigationParameters.GetValue<string?>(NavigationParameter.Data);
            _items = data.Deserialize<List<string>>();
        }

        private void LoadonUIThread(List<string> sections)
        {
            Sections = new ObservableCollection<string>(sections);
        }
    }
}
