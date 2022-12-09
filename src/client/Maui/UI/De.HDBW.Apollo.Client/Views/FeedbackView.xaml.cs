// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Diagnostics;
using De.HDBW.Apollo.Client.ViewModels;

namespace De.HDBW.Apollo.Client.Views;

public partial class FeedbackView
{
    public FeedbackView(FeedbackViewModel model)
    {
#if DEBUG
        Debug.WriteLine($"Create {GetType()}");
#endif
        InitializeComponent();
        BindingContext = model;
    }

    ~FeedbackView()
    {
#if DEBUG
        Debug.WriteLine($"~{GetType()}");
#endif
    }

    public FeedbackViewModel? ViewModel
    {
        get
        {
            return BindingContext as FeedbackViewModel;
        }
    }

    protected override void OnNavigatingFrom(NavigatingFromEventArgs args)
    {
        try
        {
            // Remark: Work around for nullpointer during use of BackButtonBehaviour.
            var behaviour = Shell.GetBackButtonBehavior(this);
            if (behaviour == null)
            {
                return;
            }

            behaviour.ClearValue(BackButtonBehavior.CommandProperty);
            behaviour.BindingContext = null;
            Shell.SetBackButtonBehavior(this, null);
        }
        finally
        {
            base.OnNavigatingFrom(args);
        }
    }

    private void HandleChildAdded(object sender, ElementEventArgs e)
    {
#if IOS
        var layout = sender as Layout;
        if (layout == null)
        {
            return;
        }

        layout.MinimumHeightRequest = 0;
        var size = layout.Measure(double.PositiveInfinity, double.PositiveInfinity, MeasureFlags.None);
        if (double.IsNaN(size.Request.Height) || double.IsInfinity(size.Request.Height))
        {
            return;
        }

        layout.MinimumHeightRequest = Math.Max(size.Request.Height, size.Minimum.Height);
#endif
    }
}
