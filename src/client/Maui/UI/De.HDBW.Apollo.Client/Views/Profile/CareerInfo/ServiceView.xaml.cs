// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using De.HDBW.Apollo.Client.ViewModels.Profile.CareerInfoEditors;

namespace De.HDBW.Apollo.Client.Views.Profile.CareerInfo
{
    public partial class ServiceView : ContentPage
    {
        public ServiceView(ServiceViewModel model)
        {
            InitializeComponent();
            BindingContext = model;
        }

        public ServiceViewModel? ViewModel
        {
            get
            {
                return BindingContext as ServiceViewModel;
            }
        }
    }
}
