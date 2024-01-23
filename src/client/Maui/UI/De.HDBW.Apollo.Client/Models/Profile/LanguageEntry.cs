// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Collections.ObjectModel;
using Invite.Apollo.App.Graph.Common.Models.UserProfile;

namespace De.HDBW.Apollo.Client.Models.Profile
{
    public partial class LanguageEntry : AbstractProfileEntry<Language>
    {
        private LanguageEntry(
            Language data,
            Func<AbstractProfileEntry<Language>, Task> editHandle,
            Func<AbstractProfileEntry<Language>, bool> canEditHandle,
            Func<AbstractProfileEntry<Language>, Task> deleteHandle,
            Func<AbstractProfileEntry<Language>, bool> canDeleteHandle)
            : base(data, editHandle, canEditHandle, deleteHandle, canDeleteHandle)
        {
        }

        public static LanguageEntry Import(Language data, Func<AbstractProfileEntry<Language>, Task> editHandle, Func<AbstractProfileEntry<Language>, bool> canEditHandle, Func<AbstractProfileEntry<Language>, Task> deleteHandle, Func<AbstractProfileEntry<Language>, bool> canDeleteHandle)
        {
            return new LanguageEntry(data, editHandle, canEditHandle,  deleteHandle, canDeleteHandle);
        }

        protected override ObservableCollection<string> GetAdditionalLines(Language data)
        {
            var items = new List<string?>();
            items.Add(data.Code?.DisplayName);
            items.Add(data.Niveau?.ToString());
            return new ObservableCollection<string>(items.Where(x => !string.IsNullOrWhiteSpace(x) && x != FirstLine).OfType<string>());
        }

        protected override string GetFristLine(Language data)
        {
            var items = new List<string?>();
            items.Add(data.Code?.DisplayName);
            items.Add(data.Niveau?.ToString());
            return items.FirstOrDefault(x => !string.IsNullOrWhiteSpace(x)) ?? string.Empty;
        }
    }
}
