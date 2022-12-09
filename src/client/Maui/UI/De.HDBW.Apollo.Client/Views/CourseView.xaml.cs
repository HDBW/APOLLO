using System.Diagnostics;
using De.HDBW.Apollo.Client.ViewModels;

namespace De.HDBW.Apollo.Client.Views;

public partial class CourseView
{
    public CourseView(CourseViewModel model)
    {
#if DEBUG
        Debug.WriteLine($"Create {GetType()}");
#endif
        InitializeComponent();
        BindingContext = model;
    }

    ~CourseView()
    {
#if DEBUG
        Debug.WriteLine($"~{GetType()}");
#endif
    }

    public CourseViewModel? ViewModel
    {
        get
        {
            return BindingContext as CourseViewModel;
        }
    }

    private void HandleChildAdded(object sender, ElementEventArgs e)
    {
#if IOS
        UpdateMinHeight(sender as Layout);
#endif
    }

    private void HandleSizeChanged(object sender, EventArgs e)
    {
#if IOS
        UpdateMinHeight(sender as Layout);
#endif
    }

    private void UpdateMinHeight(Layout? layout)
    {
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
