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

    private void HandleBindingContextChanged(object sender, System.EventArgs e)
    {
        var view = sender as CollectionView;
        if (view == null)
        {
            return;
        }

        var model = view.BindingContext as InteractionCategoryEntry;
        if (model == null)
        {
            return;
        }

        var converter = new InteractionsToMaximumHeightConverter();
        var value = converter.Convert(model.Interactions, typeof(double), 240d, CultureInfo.CurrentUICulture);
        var doubleValue = 0d;
        if (value is double)
        {
            doubleValue = (double)value;
            var rows = doubleValue / 240d;
            doubleValue += (int)Math.Floor(rows) * 16;
        }

        view.MaximumHeightRequest = Math.Max(doubleValue, view.MinimumHeightRequest);
        view.ItemsSource = model.Interactions;
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
    }

    private void HandleSizeChanged(object sender, System.EventArgs e)
    {
#if IOS
        var layout = sender as Layout;
        if (layout == null)
        {
            return;
        }

        layout.MinimumHeightRequest = 0;
        var size = layout.Measure(double.PositiveInfinity, double.PositiveInfinity, MeasureFlags.None);
        if (double.IsNaN(size.Request.Height) || double.IsInfinity(size.Request.Height))
        {
            return;
        }

        layout.MinimumHeightRequest = Math.Max(size.Request.Height, size.Minimum.Height);
#endif
    }
}
