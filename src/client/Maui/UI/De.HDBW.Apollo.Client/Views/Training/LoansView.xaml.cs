using System.Diagnostics;
using De.HDBW.Apollo.Client.ViewModels.Training;

namespace De.HDBW.Apollo.Client.Views.Training;

public partial class LoansView : ContentPage
{
    public LoansView(LoansViewModel model)
    {
#if DEBUG
        Debug.WriteLine($"Create {GetType()}");
#endif
        InitializeComponent();
        BindingContext = model;
    }

    ~LoansView()
    {
#if DEBUG
        Debug.WriteLine($"~{GetType()}");
#endif
    }

    public LoansViewModel? ViewModel
    {
        get
        {
            return BindingContext as LoansViewModel;
        }
    }
}
