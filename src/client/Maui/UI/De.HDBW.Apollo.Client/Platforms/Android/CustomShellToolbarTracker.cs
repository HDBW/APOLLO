// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

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

        protected override void UpdatePageTitle(AndroidX.AppCompat.Widget.Toolbar toolbar, Page page)
        {
            toolbar.LayoutDirection = page.FlowDirection == FlowDirection.RightToLeft? Android.Views.LayoutDirection.Rtl : Android.Views.LayoutDirection.Ltr;
            base.UpdatePageTitle(toolbar, page);
        }

        protected override IShellSearchView GetSearchView(Context context)
        {
            return new CustomShellSearchView(context, ShellContext);
        }
    }
}
