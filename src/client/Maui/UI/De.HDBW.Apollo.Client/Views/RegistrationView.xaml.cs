// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.
#if DEBUG
using System.Diagnostics;
#endif
using De.HDBW.Apollo.Client.ViewModels;

namespace De.HDBW.Apollo.Client.Views
{
    public partial class RegistrationView
    {
        public RegistrationView(RegistrationViewModel model)
        {
#if DEBUG
            Debug.WriteLine($"Create {GetType()}");
#endif
            InitializeComponent();
            BindingContext = model;
        }

        ~RegistrationView()
        {
#if DEBUG
            Debug.WriteLine($"~{GetType()}");
#endif
        }

        public RegistrationViewModel? ViewModel
        {
            get
            {
                return BindingContext as RegistrationViewModel;
            }
        }
    }
}
