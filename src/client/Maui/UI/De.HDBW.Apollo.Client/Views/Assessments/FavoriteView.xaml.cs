// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using De.HDBW.Apollo.Client.ViewModels.Assessments;

namespace De.HDBW.Apollo.Client.Views.Assessments
{
    public partial class FavoriteView : ContentPage
    {
        public FavoriteView(FavoriteViewModel model)
        {
            InitializeComponent();
            BindingContext = model;
        }

        public FavoriteViewModel? ViewModel
        {
            get
            {
                return BindingContext as FavoriteViewModel;
            }
        }
    }
}
