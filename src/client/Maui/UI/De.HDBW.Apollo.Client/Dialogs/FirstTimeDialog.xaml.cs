// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using De.HDBW.Apollo.Client.ViewModels;

namespace De.HDBW.Apollo.Client.Dialogs
{
    public partial class FirstTimeDialog : Dialog
    {
        public FirstTimeDialog(FirstTimeDialogViewModel model)
            : base()
        {
            InitializeComponent();
            BindingContext = model;
        }

        public FirstTimeDialogViewModel? ViewModel
        {
            get
            {
                return BindingContext as FirstTimeDialogViewModel;
            }
        }
    }
}
