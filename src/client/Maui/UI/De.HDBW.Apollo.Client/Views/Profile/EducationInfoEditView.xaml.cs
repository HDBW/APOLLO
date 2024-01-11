// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using De.HDBW.Apollo.Client.ViewModels.Profile;

namespace De.HDBW.Apollo.Client.Views.Profile
{
    public partial class EducationInfoEditView : ContentPage
    {
        public EducationInfoEditView(EducationInfoEditViewModel model)
        {
            InitializeComponent();
            BindingContext = model;
        }

        public EducationInfoEditViewModel? ViewModel
        {
            get
            {
                return BindingContext as EducationInfoEditViewModel;
            }
        }
    }
}
