// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using De.HDBW.Apollo.Client.ViewModels.Profile.EducationInfoEditors;

namespace De.HDBW.Apollo.Client.Views.Profile.EducationInfo
{
    public partial class VocationalTrainingView : ContentPage
    {
        public VocationalTrainingView(VocationalTrainingViewModel model)
        {
            InitializeComponent();
            BindingContext = model;
        }

        public VocationalTrainingViewModel? ViewModel
        {
            get
            {
                return BindingContext as VocationalTrainingViewModel;
            }
        }
    }
}