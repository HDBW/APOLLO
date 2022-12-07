// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Globalization;
using De.HDBW.Apollo.Client.Converter;
using De.HDBW.Apollo.Client.Models.Interactions;
using De.HDBW.Apollo.Client.ViewModels;
using Microsoft.Maui.Controls;

namespace De.HDBW.Apollo.Client.Views;
public partial class StartView
{
    public StartView(StartViewModel model)
    {
        InitializeComponent();
        BindingContext = model;
        if (ViewModel == null)
        {
            return;
        }

        ViewModel.UseCaseChangedHandler = ResetScrollViewer;
    }

    public StartViewModel? ViewModel
    {
        get
        {
            return BindingContext as StartViewModel;
        }
    }

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
#if IOS
        var handler = Handler as Microsoft.Maui.Handlers.PageHandler;
        var renderer = handler?.ViewController as UIKit.UIViewController;
        var gesture = renderer?.NavigationController?.InteractivePopGestureRecognizer;
        if (gesture == null)
        {
            return;
        }

        gesture.Enabled = true;
#endif
        if (!IsLoaded)
        {
            return;
        }

        ViewModel?.LoadDataCommand?.Execute(null);
    }

    private void HandleStateChanged(object sender, EventArgs e)
    {
        var view = sender as VisualElement;
        view?.SetBinding(Border.IsVisibleProperty, new Binding("IsProcessed"));
    }

    private async void ResetScrollViewer()
    {
        if (PART_ScrollHost == null)
        {
            return;
        }

        await PART_ScrollHost.ScrollToAsync(0, 0, false);
        HandleSizeChanged(PART_ScrollHost, null);
    }

    private void HandleSizeChanged(object sender, EventArgs? e)
    {
#if IOS
        var layout = PART_ScrollHost?.Content;
        if (layout == null)
        {
            return;
        }

        layout.MinimumHeightRequest = 0;
        var size = layout.Measure(double.PositiveInfinity, double.PositiveInfinity, MeasureFlags.None);
        layout.MinimumHeightRequest = Math.Max(size.Request.Height, size.Minimum.Height);

        if (layout.MinimumHeightRequest != PART_ScrollHost?.ContentSize.Height)
        {
        }
#endif
    }
}
