// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using De.HDBW.Apollo.Client.ViewModels.Assessments;

namespace De.HDBW.Apollo.Client.Views.Assessments
{
    // [XamlCompilation(XamlCompilationOptions.Skip)]
    public partial class ResultOverView : ContentPage
    {
        public ResultOverView(ResultOverViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }

        public ResultOverViewModel? ViewModel
        {
            get
            {
                return BindingContext as ResultOverViewModel;
            }
        }

        protected override bool OnBackButtonPressed()
        {
            ViewModel?.NavigateBackCommand.Execute(null);
            return true;
        }
    }
}
