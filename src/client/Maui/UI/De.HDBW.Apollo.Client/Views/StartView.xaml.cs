// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Diagnostics;
using De.HDBW.Apollo.Client.ViewModels;

namespace De.HDBW.Apollo.Client.Views;
public partial class StartView
{
    public StartView(StartViewModel model)
    {
#if DEBUG
        Debug.WriteLine($"Create {GetType()}");
#endif
        InitializeComponent();
        BindingContext = model;
        if (ViewModel == null)
        {
            return;
        }

        ViewModel.UseCaseChangedHandler = ResetScrollViewer;
    }

    ~StartView()
    {
#if DEBUG
        Debug.WriteLine($"~{GetType()}");
#endif
    }

    public StartViewModel? ViewModel
    {
        get
        {
            return BindingContext as StartViewModel;
        }
    }

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
#if DEBUG
        GC.Collect();
        GC.WaitForPendingFinalizers();
#endif
#if IOS
        var handler = Handler as Microsoft.Maui.Handlers.PageHandler;
        var renderer = handler?.ViewController as UIKit.UIViewController;
        var gesture = renderer?.NavigationController?.InteractivePopGestureRecognizer;
        if (gesture == null)
        {
            return;
        }

        gesture.Enabled = true;
#endif
        if (!IsLoaded)
        {
            return;
        }

        ViewModel?.LoadDataCommand?.Execute(null);
    }

    private void HandleStateChanged(object sender, EventArgs e)
    {
        var view = sender as VisualElement;
        view?.SetBinding(Border.IsVisibleProperty, new Binding("IsProcessed"));
    }

    private async void ResetScrollViewer()
    {
        if (PART_ScrollHost == null)
        {
            return;
        }

        await PART_ScrollHost.ScrollToAsync(0, 0, false);
    }
}
