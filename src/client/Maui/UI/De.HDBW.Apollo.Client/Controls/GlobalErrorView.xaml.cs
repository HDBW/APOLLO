// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using De.HDBW.Apollo.Client.ViewModels;

namespace De.HDBW.Apollo.Client.Controls
{
    public partial class GlobalErrorView : ContentView
    {
        public GlobalErrorView()
        {
            InitializeComponent();
        }

        private GlobalErrorViewModel? ViewModel
        {
            get
            {
                return BindingContext as GlobalErrorViewModel;
            }
        }

        private void HandleUnloaded(object sender, EventArgs e)
        {
            BindingContext = null;
        }

        private void HandleLoaded(object sender, EventArgs e)
        {
            BindingContext = Handler?.MauiContext?.Services?.GetService<GlobalErrorViewModel>();
        }
    }
}
