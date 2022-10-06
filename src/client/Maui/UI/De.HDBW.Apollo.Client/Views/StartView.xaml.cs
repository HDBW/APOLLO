using De.HDBW.Apollo.Client.ViewModels;

namespace De.HDBW.Apollo.Client.Views;
public partial class StartView
{
    public StartView(StartViewModel model)
    {
        InitializeComponent();
        BindingContext = model;
    }

    public StartViewModel? Viemodel
    {
        get
        {
            return BindingContext as StartViewModel;
        }
    }
}
