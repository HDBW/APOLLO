// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using De.HDBW.Apollo.Client.ViewModels.Profile.EducationInfoEditors;

namespace De.HDBW.Apollo.Client.Views.Profile.EducationInfo
{
    [XamlCompilation(XamlCompilationOptions.Skip)]
    public partial class FurtherEducationView : ContentPage
    {
        public FurtherEducationView(FurtherEducationViewModel model)
        {
            InitializeComponent();
            BindingContext = model;
        }

        public FurtherEducationViewModel? ViewModel
        {
            get
            {
                return BindingContext as FurtherEducationViewModel;
            }
        }
    }
}
