// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using De.HDBW.Apollo.Client.ViewModels;

namespace De.HDBW.Apollo.Client.Dialogs
{
    public partial class ConfirmDataUsageDialog
    {
        public ConfirmDataUsageDialog(ConfirmDataUsageDialogViewModel model)
        {
            InitializeComponent();
            var view = FindByName("Part_Root") as View;
            var page = Shell.Current ?? Application.Current?.MainPage;
            if (view != null && page != null)
            {
                view.MaximumWidthRequest = page.Width - 16;
                view.MaximumHeightRequest = page.Height - 16;
                Size = new Size(view.MaximumWidthRequest, view.MinimumHeightRequest);
            }

            BindingContext = model;
        }

        public ConfirmDataUsageDialogViewModel? ViewModel
        {
            get
            {
                return BindingContext as ConfirmDataUsageDialogViewModel;
            }
        }
    }
}
