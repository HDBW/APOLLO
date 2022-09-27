namespace De.HDBW.Apollo.Client.Views;

using De.HDBW.Apollo.Client.ViewModels;

public partial class UseCaseSelectionView
{
    public UseCaseSelectionView(UseCaseSelectionViewModel model)
    {
        this.InitializeComponent();
        this.BindingContext = model;
    }

    public UseCaseSelectionViewModel Viemodel
    {
        get
        {
            return this.BindingContext as UseCaseSelectionViewModel;
        }
    }
}