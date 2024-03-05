// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using De.HDBW.Apollo.Client.Contracts;
using Microsoft.Maui.Controls.Platform.Compatibility;

namespace De.HDBW.Apollo.Client.Platforms
{
    public class CustomSectionRenderer : ShellSectionRenderer
    {
        public CustomSectionRenderer(IShellContext context)
            : base(context)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            if (ShellSection is IPreventBackSwipe)
            {
                InteractivePopGestureRecognizer.Enabled = false;
            }
        }
    }
}
