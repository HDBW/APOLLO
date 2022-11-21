// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using De.HDBW.Apollo.Client.ViewModels;

namespace De.HDBW.Apollo.Client.Views;
public partial class UseCaseDescriptionView
{
    public UseCaseDescriptionView(UseCaseDescriptionViewModel model)
    {
        InitializeComponent();
        BindingContext = model;
    }

    public UseCaseDescriptionViewModel? ViewModel
    {
        get
        {
            return BindingContext as UseCaseDescriptionViewModel;
        }
    }

    private void HandleSizeChanged(object sender, System.EventArgs e)
    {
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
    }
}
