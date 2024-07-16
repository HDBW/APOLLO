// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using De.HDBW.Apollo.Client.ViewModels.Assessments;

namespace De.HDBW.Apollo.Client.Views.Assessments
{
    // [XamlCompilation(XamlCompilationOptions.Skip)]
    public partial class BinaryView : ContentPage
    {
        public BinaryView(BinaryViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }

        public BinaryViewModel? ViewModel
        {
            get
            {
                return BindingContext as BinaryViewModel;
            }
        }
    }
}
