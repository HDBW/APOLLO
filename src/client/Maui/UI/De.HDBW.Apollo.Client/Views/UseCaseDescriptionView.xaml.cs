// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.
#if DEBUG
using System.Diagnostics;
#endif
using De.HDBW.Apollo.Client.ViewModels;

namespace De.HDBW.Apollo.Client.Views
{
    public partial class UseCaseDescriptionView
    {
        public UseCaseDescriptionView(UseCaseDescriptionViewModel model)
        {
#if DEBUG
            Debug.WriteLine($"Create {GetType()}");
#endif
            InitializeComponent();
            BindingContext = model;
        }

        ~UseCaseDescriptionView()
        {
#if DEBUG
            Debug.WriteLine($"~{GetType()}");
#endif
        }

        public UseCaseDescriptionViewModel? ViewModel
        {
            get
            {
                return BindingContext as UseCaseDescriptionViewModel;
            }
        }
    }
}
