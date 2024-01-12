// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

namespace Apollo.Common.Entities
{
    public enum EducationType
    {
        Unkown = 0,
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
