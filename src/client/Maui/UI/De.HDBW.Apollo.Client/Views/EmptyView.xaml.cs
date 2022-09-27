namespace De.HDBW.Apollo.Client.Views;

using De.HDBW.Apollo.Client.ViewModels;
public partial class EmptyView
{
    public EmptyView(EmptyViewModel model)
    {
        this.InitializeComponent();
        this.BindingContext = model;
    }

    public EmptyViewModel? Viemodel
    {
        get
        {
            return this.BindingContext as EmptyViewModel;
        }
    }
}