// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using De.HDBW.Apollo.Client.ViewModels.Profile.LicenseEditors;

namespace De.HDBW.Apollo.Client.Views.Profile.License
{
    // [XamlCompilation(XamlCompilationOptions.Skip)]
    public partial class LicenseView : ContentPage
    {
        public LicenseView(LicenseViewModel model)
        {
            InitializeComponent();
            BindingContext = model;
        }

        public LicenseViewModel? ViewModel
        {
            get
            {
                return BindingContext as LicenseViewModel;
            }
        }

        protected override bool OnBackButtonPressed()
        {
            ViewModel?.CancelCommand.Execute(null);
            return true;
        }
    }
}
