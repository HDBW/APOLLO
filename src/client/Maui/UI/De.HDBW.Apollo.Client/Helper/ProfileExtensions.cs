// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using Invite.Apollo.App.Graph.Common.Models.UserProfile;
using Contact = Invite.Apollo.App.Graph.Common.Models.Contact;

namespace De.HDBW.Apollo.Client.Helper
{
    public static class ProfileExtensions
    {
        public static List<Contact> AsSortedList(this List<Contact>? items)
        {
            return items?.OrderBy(x => x.ContactType).ThenBy(x => x.Address).ToList() ?? new List<Contact>();
        }

        public static List<Qualification> AsSortedList(this List<Qualification>? items)
        {
            return items?.OrderBy(x => x.Name).OrderByDescending(x => x.ExpirationDate ?? (x.IssueDate.HasValue ? DateTime.MaxValue.AddDays(-1) : DateTime.MaxValue)).ThenByDescending(x => x.IssueDate ?? x.ExpirationDate ?? DateTime.MaxValue).ToList() ?? new List<Qualification>();
        }

        public static List<License> AsSortedList(this List<License>? items)
        {
            return items?.OrderBy(x => x.Name).OrderByDescending(x => x.Expires ?? (x.Granted.HasValue ? DateTime.MaxValue.AddDays(-1) : DateTime.MaxValue)).ThenByDescending(x => x.Granted ?? x.Expires ?? DateTime.MaxValue).ToList() ?? new List<License>();
        }

        public static List<EducationInfo> AsSortedList(this List<EducationInfo>? items)
        {
            return items?.OrderByDescending(x => x.End ?? DateTime.MaxValue).ThenByDescending(x => x.Start).ToList() ?? new List<EducationInfo>();
        }

        public static List<CareerInfo> AsSortedList(this List<CareerInfo>? items)
        {
            return items?.OrderByDescending(x => x.End ?? DateTime.MaxValue).ThenByDescending(x => x.Start).ToList() ?? new List<CareerInfo>();
        }

        public static List<Language> AsSortedList(this List<Language>? items)
        {
            return items?.OrderByDescending(x => x.Niveau).ThenByDescending(x => x.Code).ToList() ?? new List<Language>();
        }

        public static List<WebReference> AsSortedList(this List<WebReference>? items)
        {
            return items?.OrderBy(x => x.Title).ThenBy(x => x.Url?.OriginalString).ToList() ?? new List<WebReference>();
        }
    }
}
