// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Collections.ObjectModel;
using System.Globalization;
using De.HDBW.Apollo.Client.Models.Generic;
using De.HDBW.Apollo.Data.Helper;
using Invite.Apollo.App.Graph.Common.Models.UserProfile;
using Invite.Apollo.App.Graph.Common.Models.UserProfile.Enums;

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

        protected override ObservableCollection<StringValue> GetAllLines(Language data)
        {
            var culture = CultureInfo.GetCultures(CultureTypes.AllCultures).FirstOrDefault(x => x.Name == data.Code);

            var items = new List<StringValue>();
#if ANDROID
            items.Add(StringValue.Import(Resources.Strings.Resources.Global_Language, culture?.DisplayName));
#elif IOS
            items.Add(StringValue.Import(Resources.Strings.Resources.Global_Language, culture?.NativeName));
#endif
            items.Add(StringValue.Import(Resources.Strings.Resources.Global_LanguageNiveau, data.Niveau?.AsEnum<LanguageNiveau>().ToString()));
            return new ObservableCollection<StringValue>(items.Where(x => !string.IsNullOrWhiteSpace(x.Data)));
        }
    }
}
