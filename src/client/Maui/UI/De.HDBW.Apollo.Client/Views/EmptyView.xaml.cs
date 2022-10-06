using De.HDBW.Apollo.Client.ViewModels;

namespace De.HDBW.Apollo.Client.Views;
public partial class EmptyView
{
    public EmptyView(EmptyViewModel model)
    {
        InitializeComponent();
        BindingContext = model;
    }

    public EmptyViewModel? Viemodel
    {
        get
        {
            return BindingContext as EmptyViewModel;
        }
    }
}