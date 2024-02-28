// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.ComponentModel;
using System.Diagnostics;
using De.HDBW.Apollo.Client.Contracts;
using De.HDBW.Apollo.Client.ViewModels;

namespace De.HDBW.Apollo.Client.Views;

public partial class FavoriteView
{
    public FavoriteView(FavoriteViewModel model)
    {
        InitializeComponent();
        BindingContext = model;
    }

    public FavoriteView()
    {
        InitializeComponent();
    }

    ~FavoriteView()
    {
#if DEBUG
        Debug.WriteLine($"~{GetType()}");
#endif
    }

    public FavoriteViewModel? ViewModel
    {
        get
        {
            return BindingContext as FavoriteViewModel;
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
        PART_Collection.PropertyChanged += OnPropertyChanged;
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

        collectionView.ScrollTo(0, animate: false);
    }
}
