// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using De.HDBW.Apollo.Client.ViewModels.Assessments;

namespace De.HDBW.Apollo.Client.Views.Assessments
{
    // [XamlCompilation(XamlCompilationOptions.Skip)]
    public partial class JobSearchResultView : ContentPage
    {
        public JobSearchResultView(JobSearchResultViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }

        public JobSearchResultViewModel? ViewModel
        {
            get
            {
                return BindingContext as JobSearchResultViewModel;
            }
        }
    }
}
