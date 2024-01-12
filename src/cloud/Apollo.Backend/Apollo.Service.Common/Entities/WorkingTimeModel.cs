// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

namespace Apollo.Common.Entities
{
    /// <summary>
    /// Based on Original BA Dataset.
    /// Needs to be reviewd? maybe we can change it.
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
