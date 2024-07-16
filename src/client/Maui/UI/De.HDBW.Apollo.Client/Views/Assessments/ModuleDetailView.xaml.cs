// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using CommunityToolkit.Mvvm.Messaging;
using De.HDBW.Apollo.Client.Messages;
using De.HDBW.Apollo.Client.ViewModels.Assessments;

namespace De.HDBW.Apollo.Client.Views.Assessments
{
    // [XamlCompilation(XamlCompilationOptions.Skip)]
    public partial class ModuleDetailView : ContentPage
    {
        public ModuleDetailView(ModuleDetailViewModel model)
        {
            InitializeComponent();
            BindingContext = model;
            WeakReferenceMessenger.Default.Register<UpdateToolbarMessage>(this, RefreshToolbarItems);
        }

        public ModuleDetailViewModel? ViewModel
        {
            get
            {
                return BindingContext as ModuleDetailViewModel;
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            RefreshToolbarItems(this, new UpdateToolbarMessage());
        }

        private void RefreshToolbarItems(object recipient, UpdateToolbarMessage message)
        {
            SetToolbarItemVisibility(PART_SwitchLanguage, ViewModel?.HasLanguageSelection ?? false);
        }

        private void SetToolbarItemVisibility(ToolbarItem toolbarItem, bool value)
        {
            if (value && !ToolbarItems.Contains(toolbarItem))
            {
                ToolbarItems.Add(toolbarItem);
                toolbarItem.Command = ViewModel?.OpenLanguageSelectionCommand;
            }
            else if (!value)
            {
                ToolbarItems.Remove(toolbarItem);
            }
        }
    }
}
