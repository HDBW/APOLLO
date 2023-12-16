// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

namespace De.HDBW.Apollo.Client.Helper;

public static class IocServiceHelper
{
    public static IServiceProvider? ServiceProvider
    {
        get
        {
            IPlatformApplication? app = null;
#if ANDROID
            app = MauiApplication.Current;
#elif IOS
            app = MauiUIApplicationDelegate.Current;
#elif WINDOWS
            app = MauiWinUIApplication.Current;
#endif
            return app?.Services;
        }
    }
}
