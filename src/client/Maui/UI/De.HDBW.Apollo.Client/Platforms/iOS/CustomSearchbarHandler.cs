// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;

namespace De.HDBW.Apollo.Client.Platforms.iOS
{
    public class CustomSearchbarHandler : SearchBarHandler
    {
        protected override void ConnectHandler(MauiSearchBar platformView)
        {
            base.ConnectHandler(platformView);
            platformView.CancelButtonClicked += HandleClearButtonClicked;
        }

        protected override void DisconnectHandler(MauiSearchBar platformView)
        {
            base.DisconnectHandler(platformView);
            platformView.CancelButtonClicked -= HandleClearButtonClicked;
        }

        private void HandleClearButtonClicked(object? sender, EventArgs e)
        {
            var searchBar = sender as MauiSearchBar;
            if (searchBar == null)
            {
                return;
            }

            if (searchBar.IsFirstResponder || searchBar.Focused)
            {
                return;
            }

            searchBar.Focus(new FocusRequest());
        }
    }
}