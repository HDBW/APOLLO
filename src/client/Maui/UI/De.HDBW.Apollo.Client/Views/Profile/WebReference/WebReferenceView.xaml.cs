// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using De.HDBW.Apollo.Client.ViewModels.Profile.WebReferenceEditors;

namespace De.HDBW.Apollo.Client.Views.Profile.WebReference
{
    [XamlCompilation(XamlCompilationOptions.Skip)]
    public partial class WebReferenceView : ContentPage
    {
        public WebReferenceView(WebReferenceViewModel model)
        {
            InitializeComponent();
            BindingContext = model;
        }

        public WebReferenceViewModel? ViewModel
        {
            get
            {
                return BindingContext as WebReferenceViewModel;
            }
        }

        protected override bool OnBackButtonPressed()
        {
            ViewModel?.CancelCommand.Execute(null);
            return true;
        }
    }
}
