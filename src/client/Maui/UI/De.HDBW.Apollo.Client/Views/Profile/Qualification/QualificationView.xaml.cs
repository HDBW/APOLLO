// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using De.HDBW.Apollo.Client.ViewModels.Profile.QualificationEditors;

namespace De.HDBW.Apollo.Client.Views.Profile.Qualification
{
    [XamlCompilation(XamlCompilationOptions.Skip)]
    public partial class QualificationView : ContentPage
    {
        public QualificationView(QualificationViewModel model)
        {
            InitializeComponent();
            BindingContext = model;
        }

        public QualificationViewModel? ViewModel
        {
            get
            {
                return BindingContext as QualificationViewModel;
            }
        }
    }
}
