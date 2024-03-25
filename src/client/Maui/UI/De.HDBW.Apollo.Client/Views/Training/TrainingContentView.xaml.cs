// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using De.HDBW.Apollo.Client.ViewModels.Training;

namespace De.HDBW.Apollo.Client.Views.Training
{
    [XamlCompilation(XamlCompilationOptions.Skip)]
    public partial class TrainingContentView : ContentPage
    {
        public TrainingContentView(TrainingContentViewModel model)
        {
#if DEBUG
            System.Diagnostics.Debug.WriteLine($"Create {GetType()}");
#endif
            InitializeComponent();
            BindingContext = model;
        }

        ~TrainingContentView()
        {
#if DEBUG
            System.Diagnostics.Debug.WriteLine($"~{GetType()}");
#endif
        }

        public TrainingContentViewModel? ViewModel
        {
            get
            {
                return BindingContext as TrainingContentViewModel;
            }
        }
    }
}
