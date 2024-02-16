// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

namespace Apollo.Common.Entities
{
    /// <summary>
    /// Currentlly not a part of UI
    /// This is not a esco or ai taxonomy skillset it is basically a skillset from ba data.
    /// since we have a lot of data, why not use it.
    /// </summary>
    public class LeadershipSkills
    {
        public bool PowerOfAttorney { get; set; }

        public bool BudgetResponsibility { get; set; }

        public YearRange YearsofLeadership { get; set; }

        public StaffResponsibility StaffResponsibility { get; set; }
    }
}
