// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apollo.Common.Entities
{
    /// <summary>
    /// Defines the filter for lookups of queries.
    /// </summary>
    public class UserQuery
    {
        public string? Contains { get; set; }
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
    }
}
