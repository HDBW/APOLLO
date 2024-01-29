// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using De.HDBW.Apollo.Client.Helper;
using De.HDBW.Apollo.Client.ViewModels;

namespace De.HDBW.Apollo.Client.Dialogs
{
    public partial class ConfirmDataUsageDialog
    {
        public ConfirmDataUsageDialog(ConfirmDataUsageDialogViewModel model)
        {
            InitializeComponent();
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
