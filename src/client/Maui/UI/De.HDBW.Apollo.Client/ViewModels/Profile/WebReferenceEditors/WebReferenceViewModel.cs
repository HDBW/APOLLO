// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.ComponentModel.DataAnnotations;
using CommunityToolkit.Mvvm.ComponentModel;
using De.HDBW.Apollo.Client.Contracts;
using De.HDBW.Apollo.SharedContracts.Repositories;
using De.HDBW.Apollo.SharedContracts.Services;
using Invite.Apollo.App.Graph.Common.Models.UserProfile;
using Microsoft.Extensions.Logging;

namespace De.HDBW.Apollo.Client.ViewModels.Profile.WebReferenceEditors
{
    public partial class WebReferenceViewModel : AbstractProfileEditorViewModel<WebReference>
    {
        [ObservableProperty]
        [Required(ErrorMessageResourceType = typeof(Resources.Strings.Resources), ErrorMessageResourceName = nameof(Resources.Strings.Resources.GlobalError_PropertyRequired))]
        private string? _description;

        [ObservableProperty]
        [Required(ErrorMessageResourceType = typeof(Resources.Strings.Resources), ErrorMessageResourceName = nameof(Resources.Strings.Resources.GlobalError_PropertyRequired))]
        [Url(ErrorMessageResourceType = typeof(Resources.Strings.Resources), ErrorMessageResourceName = nameof(Resources.Strings.Resources.GlobalError_InvalidUrl))]
        private string? _url;

        public WebReferenceViewModel(
            IDispatcherService dispatcherService,
            INavigationService navigationService,
            IDialogService dialogService,
            ILogger<WebReferenceViewModel> logger,
            IUserRepository userRepository,
            IUserService userServic)
            : base(dispatcherService, navigationService, dialogService, logger, userRepository, userServic)
        {
        }

        protected override async Task<WebReference?> LoadDataAsync(User user, string? enityId, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            var webReference = user?.Profile?.WebReferences?.FirstOrDefault(x => x.Id == enityId);
            await ExecuteOnUIThreadAsync(() => LoadonUIThread(webReference), token).ConfigureAwait(false);
            return webReference;
        }

        protected override WebReference CreateNewEntry(User user)
        {
            var entry = new WebReference();
            user.Profile!.WebReferences = user.Profile!.WebReferences ?? new List<WebReference>();
            user.Profile!.WebReferences.Add(entry);
            return entry;
        }

        protected override void DeleteEntry(User user, WebReference entry)
        {
            user.Profile!.WebReferences!.Remove(entry);
        }

        protected override void ApplyChanges(WebReference entry)
        {
            entry.Url = new Uri(Url?.Trim() ?? "about:blank");
            entry.Title = Description?.Trim() ?? string.Empty;
        }

        partial void OnUrlChanging(string? value)
        {
            value = string.IsNullOrWhiteSpace(value) ? null : value?.Trim();
        }

        partial void OnUrlChanged(string? value)
        {
            ValidateProperty(value, nameof(Url));
            IsDirty = true;
        }

        partial void OnDescriptionChanging(string? value)
        {
            value = string.IsNullOrWhiteSpace(value) ? null : value;
        }

        partial void OnDescriptionChanged(string? value)
        {
            IsDirty = true;
        }

        private void LoadonUIThread(WebReference? webReference)
        {
            Url = webReference?.Url?.AbsoluteUri;
            Description = webReference?.Title;
            IsDirty = false;
            ValidateCommand?.Execute(null);
        }
    }
}
