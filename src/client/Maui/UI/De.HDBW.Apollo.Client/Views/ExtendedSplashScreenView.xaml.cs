// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Diagnostics;
using De.HDBW.Apollo.Client.ViewModels;

namespace De.HDBW.Apollo.Client.Views
{
    public partial class ExtendedSplashScreenView
    {
        public ExtendedSplashScreenView(ExtendedSplashScreenViewModel model)
        {
#if DEBUG
            Debug.WriteLine($"Create {GetType()}");
#endif
            InitializeComponent();
            BindingContext = model;
        }

        ~ExtendedSplashScreenView()
        {
#if DEBUG
            Debug.WriteLine($"~{GetType()}");
#endif
        }

        public ExtendedSplashScreenViewModel? ViewModel
        {
            get
            {
                return BindingContext as ExtendedSplashScreenViewModel;
            }
        }
    }
}
