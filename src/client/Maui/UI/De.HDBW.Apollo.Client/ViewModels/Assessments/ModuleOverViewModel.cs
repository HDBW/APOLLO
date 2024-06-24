// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.
using De.HDBW.Apollo.Client.Contracts;
using Microsoft.Extensions.Logging;

namespace De.HDBW.Apollo.Client.ViewModels.Assessments
{
    public partial class ModuleOverViewModel : BaseViewModel
    {
        public ModuleOverViewModel(
            IDispatcherService dispatcherService,
            INavigationService navigationService,
            IDialogService dialogService,
            ILogger<ModuleOverViewModel> logger)
            : base(dispatcherService, navigationService, dialogService, logger)
        {
        }
    }
}
