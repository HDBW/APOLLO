// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using Microsoft.Maui.Controls.Handlers.Compatibility;
using Microsoft.Maui.Controls.Platform.Compatibility;

namespace De.HDBW.Apollo.Client.Platforms
{
    public class CustomShellHandler : ShellRenderer
    {
        protected override IShellPageRendererTracker CreatePageRendererTracker()
        {
            return new CustomShellPageRendererTracker(this);
        }

        protected override IShellSectionRenderer CreateShellSectionRenderer(ShellSection shellSection)
        {
            return new CustomSectionRenderer(this);
        }
    }
}
