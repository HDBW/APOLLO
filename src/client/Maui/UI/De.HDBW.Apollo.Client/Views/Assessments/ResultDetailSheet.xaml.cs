// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using De.HDBW.Apollo.Client.ViewModels.Assessments;

namespace De.HDBW.Apollo.Client.Views.Assessments
{
    // [XamlCompilation(XamlCompilationOptions.Skip)]
    public partial class ResultDetailSheet
    {
        public ResultDetailSheet(ResultDetailSheetViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }

        public ResultDetailSheetViewModel? ViewModel
        {
            get
            {
                return BindingContext as ResultDetailSheetViewModel;
            }
        }

        private async void OnShown(object? sender, EventArgs e)
        {
            if (ViewModel == null)
            {
                return;
            }

            await ViewModel.OnNavigatedToAsync();
        }
    }
}
