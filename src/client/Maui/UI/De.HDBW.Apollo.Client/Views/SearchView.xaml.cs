// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.ComponentModel;
using System.Diagnostics;
using CommunityToolkit.Mvvm.Messaging;
using De.HDBW.Apollo.Client.Messages;
using De.HDBW.Apollo.Client.ViewModels;

namespace De.HDBW.Apollo.Client.Views
{
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
            WeakReferenceMessenger.Default.Unregister<FlyoutStateChangedMessage>(this);
            PART_Collection.PropertyChanged -= OnPropertyChanged;
            base.OnDisappearing();
        }

        protected override void OnAppearing()
        {
            PART_SearchHandler.Close();
            PART_SearchHandler.Unfocus();
            base.OnAppearing();
            PART_Collection.PropertyChanged += OnPropertyChanged;
            WeakReferenceMessenger.Default.Register<FlyoutStateChangedMessage>(this, OnFlyoutStateChangedMessage);
        }

        private void OnFlyoutStateChangedMessage(object recipient, FlyoutStateChangedMessage message)
        {
            PART_SearchHandler.Close();
            PART_SearchHandler.Unfocus();
        }

        private void HandleScrolled(object sender, ItemsViewScrolledEventArgs e)
        {
            if (OperatingSystem.IsAndroid())
            {
                PART_SearchHandler.Close();
            }
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
}
