﻿// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apollo.Common.Entities
{
    /// <summary>
    /// Defines the filter fol lookups of trainings.
    /// </summary>
    public class QueryTrainings
    {
        public string? Contains { get; set; }
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
    }
}
