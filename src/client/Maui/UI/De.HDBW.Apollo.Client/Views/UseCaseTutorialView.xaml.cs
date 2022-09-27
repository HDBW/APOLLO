namespace De.HDBW.Apollo.Client.Views;

using De.HDBW.Apollo.Client.ViewModels;
using Microsoft.Maui.Controls;

public partial class UseCaseTutorialView
{
    public UseCaseTutorialView(UseCaseTutorialViewModel model)
    {
        this.InitializeComponent();
        this.BindingContext = model;
    }

    public UseCaseTutorialViewModel? Viemodel
    {
        get
        {
            return this.BindingContext as UseCaseTutorialViewModel;
        }
    }

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
        this.PART_Animation.IsAnimationEnabled = true;
    }
}