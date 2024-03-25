// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Diagnostics;
using De.HDBW.Apollo.Client.ViewModels.Training;

namespace De.HDBW.Apollo.Client.Views.Training
{
    public partial class AppointmentsView : ContentPage
    {
        public AppointmentsView(AppointmentsViewModel model)
        {
#if DEBUG
            Debug.WriteLine($"Create {GetType()}");
#endif
            InitializeComponent();
            BindingContext = model;
        }

        ~AppointmentsView()
        {
#if DEBUG
            Debug.WriteLine($"~{GetType()}");
#endif
        }

        public AppointmentsViewModel? ViewModel
        {
            get
            {
                return BindingContext as AppointmentsViewModel;
            }
        }
    }
}
