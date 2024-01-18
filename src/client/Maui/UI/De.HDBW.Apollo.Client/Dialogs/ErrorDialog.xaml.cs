// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using De.HDBW.Apollo.Client.ViewModels;

namespace De.HDBW.Apollo.Client.Dialogs
{
    public partial class ErrorDialog
    {
        public ErrorDialog(MessageDialogViewModel model)
        {
            InitializeComponent();
            var view = FindByName("Part_Root") as View;
            if (view != null && Shell.Current != null)
            {
                view.MaximumWidthRequest = Shell.Current.CurrentPage.Width - 16;
                view.MaximumHeightRequest = Shell.Current.CurrentPage.Height - 16;
                Size = new Size(view.MaximumWidthRequest, view.MinimumHeightRequest);
            }

            BindingContext = model;
        }

        public MessageDialogViewModel? ViewModel
        {
            get
            {
                return BindingContext as MessageDialogViewModel;
            }
        }
    }
}