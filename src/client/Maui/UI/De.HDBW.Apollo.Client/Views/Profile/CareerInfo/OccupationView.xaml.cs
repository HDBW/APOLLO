// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using De.HDBW.Apollo.Client.ViewModels.Profile.CareerInfoEditors;

namespace De.HDBW.Apollo.Client.Views.Profile.CareerInfo
{
    [XamlCompilation(XamlCompilationOptions.Skip)]
    public partial class OccupationView : ContentPage
    {
        public OccupationView(OccupationViewModel model)
        {
            InitializeComponent();
            BindingContext = model;
        }

        public OccupationViewModel? ViewModel
        {
            get
            {
                return BindingContext as OccupationViewModel;
            }
        }
    }
}
