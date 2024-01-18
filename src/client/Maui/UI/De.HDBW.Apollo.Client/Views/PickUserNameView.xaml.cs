// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using De.HDBW.Apollo.Client.ViewModels;

namespace De.HDBW.Apollo.Client.Views
{
    public partial class PickUserNameView
    {
        public PickUserNameView(PickUserNameViewModel model)
        {
            InitializeComponent();
            BindingContext = model;
        }

        public PickUserNameViewModel? ViewModel
        {
            get
            {
                return BindingContext as PickUserNameViewModel;
            }
        }
    }
}
