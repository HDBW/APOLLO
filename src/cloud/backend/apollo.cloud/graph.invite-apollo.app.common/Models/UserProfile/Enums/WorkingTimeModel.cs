// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

namespace Invite.Apollo.App.Graph.Common.Models.UserProfile.Enums
{
    /// <summary>
    /// This Enum represents the Working Time Model of the User
    /// This is a reverse engineered Enum based on the Data we have in the BA Dataset.
    /// </summary>
    public enum WorkingTimeModel
    {
        Unknown,
        FULLTIME,
        PARTTIME,
        SHIFT_NIGHT_WORK_WEEKEND,
        MINIJOB,
        HOME_TELEWORK
    }
}
