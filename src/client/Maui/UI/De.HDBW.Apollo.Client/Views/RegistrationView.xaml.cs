namespace De.HDBW.Apollo.Client.Views;

using De.HDBW.Apollo.Client.ViewModels;

public partial class RegistrationView
{
    public RegistrationView(RegistrationViewModel model)
    {
        this.InitializeComponent();
        this.BindingContext = model;
    }

    public RegistrationViewModel? Viemodel
    {
        get
        {
            return this.BindingContext as RegistrationViewModel;
        }
    }
}