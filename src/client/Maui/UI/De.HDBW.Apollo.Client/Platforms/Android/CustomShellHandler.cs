// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using Microsoft.Maui.Controls.Handlers.Compatibility;
using Microsoft.Maui.Controls.Platform.Compatibility;
using AToolbar = AndroidX.AppCompat.Widget.Toolbar;

namespace De.HDBW.Apollo.Client.Platforms
{
    public class CustomShellHandler : ShellRenderer
    {
        protected override IShellToolbarTracker CreateTrackerForToolbar(AToolbar toolbar)
        {
            return new CustomShellToolbarTracker(this, toolbar, ((IShellContext)this).CurrentDrawerLayout);
        }
    }
}
