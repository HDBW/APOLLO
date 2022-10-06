using De.HDBW.Apollo.Client.ViewModels;

namespace De.HDBW.Apollo.Client.Views;
public partial class RegistrationView
{
    public RegistrationView(RegistrationViewModel model)
    {
        InitializeComponent();
        BindingContext = model;
    }

    public RegistrationViewModel? Viemodel
    {
        get
        {
            return BindingContext as RegistrationViewModel;
        }
    }
}
