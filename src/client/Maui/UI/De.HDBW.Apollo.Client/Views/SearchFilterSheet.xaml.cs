using De.HDBW.Apollo.Client.ViewModels;

namespace De.HDBW.Apollo.Client.Views;

public partial class SearchFilterSheet
{
    public SearchFilterSheet(SearchFilterSheetViewModel model)
    {
        InitializeComponent();
        BindingContext = model;
        if (ViewModel == null)
        {
            return;
        }
    }

    public SearchFilterSheet()
    {
        InitializeComponent();
    }

    public SearchFilterSheetViewModel? ViewModel
    {
        get
        {
            return BindingContext as SearchFilterSheetViewModel;
        }
    }

    protected override void OnBindingContextChanged()
    {
        base.OnBindingContextChanged();
        Shown += OnShown;
    }

    private async void OnShown(object? sender, EventArgs e)
    {
        if (ViewModel == null)
        {
            return;
        }

        await ViewModel.OnNavigatedToAsync();
    }
}
