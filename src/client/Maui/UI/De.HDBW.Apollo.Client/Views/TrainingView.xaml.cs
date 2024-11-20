// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.
#if DEBUG
using System.Diagnostics;
#endif
using De.HDBW.Apollo.Client.ViewModels;

namespace De.HDBW.Apollo.Client.Views
{
    public partial class TrainingView
    {
        public TrainingView(TrainingViewModel model)
        {
#if DEBUG
            Debug.WriteLine($"Create {GetType()}");
#endif
            InitializeComponent();
            BindingContext = model;
        }

        ~TrainingView()
        {
#if DEBUG
            Debug.WriteLine($"~{GetType()}");
#endif
        }

        public TrainingViewModel? ViewModel
        {
            get
            {
                return BindingContext as TrainingViewModel;
            }
        }

        protected override bool OnBackButtonPressed()
        {
            ViewModel?.NavigateBackCommand.Execute(null);
            return true;
        }
    }
}
