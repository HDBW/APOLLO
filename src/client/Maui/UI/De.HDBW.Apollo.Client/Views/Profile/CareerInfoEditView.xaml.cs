// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using De.HDBW.Apollo.Client.ViewModels.Profile;

namespace De.HDBW.Apollo.Client.Views.Profile;

public partial class CareerInfoEditView : ContentPage
{
    public CareerInfoEditView(CareerInfoEditViewModel model)
    {
        InitializeComponent();
        BindingContext = model;
    }

    public CareerInfoEditViewModel? ViewModel
    {
        get
        {
            return BindingContext as CareerInfoEditViewModel;
        }
    }
}
