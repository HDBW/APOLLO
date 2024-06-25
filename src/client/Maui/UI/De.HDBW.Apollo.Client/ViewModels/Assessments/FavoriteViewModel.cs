// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using De.HDBW.Apollo.Client.Contracts;
using Microsoft.Extensions.Logging;
using De.HDBW.Apollo.Client.Models.Assessment;

namespace De.HDBW.Apollo.Client.ViewModels.Assessments
{
    public partial class FavoriteViewModel : BaseViewModel
    {
        [ObservableProperty]
        private ObservableCollection<ModuleTileEntry> _favoriteItems = new ObservableCollection<ModuleTileEntry>();

        public FavoriteViewModel(
            IDispatcherService dispatcherService,
            INavigationService navigationService,
            IDialogService dialogService,
            ILogger logger)
            : base(dispatcherService, navigationService, dialogService, logger)
        {
        }
    }
}
