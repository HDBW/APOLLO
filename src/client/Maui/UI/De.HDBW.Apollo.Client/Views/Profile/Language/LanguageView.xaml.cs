// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using De.HDBW.Apollo.Client.ViewModels.Profile.LanguageEditors;

namespace De.HDBW.Apollo.Client.Views.Profile.Language
{
    [XamlCompilation(XamlCompilationOptions.Skip)]
    public partial class LanguageView : ContentPage
    {
        public LanguageView(LanguageViewModel model)
        {
            InitializeComponent();
            BindingContext = model;
        }

        public LanguageViewModel? ViewModel
        {
            get
            {
                return BindingContext as LanguageViewModel;
            }
        }

        protected override bool OnBackButtonPressed()
        {
            ViewModel?.CancelCommand.Execute(null);
            return true;
        }
    }
}
