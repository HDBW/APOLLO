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
            return items ?? new List<Contact>();
        }

        public static List<Qualification> AsSortedList(this List<Qualification>? items)
        {
            return items ?? new List<Qualification>();
        }

        public static List<License> AsSortedList(this List<License>? items)
        {
            return items ?? new List<License>();
        }

        public static List<EducationInfo> AsSortedList(this List<EducationInfo>? items)
        {
            return items ?? new List<EducationInfo>();
        }

        public static List<CareerInfo> AsSortedList(this List<CareerInfo>? items)
        {
            return items ?? new List<CareerInfo>();
        }

        public static List<Language> AsSortedList(this List<Language>? items)
        {
            return items ?? new List<Language>();
        }

        public static List<WebReference> AsSortedList(this List<WebReference>? items)
        {
            return items ?? new List<WebReference>();
        }
    }
}
