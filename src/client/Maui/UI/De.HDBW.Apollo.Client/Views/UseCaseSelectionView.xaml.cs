// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using De.HDBW.Apollo.Client.ViewModels;

namespace De.HDBW.Apollo.Client.Views;
public partial class UseCaseSelectionView
{
    public UseCaseSelectionView(UseCaseSelectionViewModel model)
    {
        InitializeComponent();
        BindingContext = model;
    }

    public UseCaseSelectionViewModel? ViewModel
    {
        get
        {
            return BindingContext as UseCaseSelectionViewModel;
        }
    }

    protected override bool OnBackButtonPressed()
    {
        return true;
    }

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
#if IOS
        var handler = Handler as Microsoft.Maui.Handlers.PageHandler;
        var renderer = handler?.ViewController as UIKit.UIViewController;
        var gesture = renderer?.NavigationController?.InteractivePopGestureRecognizer;
        if (gesture == null)
        {
            return;
        }

        gesture.Enabled = false;
#endif
    }
}
