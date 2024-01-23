// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using De.HDBW.Apollo.Client.Helper;
using De.HDBW.Apollo.Client.ViewModels;

namespace De.HDBW.Apollo.Client.Dialogs
{
    public partial class ErrorDialog
    {
        public ErrorDialog(MessageDialogViewModel model)
        {
            InitializeComponent();
            BindingContext = model;
        }

        public MessageDialogViewModel? ViewModel
        {
            get
            {
                return BindingContext as MessageDialogViewModel;
            }
        }

        private void OnSizeChanged(object sender, EventArgs e)
        {
            var button = sender as Button;
            this.FixButtonTextLayout(button);
        }
    }
}