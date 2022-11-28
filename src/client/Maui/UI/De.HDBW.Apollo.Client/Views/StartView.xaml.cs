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
            doubleValue += ((int)Math.Floor(rows) * 16);
        }

        view.MaximumHeightRequest = Math.Max(doubleValue, view.MinimumHeightRequest);
        view.ItemsSource = model.Interactions;
    }

    private void HandleStateChanged(object sender, EventArgs e)
    {
        var view = sender as VisualElement;
        view?.SetBinding(Border.IsVisibleProperty, new Binding("IsProcessed"));
    }
}
