// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.
#if DEBUG
using System.Diagnostics;
#endif
using De.HDBW.Apollo.Client.ViewModels;

namespace De.HDBW.Apollo.Client.Views
{
    public partial class SettingsView
    {
        public SettingsView(SettingsViewModel model)
        {
#if DEBUG
            Debug.WriteLine($"Create {GetType()}");
#endif
            InitializeComponent();
            BindingContext = model;
        }

        ~SettingsView()
        {
#if DEBUG
            Debug.WriteLine($"~{GetType()}");
#endif
        }

        public SettingsViewModel? ViewModel
        {
            get
            {
                return BindingContext as SettingsViewModel;
            }
        }
    }
}
