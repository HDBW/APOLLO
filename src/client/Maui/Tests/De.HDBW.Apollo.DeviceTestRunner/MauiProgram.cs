// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using Xunit.Runners.Maui;

namespace De.HDBW.Apollo.DeviceTestRunner
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp() => MauiApp
             .CreateBuilder()
             .ConfigureTests(
             new TestOptions
             {
                 Assemblies =
                 {
                    typeof(MauiProgram).Assembly,
                 },
             })
             .UseVisualRunner()
             .Build();
    }
}
