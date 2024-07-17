// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.ComponentModel;
using System.Drawing;
using CommunityToolkit.Mvvm.Messaging;
using De.HDBW.Apollo.Client.Contracts;
using De.HDBW.Apollo.Client.Messages;
using Microsoft.Maui.Controls.Platform.Compatibility;
using Microsoft.Maui.Platform;
using UIKit;

namespace De.HDBW.Apollo.Client.Platforms
{
    public class CustomShellPageRendererTracker : ShellPageRendererTracker
    {
        private WeakReference<UISearchController>? _searchController = null;
        private WeakReference<UINavigationItem>? _navigationItem = null;
        private IShellContext? _context;

        public CustomShellPageRendererTracker(IShellContext context)
            : base(context)
        {
            _context = context;
        }

        protected override void OnSearchHandlerPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnSearchHandlerPropertyChanged(sender, e);
            if (_searchController == null)
            {
                base.OnSearchHandlerPropertyChanged(sender, new PropertyChangedEventArgs(SearchHandler.SearchBoxVisibilityProperty.PropertyName));
            }
        }

        protected override void OnRendererSet()
        {
            if (ViewController?.NavigationItem != null)
            {
                _navigationItem = new WeakReference<UINavigationItem>(ViewController.NavigationItem);
            }

            base.OnRendererSet();
        }

        protected override void UpdateTitle()
        {
            base.UpdateTitle();
            if (!(_navigationItem?.TryGetTarget(out UINavigationItem? control) ?? false) || control == null)
            {
                return;
            }

            var bar = ViewController?.NavigationController?.NavigationBar;
            if (bar != null)
            {
                bar.SemanticContentAttribute = UISemanticContentAttribute.ForceRightToLeft;
            }
        }

        protected override void UpdateSearchVisibility(UISearchController searchController)
        {
            if (searchController != null)
            {
                _searchController = new WeakReference<UISearchController>(searchController);
                searchController.SearchBar.OnEditingStarted += OnEditingStarted;
                searchController.SearchBar.BackgroundColor = Microsoft.Maui.Graphics.Color.FromArgb("#FEFCF7").ToPlatform();
                searchController.SearchBar.UpdateConstraints();
            }

            WeakReferenceMessenger.Default.Register<HideSearchSuggestionsMessage>(this, OnHideSearchSuggestions);
            base.UpdateSearchVisibility(searchController);
        }

        protected override void RemoveSearchController(UINavigationItem navigationItem)
        {
            base.RemoveSearchController(navigationItem);
        }

        protected override void Dispose(bool disposing)
        {
            var renderen = _context?.CurrentShellItemRenderer as ShellItemRenderer;
            var navigationController = renderen?.SelectedViewController as UINavigationController;
            if (navigationController != null)
            {
                navigationController.InteractivePopGestureRecognizer.Enabled = true;
            }

            _context = null;
            WeakReferenceMessenger.Default.Unregister<HideSearchSuggestionsMessage>(this);
            _searchController = null;
            _navigationItem = null;
            base.Dispose(disposing);
        }

        protected override void OnPageSet(Page oldPage, Page newPage)
        {
            base.OnPageSet(oldPage, newPage);
            var preventBackSwipe = newPage as IPreventBackSwipe ?? newPage?.BindingContext as IPreventBackSwipe;
            var renderen = _context?.CurrentShellItemRenderer as ShellItemRenderer;
            var navigationController = renderen?.SelectedViewController as UINavigationController;
            if (navigationController != null && preventBackSwipe != null)
            {
                navigationController.InteractivePopGestureRecognizer.Enabled = false;
            }
            else if (navigationController != null)
            {
                navigationController.InteractivePopGestureRecognizer.Enabled = true;
            }
        }

        private void OnHideSearchSuggestions(object recipient, HideSearchSuggestionsMessage message)
        {
            if (!(_searchController?.TryGetTarget(out UISearchController? controller) ?? false) || controller == null)
            {
                return;
            }

            if (controller.Active)
            {
                controller.SearchBar.EndEditing(true);
                controller.Active = false;
            }
        }

        private void OnEditingStarted(object? sender, EventArgs e)
        {
            var searchBar = sender as UISearchBar;
            if (searchBar == null)
            {
                return;
            }

            if (searchBar.InputAccessoryView == null)
            {
                var toolbar = new UIToolbar(new RectangleF(0.0f, 0.0f, 50.0f, 44.0f));

                var doneButton = new UIBarButtonItem(Resources.Strings.Resources.Global_Close, UIBarButtonItemStyle.Done, (s, a) =>
                {
                    OnHideSearchSuggestions(this, new HideSearchSuggestionsMessage());
                });

                toolbar.Items = new UIBarButtonItem[]
                {
                    new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace),
                    doneButton,
                };
                searchBar.InputAccessoryView = toolbar;
            }
        }
    }
}
