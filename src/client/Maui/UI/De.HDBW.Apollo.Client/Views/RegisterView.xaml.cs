// <copyright file="RegisterView.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

#if DEBUG
using System.Diagnostics;
#endif
using De.HDBW.Apollo.Client.ViewModels;

namespace De.HDBW.Apollo.Client.Views
{
    public partial class RegisterView : ContentPage
    {
        public RegisterView(RegisterViewModel model)
        {
#if DEBUG
            Debug.WriteLine($"Create {GetType()}");
#endif
            InitializeComponent();
            BindingContext = model;
        }

        ~RegisterView()
        {
#if DEBUG
            Debug.WriteLine($"~{GetType()}");
#endif
        }

        public RegisterViewModel? ViewModel
        {
            get
            {
                return BindingContext as RegisterViewModel;
            }
        }
    }
}
