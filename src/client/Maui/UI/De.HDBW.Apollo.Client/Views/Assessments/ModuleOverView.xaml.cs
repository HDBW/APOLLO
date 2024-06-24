// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using De.HDBW.Apollo.Client.ViewModels.Assessments;

namespace De.HDBW.Apollo.Client.Views.Assessments
{
    [XamlCompilation(XamlCompilationOptions.Skip)]
    public partial class ModuleOverView : ContentPage
    {
        public ModuleOverView(ModuleOverViewModel model)
        {
            InitializeComponent();
            BindingContext = model;
        }

        public ModuleOverViewModel? ViewModel
        {
            get
            {
                return BindingContext as ModuleOverViewModel;
            }
        }
    }
}
