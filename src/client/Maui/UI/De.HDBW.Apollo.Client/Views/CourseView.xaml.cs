using De.HDBW.Apollo.Client.ViewModels;

namespace De.HDBW.Apollo.Client.Views;

public partial class CourseView : ContentView
{
    public CourseView(CourseViewModel model)
    {
        InitializeComponent();
        BindingContext = model;
    }

    public CourseViewModel? ViewModel
    {
        get
        {
            return BindingContext as CourseViewModel;
        }
    }
}
