// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.ComponentModel;
using System.Diagnostics;
using De.HDBW.Apollo.Client.Contracts;
using De.HDBW.Apollo.Client.ViewModels;

namespace De.HDBW.Apollo.Client.Views;

public partial class FavoriteView
{
    public FavoriteView(FavoriteViewModel model)
    {
        InitializeComponent();
        BindingContext = model;
    }

    public FavoriteView()
    {
        InitializeComponent();
    }

    ~FavoriteView()
    {
#if DEBUG
        Debug.WriteLine($"~{GetType()}");
#endif
    }

    public FavoriteViewModel? ViewModel
    {
        get
        {
            return BindingContext as FavoriteViewModel;
        }
    }
}
