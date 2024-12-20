// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using De.HDBW.Apollo.Client.ViewModels.Profile.EducationInfoEditors;

namespace De.HDBW.Apollo.Client.Views.Profile.EducationInfo
{
    [XamlCompilation(XamlCompilationOptions.Skip)]
    public partial class StudyView : ContentPage
    {
        public StudyView(StudyViewModel model)
        {
            InitializeComponent();
            BindingContext = model;
        }

        public StudyView? ViewModel
        {
            get
            {
                return BindingContext as StudyView;
            }
        }
    }
}
