using De.HDBW.Apollo.Client.ViewModels.Profile.EducationInfoEditors;

namespace De.HDBW.Apollo.Client.Views.Profile.EducationInfo;

public partial class CompanyBasedVocationalTrainingView : ContentPage
{
    public CompanyBasedVocationalTrainingView(CompanyBasedVocationalTrainingViewModel model)
    {
        InitializeComponent();
        BindingContext = model;
    }

    public CompanyBasedVocationalTrainingViewModel? ViewModel
    {
        get
        {
            return BindingContext as CompanyBasedVocationalTrainingViewModel;
        }
    }
}
