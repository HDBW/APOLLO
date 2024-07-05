// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using CommunityToolkit.Mvvm.Messaging;
using De.HDBW.Apollo.Client.Messages;
using De.HDBW.Apollo.Client.ViewModels.Assessments;

namespace De.HDBW.Apollo.Client.Views.Assessments
{
    [XamlCompilation(XamlCompilationOptions.Skip)]
    public partial class SelectionSheet
    {
        public SelectionSheet()
        {
            InitializeComponent();
        }

        public SelectionSheet(SelectionSheetViewModel model)
            : this()
        {
            BindingContext = model;
        }

        public SelectionSheetViewModel? ViewModel
        {
            get
            {
                return BindingContext as SelectionSheetViewModel;
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
