// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

namespace De.HDBW.Apollo.Client.ViewModels.Profile.ContactInfoEditors
{
    public partial class ContactView : ContentPage
    {
        public ContactView(ContactViewModel model)
        {
            InitializeComponent();
            BindingContext = model;
        }

        public ContactViewModel? ViewModel
        {
            get
            {
                return BindingContext as ContactViewModel;
            }
        }
    }
}
