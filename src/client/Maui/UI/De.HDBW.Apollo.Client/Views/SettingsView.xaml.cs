// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Diagnostics;
using De.HDBW.Apollo.Client.ViewModels;
using Microsoft.Maui.Controls.PlatformConfiguration;
using Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific;

namespace De.HDBW.Apollo.Client.Views;

public partial class SettingsView
{
    public SettingsView(SettingsViewModel model)
    {
#if DEBUG
        Debug.WriteLine($"Create {GetType()}");
#endif
        InitializeComponent();
        BindingContext = model;
    }

    ~SettingsView()
    {
#if DEBUG
        Debug.WriteLine($"~{GetType()}");
#endif
    }

    public SettingsViewModel? ViewModel
    {
        get
        {
            return BindingContext as SettingsViewModel;
        }
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        var thickness = default(Thickness);
#if IOS
        var safeInsets = On<iOS>().SafeAreaInsets();
        safeInsets.Left = 0;
        safeInsets.Right = 0;
        thickness = safeInsets;
#endif

        Padding = thickness;
    }
}
