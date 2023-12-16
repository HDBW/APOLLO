using De.HDBW.Apollo.Client.ViewModels;
using Microsoft.Maui.Controls;

namespace De.HDBW.Apollo.Client.Views;

public partial class FiltersSheet
{
    public FiltersSheet(FiltersSheetViewModel model)
    {
        InitializeComponent();
        BindingContext = model;
        if (ViewModel == null)
        {
            return;
        }
    }

    public FiltersSheet()
    {
        InitializeComponent();
    }

    public FiltersSheetViewModel? ViewModel
    {
        get
        {
            return BindingContext as FiltersSheetViewModel;
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
