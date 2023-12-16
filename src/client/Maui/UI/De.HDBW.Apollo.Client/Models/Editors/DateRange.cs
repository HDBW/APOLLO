// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using De.HDBW.Apollo.Client.Contracts;

namespace De.HDBW.Apollo.Client.Models.Editors
{
    public class DateRange
    {
        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }
    }
}
