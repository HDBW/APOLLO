namespace De.HDBW.Apollo.Client.Views;

using De.HDBW.Apollo.Client.ViewModels;

public partial class StartView
{
    public StartView(StartViewModel model)
    {
        this.InitializeComponent();
        this.BindingContext = model;
    }

    public StartViewModel Viemodel
    {
        get
        {
            return this.BindingContext as StartViewModel;
        }
    }
}
