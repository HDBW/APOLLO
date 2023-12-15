using System.Diagnostics;
using De.HDBW.Apollo.Client.Contracts;
using De.HDBW.Apollo.Client.ViewModels;
using De.HDBW.Apollo.SharedContracts.Services;
using Microsoft.Extensions.Logging;

namespace De.HDBW.Apollo.Client.Views;

public partial class SearchView
{
    public SearchView(SearchViewModel model)
    {
        InitializeComponent();
        BindingContext = model;
        if (ViewModel == null)
        {
            return;
        }
    }

    public SearchView()
    {
        InitializeComponent();
    }

    ~SearchView()
    {
#if DEBUG
        Debug.WriteLine($"~{GetType()}");
#endif
    }

    public SearchViewModel? ViewModel
    {
        get
        {
            return BindingContext as SearchViewModel;
        }
    }

    private async void OnShowFiltersSheetClicked(object sender, EventArgs e)
    {
        if (ViewModel == null || Handler?.MauiContext?.Services == null)
        {
            return;
        }

        // TODO: Show it by using the NavigationService in SearchViewModel.
        var sheet = new FiltersSheet(new FiltersSheetViewModel(
            Handler.MauiContext.Services.GetService<IDispatcherService>(),
            Handler.MauiContext.Services.GetService<INavigationService>(),
            Handler.MauiContext.Services.GetService<IDialogService>(),
            Handler.MauiContext.Services.GetService<ITrainingService>(),
            Handler.MauiContext.Services.GetService<ILogger<FiltersSheetViewModel>>()));

        // Alternatively, pass the window in which the sheet should show. Usually accessible from any other page of the app.
        await sheet.ShowAsync(Window);
    }
}
