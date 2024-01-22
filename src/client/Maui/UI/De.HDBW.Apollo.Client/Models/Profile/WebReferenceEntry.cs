// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.
using System.Collections.ObjectModel;
using Invite.Apollo.App.Graph.Common.Models.UserProfile;

namespace De.HDBW.Apollo.Client.Models.Profile
{
    public partial class WebReferenceEntry : AbstractProfileEntry<WebReference>
    {
        private WebReferenceEntry(
            WebReference data,
            Func<AbstractProfileEntry<WebReference>, Task> editHandle,
            Func<AbstractProfileEntry<WebReference>, bool> canEditHandle,
            Func<AbstractProfileEntry<WebReference>, Task> deleteHandle,
            Func<AbstractProfileEntry<WebReference>, bool> canDeleteHandle)
            : base(data, editHandle, canEditHandle, deleteHandle, canDeleteHandle)
        {
        }

        public static WebReferenceEntry Import(WebReference data, Func<AbstractProfileEntry<WebReference>, Task> editHandle, Func<AbstractProfileEntry<WebReference>, bool> canEditHandle, Func<AbstractProfileEntry<WebReference>, Task> deleteHandle, Func<AbstractProfileEntry<WebReference>, bool> canDeleteHandle)
        {
            return new WebReferenceEntry(data, editHandle, canEditHandle,  deleteHandle, canDeleteHandle);
        }

        protected override ObservableCollection<string> GetAdditionalLines(WebReference data)
        {
            var items = new List<string?>();
            items.Add(data.Title);
            items.Add(data.Url.OriginalString);
            return new ObservableCollection<string>(items.Where(x => !string.IsNullOrWhiteSpace(x) && x != FirstLine).OfType<string>());
        }

        protected override string GetFristLine(WebReference data)
        {
            var items = new List<string?>();
            items.Add(data.Title);
            items.Add(data.Url.OriginalString);
            return items.FirstOrDefault(x => !string.IsNullOrWhiteSpace(x)) ?? string.Empty;
        }
    }
}
