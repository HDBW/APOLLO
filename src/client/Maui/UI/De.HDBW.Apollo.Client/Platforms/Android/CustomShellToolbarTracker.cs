// (c) Licensed to the HDBW under one or more agreements.

using Android.Content;
using AndroidX.DrawerLayout.Widget;
using Microsoft.Maui.Controls.Platform.Compatibility;

namespace De.HDBW.Apollo.Client.Platforms
{
    public class CustomShellToolbarTracker : ShellToolbarTracker
    {
        public CustomShellToolbarTracker(
            IShellContext shellContext,
            AndroidX.AppCompat.Widget.Toolbar toolbar,
            DrawerLayout drawerLayout)
            : base(shellContext, toolbar, drawerLayout)
        {
        }

        protected override IShellSearchView GetSearchView(Context context)
        {
            return new CustomShellSearchView(context, ShellContext);
        }
    }
}
