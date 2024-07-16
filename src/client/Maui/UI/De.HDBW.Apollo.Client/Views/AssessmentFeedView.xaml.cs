// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using De.HDBW.Apollo.Client.ViewModels.Assessments;

namespace De.HDBW.Apollo.Client.Views
{
    // [XamlCompilation(XamlCompilationOptions.Skip)]
    public partial class AssessmentFeedView : ContentPage
    {
        public AssessmentFeedView(AssessmentFeedViewModel model)
        {
            InitializeComponent();
            BindingContext = model;
        }

        public AssessmentFeedViewModel? ViewModel
        {
            get
            {
                return BindingContext as AssessmentFeedViewModel;
            }
        }
    }
}
