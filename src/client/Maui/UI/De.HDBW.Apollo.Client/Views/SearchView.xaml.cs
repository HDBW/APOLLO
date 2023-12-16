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
}
