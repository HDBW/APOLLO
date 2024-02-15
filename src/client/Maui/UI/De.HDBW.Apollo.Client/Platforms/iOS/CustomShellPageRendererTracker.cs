using System.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using De.HDBW.Apollo.Client.Messages;
using Microsoft.Maui.Controls.Platform.Compatibility;
using UIKit;

namespace De.HDBW.Apollo.Client.Platforms
{
    public class CustomShellPageRendererTracker : ShellPageRendererTracker
    {
        WeakReference<UISearchController>? _searchController = null;

        public CustomShellPageRendererTracker(IShellContext context)
            : base(context)
        {
        }

        protected override void OnSearchHandlerPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnSearchHandlerPropertyChanged(sender, e);
            if (_searchController == null)
            {
                base.OnSearchHandlerPropertyChanged(sender, new PropertyChangedEventArgs(SearchHandler.SearchBoxVisibilityProperty.PropertyName));
            }
        }

        protected override void UpdateSearchVisibility(UISearchController searchController)
        {
            if (searchController != null)
            {
                _searchController = new WeakReference<UISearchController>(searchController);
            }

            WeakReferenceMessenger.Default.Register<HideSearchSuggestionsMessage>(this, OnHideSearchSuggestions);
            base.UpdateSearchVisibility(searchController);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            WeakReferenceMessenger.Default.Unregister<HideSearchSuggestionsMessage>(this);
            _searchController = null;
        }

        private void OnHideSearchSuggestions(object recipient, HideSearchSuggestionsMessage message)
        {
            if (!(_searchController?.TryGetTarget(out UISearchController? controller) ?? false) || controller == null)
            {
                return;
            }

            controller.Active = false;
        }
    }
}