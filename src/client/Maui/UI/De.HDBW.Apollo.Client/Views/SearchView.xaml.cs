// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Diagnostics;
using De.HDBW.Apollo.Client.ViewModels;

namespace De.HDBW.Apollo.Client.Views;

public partial class SearchView
{
    public SearchView(SearchViewModel model)
    {
        InitializeComponent();
        BindingContext = model;
        PART_SearchHandler.QueryIcon.Parent = this;
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
