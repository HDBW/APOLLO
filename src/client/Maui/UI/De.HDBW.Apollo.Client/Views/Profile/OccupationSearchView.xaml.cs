// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using De.HDBW.Apollo.Client.ViewModels.Profile;

namespace De.HDBW.Apollo.Client.Views.Profile
{
    public partial class OccupationSearchView : ContentPage
    {
        public OccupationSearchView(OccupationSearchViewModel model)
        {
            InitializeComponent();
            BindingContext = model;
        }

        public OccupationSearchViewModel? ViewModel
        {
            get
            {
                return BindingContext as OccupationSearchViewModel;
            }
        }

        private void HandleScrolled(object sender, ItemsViewScrolledEventArgs e)
        {
            if (PART_SearchBar.IsFocused)
            {
                PART_SearchBar.Unfocus();
            }
        }
    }
}
