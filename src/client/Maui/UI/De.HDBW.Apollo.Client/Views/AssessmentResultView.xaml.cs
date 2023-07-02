// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Diagnostics;
using De.HDBW.Apollo.Client.ViewModels;

namespace De.HDBW.Apollo.Client.Views;

public partial class AssessmentResultView : ContentPage
{
    public AssessmentResultView(AssessmentResultViewModel model)
    {
#if DEBUG
        Debug.WriteLine($"Create {GetType()}");
#endif
        InitializeComponent();
        BindingContext = model;
    }

    ~AssessmentResultView()
    {
#if DEBUG
        Debug.WriteLine($"~{GetType()}");
#endif
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

    protected override void OnSizeAllocated(double width, double height)
    {
        base.OnSizeAllocated(width, height);

#if IOS
        var grid = Content as Grid;
        if (grid == null)
        {
            return;
        }

        grid.Measure(width, height);

        if (double.IsNaN(height) || double.IsInfinity(height))
        {
            height = Window.Height;
        }

        if (double.IsNaN(width) || double.IsInfinity(width))
        {
            height = Window.Width;
        }

        var heightSum = 0d;
        foreach (var child in grid.Children ?? Array.Empty<IView>())
        {
            switch (child)
            {
                case CarouselView _:
                    break;

                default:
                    var size = child.Measure(grid.Width, grid.Height);
                    heightSum += size.Height;
                    break;
            }
        }

        var diff = height - heightSum;
        PART_ScrollHost.MaximumHeightRequest = diff <= 0 ? double.PositiveInfinity : diff;
#endif
    }
}
