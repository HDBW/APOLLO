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
