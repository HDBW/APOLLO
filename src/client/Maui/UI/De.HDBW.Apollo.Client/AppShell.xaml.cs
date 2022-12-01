// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using De.HDBW.Apollo.Client.ViewModels;
#if IOS
using Microsoft.Maui.Controls.Compatibility.Platform.iOS;
#endif

namespace De.HDBW.Apollo.Client;
public partial class AppShell : Shell
{
    public AppShell(AppShellViewModel model)
    {
        InitializeComponent();
        BindingContext = model;
#if IOS
        var gesture = this.GetRenderer()?.ViewController?.NavigationController?.InteractivePopGestureRecognizer;
        if (gesture == null)
        {
            return;
        }

        gesture.Enabled = false;
#endif
    }

    public AppShellViewModel? ViewModel
    {
        get
        {
            return BindingContext as AppShellViewModel;
        }
    }
}
