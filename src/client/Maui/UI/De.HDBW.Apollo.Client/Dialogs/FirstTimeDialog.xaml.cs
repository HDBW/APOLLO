namespace De.HDBW.Apollo.Client.Dialogs;

using De.HDBW.Apollo.Client.ViewModels;

public partial class FirstTimeDialog
{
    public FirstTimeDialog(FirstTimeDialogViewModel model)
    {
        this.InitializeComponent();
        this.BindingContext = model;
    }

    public FirstTimeDialogViewModel? Viemodel
    {
        get
        {
            return this.BindingContext as FirstTimeDialogViewModel;
        }
    }
}