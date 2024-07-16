// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using De.HDBW.Apollo.Client.ViewModels.Assessments;

namespace De.HDBW.Apollo.Client.Views.Assessments
{
    // [XamlCompilation(XamlCompilationOptions.Skip)]
    public partial class ImagemapView : ContentPage
    {
        public ImagemapView(ImagemapViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }

        public ImagemapViewModel? ViewModel
        {
            get
            {
                return BindingContext as ImagemapViewModel;
            }
        }
    }
}
