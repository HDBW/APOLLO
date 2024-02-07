// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using De.HDBW.Apollo.Client.ViewModels;

namespace De.HDBW.Apollo.Client.Dialogs
{
    public partial class SelectOptionDialog
    {
        public SelectOptionDialog(SelectOptionDialogViewModel model)
            : base()
        {
            InitializeComponent();
            BindingContext = model;
        }

        public SelectOptionDialogViewModel? ViewModel
        {
            get
            {
                return BindingContext as SelectOptionDialogViewModel;
            }
        }
    }
}