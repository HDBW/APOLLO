﻿// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

namespace Invite.Apollo.App.Graph.Common.Models.UserProfile.Enums
{
    public enum EducationType
    {
        Unknown = 0,
        // Schulbildung
        Education = 1,
        // Berufsausbildung (betr./außerbetr.)
        CompanyBasedVocationalTraining = 2,
        // Studium
        Study = 3,
        // Berufsausbildung (schulisch)
        VocationalTraining = 4,
        // Weiterbildung
        FurtherEducation = 5,
    }
}
