// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using CommunityToolkit.Mvvm.ComponentModel;
using De.HDBW.Apollo.Client.Contracts;
using Microsoft.Extensions.Logging;

namespace De.HDBW.Apollo.Client.ViewModels.Profile
{
    public partial class WebReferenceEditViewModel : BaseViewModel
    {
        //[ObservableProperty]
        //public string? _title;

        //[ObservableProperty]
        //public string? _uri;

        //[ObservableProperty]
        //public ObservableCollection<StringValue> _links;

        //[ObservableProperty]
        //public StringValue _selectedLinks;

        public WebReferenceEditViewModel(
            IDispatcherService dispatcherService,
            INavigationService navigationService,
            IDialogService dialogService,
            ILogger<WebReferenceEditViewModel> logger)
            : base(dispatcherService, navigationService, dialogService, logger)
        {
        }
    }
}
