// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using De.HDBW.Apollo.Client.ViewModels;

namespace De.HDBW.Apollo.Client.Views
{
    public partial class SearchFilterSheet
    {
        public SearchFilterSheet()
        {
            InitializeComponent();
        }

        public SearchFilterSheet(SearchFilterSheetViewModel model)
            : this()
        {
            BindingContext = model;
        }

        public SearchFilterSheetViewModel? ViewModel
        {
            get
            {
                return BindingContext as SearchFilterSheetViewModel;
            }
        }

        private async void OnShown(object? sender, EventArgs e)
        {
            if (ViewModel == null)
            {
                return;
            }

            await ViewModel.OnNavigatedToAsync();
        }
    }
}
