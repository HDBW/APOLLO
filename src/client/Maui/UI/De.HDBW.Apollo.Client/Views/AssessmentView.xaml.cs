using De.HDBW.Apollo.Client.ViewModels;

namespace De.HDBW.Apollo.Client.Views;

public partial class AssessmentView
{
    public AssessmentView(AssessmentViewModel model)
    {
        InitializeComponent();
        BindingContext = model;
    }

    public AssessmentViewModel? Viemodel
    {
        get
        {
            return BindingContext as AssessmentViewModel;
        }
    }
}
