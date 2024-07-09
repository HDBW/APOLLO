// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using De.HDBW.Apollo.Client.ViewModels.Profile.EducationInfoEditors;

namespace De.HDBW.Apollo.Client.Views.Profile.EducationInfo
{
    [XamlCompilation(XamlCompilationOptions.Skip)]
    public partial class CompanyBasedVocationalTrainingView : ContentPage
    {
        public CompanyBasedVocationalTrainingView(CompanyBasedVocationalTrainingViewModel model)
        {
            InitializeComponent();
            BindingContext = model;
        }

        public CompanyBasedVocationalTrainingViewModel? ViewModel
        {
            get
            {
                return BindingContext as CompanyBasedVocationalTrainingViewModel;
            }
        }

        protected override bool OnBackButtonPressed()
        {
            ViewModel?.CancelCommand.Execute(null);
            return true;
        }
    }
}
