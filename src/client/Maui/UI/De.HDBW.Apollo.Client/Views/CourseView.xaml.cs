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

    protected override void OnSizeAllocated(double width, double height)
    {
        base.OnSizeAllocated(width, height);

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
                case ScrollView _:
                    break;
                default:
                    if (child is Image)
                    {
                        heightSum += 200;
                        continue;
                    }

                    if (child == PART_Deko)
                    {
                        continue;
                    }

                    if (child == PART_Background)
                    {
                        heightSum += 12;
                        continue;
                    }

                    var size = child.Measure(grid.Width, grid.Height);
                    heightSum += size.Height;
                    break;
            }
        }

        var diff = height - heightSum;
        PART_ScrollHost.MaximumHeightRequest = diff <= 0 ? double.PositiveInfinity : diff;
    }
}
