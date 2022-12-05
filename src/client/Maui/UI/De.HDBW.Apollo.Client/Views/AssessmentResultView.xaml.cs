// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using De.HDBW.Apollo.Client.ViewModels;

namespace De.HDBW.Apollo.Client.Views;

public partial class AssessmentResultView : ContentPage
{
    public AssessmentResultView(AssessmentResultViewModel model)
    {
        InitializeComponent();
        BindingContext = model;
    }

    public AssessmentResultViewModel? ViewModel
    {
        get
        {
            return BindingContext as AssessmentResultViewModel;
        }
    }

    protected override void OnNavigatingFrom(NavigatingFromEventArgs args)
    {
        // Remark: Work around for nullpointer during use of BackButtonBehaviour.
        var behaviour = Shell.GetBackButtonBehavior(this);
        behaviour?.ClearValue(BackButtonBehavior.CommandProperty);
    }

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        // Remark: Work around for nullpointer during use of BackButtonBehaviour.
        var behaviour = Shell.GetBackButtonBehavior(this);
        var binding = new Binding();
        binding.Path = "ConfirmCommand";
        behaviour.SetBinding(BackButtonBehavior.CommandProperty, binding);
    }
}
