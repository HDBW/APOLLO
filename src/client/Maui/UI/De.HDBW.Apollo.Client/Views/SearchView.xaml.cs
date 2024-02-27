// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.ComponentModel;
using System.Diagnostics;
using De.HDBW.Apollo.Client.Contracts;
using De.HDBW.Apollo.Client.ViewModels;

namespace De.HDBW.Apollo.Client.Views;

public partial class SearchView
{
    public SearchView(SearchViewModel model)
    {
        InitializeComponent();
        BindingContext = model;
        if (PART_SearchHandler.QueryIcon != null)
        {
            PART_SearchHandler.QueryIcon.Parent = this;
        }

        if (PART_SearchHandler.ClearIcon != null)
        {
            PART_SearchHandler.ClearIcon.Parent = this;
        }
    }

    public SearchView()
    {
        InitializeComponent();
    }

    ~SearchView()
    {
#if DEBUG
        Debug.WriteLine($"~{GetType()}");
#endif
    }

    public SearchViewModel? ViewModel
    {
        get
        {
            return BindingContext as SearchViewModel;
        }
    }

    protected override void OnDisappearing()
    {
        PART_Collection.PropertyChanged -= OnPropertyChanged;
        base.OnDisappearing();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        Dispatcher.DispatchDelayed(TimeSpan.FromMilliseconds(200), PART_SearchHandler.Close);
        PART_Collection.PropertyChanged += OnPropertyChanged;
    }

    private void HandleScrolled(object sender, ItemsViewScrolledEventArgs e)
    {
        PART_SearchHandler.Close();
    }

    private void OnPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName?.Equals(nameof(CollectionView.ItemsSource)) != true)
        {
            return;
        }

        var collectionView = sender as CollectionView;

        if (collectionView == null)
        {
            return;
        }

        collectionView.ScrollTo(0);
    }
}
