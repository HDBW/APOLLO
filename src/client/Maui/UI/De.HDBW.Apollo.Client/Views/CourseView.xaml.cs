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
}
