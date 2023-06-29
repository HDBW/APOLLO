// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Diagnostics;
using De.HDBW.Apollo.Client.ViewModels;
using Microsoft.Maui.Controls.PlatformConfiguration;
using Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific;

namespace De.HDBW.Apollo.Client.Views;
public partial class ExtendedSplashScreenView
{
    public ExtendedSplashScreenView(ExtendedSplashScreenViewModel model)
    {
#if DEBUG
        Debug.WriteLine($"Create {GetType()}");
#endif
        InitializeComponent();
        BindingContext = model;
    }

    ~ExtendedSplashScreenView()
    {
#if DEBUG
        Debug.WriteLine($"~{GetType()}");
#endif
    }

    public ExtendedSplashScreenViewModel? ViewModel
    {
        get
        {
            return BindingContext as ExtendedSplashScreenViewModel;
        }
    }

    private void HandleSizeChanged(Object sender, System.EventArgs e)
    {
#if IOS
        var view = sender as IView;
        var grid = view as Grid;
        if (grid == null)
        {
            return;
        }

        var height = grid.Height;
        var width = grid.Width;
        if (double.IsNaN(height) || double.IsInfinity(height))
        {
            height = Window.Height;
        }

        if (double.IsNaN(width) || double.IsInfinity(width))
        {
            height = Window.Width;
        }

        var heightSum = 0d;
        foreach (var child in grid.Children ?? Array.Empty<IView>())
        {
            switch (child)
            {
                case CarouselView _:
                    break;

                default:
                    var handler = child.Handler;
                    var size = handler?.GetDesiredSize(grid.Width, grid.Height) ?? Size.Zero;
                    heightSum += size.Height + child.Margin.Top + child.Margin.Bottom;
                    break;
            }
        }

        var diff = height - heightSum;
        PART_Animation.MaximumHeightRequest = diff <= 0 ? double.PositiveInfinity : diff;
#endif
    }
}
