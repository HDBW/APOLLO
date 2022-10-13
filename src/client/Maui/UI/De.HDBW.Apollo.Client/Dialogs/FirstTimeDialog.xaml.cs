// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using De.HDBW.Apollo.Client.ViewModels;

namespace De.HDBW.Apollo.Client.Dialogs;
public partial class FirstTimeDialog
{
    public FirstTimeDialog(FirstTimeDialogViewModel model)
    {
        InitializeComponent();
        BindingContext = model;
    }

    public FirstTimeDialogViewModel? Viemodel
    {
        get
        {
            return BindingContext as FirstTimeDialogViewModel;
        }
    }
}
