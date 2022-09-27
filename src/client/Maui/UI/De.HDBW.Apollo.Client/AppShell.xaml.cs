namespace De.HDBW.Apollo.Client;

using De.HDBW.Apollo.Client.ViewModels;
public partial class AppShell : Shell
{
    public AppShell(AppShellViewModel model)
    {
        this.InitializeComponent();
        this.BindingContext = model;
    }

    public AppShellViewModel? Viemodel
    {
        get
        {
            return this.BindingContext as AppShellViewModel;
        }
    }
}
