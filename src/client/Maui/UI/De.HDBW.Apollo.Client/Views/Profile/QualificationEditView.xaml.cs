// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using De.HDBW.Apollo.Client.ViewModels.Profile;

namespace De.HDBW.Apollo.Client.Views.Profile
{
    public partial class QualificationEditView : ContentPage
    {
        public QualificationEditView(QualificationEditViewModel model)
        {
            InitializeComponent();
            BindingContext = model;
        }

        public QualificationEditViewModel? ViewModel
        {
            get
            {
                return BindingContext as QualificationEditViewModel;
            }
        }
    }
}