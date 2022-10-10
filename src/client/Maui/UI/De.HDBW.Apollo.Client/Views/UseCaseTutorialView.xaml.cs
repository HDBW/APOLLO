using De.HDBW.Apollo.Client.ViewModels;
using Microsoft.Maui.Controls;

namespace De.HDBW.Apollo.Client.Views;
public partial class UseCaseTutorialView
{
    public UseCaseTutorialView(UseCaseTutorialViewModel model)
    {
        InitializeComponent();
        BindingContext = model;
    }

    public UseCaseTutorialViewModel? Viemodel
    {
        get
        {
            return BindingContext as UseCaseTutorialViewModel;
        }
    }

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
        PART_Animation.IsAnimationEnabled = true;
    }

    protected override void OnNavigatedFrom(NavigatedFromEventArgs args)
    {
        base.OnNavigatedFrom(args);
        PART_Animation.IsAnimationEnabled = false;
    }
}
