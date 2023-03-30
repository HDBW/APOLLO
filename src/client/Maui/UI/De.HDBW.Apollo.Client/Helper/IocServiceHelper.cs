using System;

namespace De.HDBW.Apollo.Client.Helper;

public static class IocServiceHelper
{
    public static IServiceProvider ServiceProvider
    {
        get
        {
#if ANDROID
            return MauiApplication.Current.Services;
#elif IOS
            return MauiUIApplicationDelegate.Current.Services;
#elif WINDOWS
            return MauiWinUIApplication.Current.Services;
#endif
        }
    }
}
