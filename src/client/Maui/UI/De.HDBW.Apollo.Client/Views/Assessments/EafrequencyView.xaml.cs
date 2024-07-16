// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using De.HDBW.Apollo.Client.ViewModels.Assessments;

namespace De.HDBW.Apollo.Client.Views.Assessments
{
    // [XamlCompilation(XamlCompilationOptions.Skip)]
    public partial class EafrequencyView : ContentPage
    {
        public EafrequencyView(EafrequencyViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }

        public EafrequencyViewModel? ViewModel
        {
            get
            {
                return BindingContext as EafrequencyViewModel;
            }
        }
    }
}
