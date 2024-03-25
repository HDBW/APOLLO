// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

namespace Invite.Apollo.App.Graph.Common.Models.UserProfile.Enums
{
    /// <summary>
    /// This Enum represents the Type of the Contact
    /// It indicates if the Contact is a Professional or Private Contact
    /// </summary>
    public enum ContactType
    {
        Unknown = 0,
        Professional = 1,
        Private = 2,
        Trainer = 3,
        Training = 4,
        TrainingLocation = 5,
    }
}
