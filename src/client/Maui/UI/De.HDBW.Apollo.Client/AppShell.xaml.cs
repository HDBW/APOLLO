using De.HDBW.Apollo.Client.ViewModels;

namespace De.HDBW.Apollo.Client;
public partial class AppShell : Shell
{
    public AppShell(AppShellViewModel model)
    {
        InitializeComponent();
        BindingContext = model;
    }

    public AppShellViewModel? Viemodel
    {
        get
        {
            return BindingContext as AppShellViewModel;
        }
    }
}
