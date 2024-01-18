// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Globalization;
using Invite.Apollo.App.Graph.Common.Models.UserProfile.Enums;

namespace Invite.Apollo.App.Graph.Common.Models.UserProfile
{
    public class Language
    {
        public string Name { get; set; }

        public LanguageNiveau? Niveau { get; set; }

        // CultureInfo get Culture ISO639-2 
        public CultureInfo Code { get; set; }
    }
}
