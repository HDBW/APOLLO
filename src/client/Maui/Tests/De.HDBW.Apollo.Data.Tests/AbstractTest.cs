// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using De.HDBW.Apollo.Data.Tests.Extensions;
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace De.HDBW.Apollo.Data.Tests
{
    public abstract class AbstractTest
    {
        protected AbstractTest(ITestOutputHelper outputHelper)
        {
            OutputHelper = outputHelper;
            Logger = this.SetupLogger<AbstractTest>(OutputHelper);
        }

        protected ITestOutputHelper OutputHelper { get; }

        protected ILogger<AbstractTest> Logger { get; }
    }
}
