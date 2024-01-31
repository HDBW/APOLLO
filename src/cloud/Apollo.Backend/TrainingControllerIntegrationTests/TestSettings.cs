// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amazon.Runtime;

namespace Apollo.RestService.IntergrationTests
{
    internal class TestSettings
    {
        public string ServiceUrl{ get; set; }

        public string ApiKey { get; set; }
    }
}
