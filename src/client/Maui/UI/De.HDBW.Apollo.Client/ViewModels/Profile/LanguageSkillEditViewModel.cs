// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using CommunityToolkit.Mvvm.ComponentModel;
using De.HDBW.Apollo.Client.Contracts;
using Invite.Apollo.App.Graph.Common.Models.UserProfile;
using Microsoft.Extensions.Logging;

namespace De.HDBW.Apollo.Client.ViewModels.Profile
{
    public partial class LanguageSkillEditViewModel : BaseViewModel
    {
        [ObservableProperty]
        public List<Language> _laguages;

        public LanguageSkillEditViewModel(
            IDispatcherService dispatcherService,
            INavigationService navigationService,
            IDialogService dialogService,
            ILogger<LanguageSkillEditViewModel> logger)
            : base(dispatcherService, navigationService, dialogService, logger)
        {
        }
    }
}
