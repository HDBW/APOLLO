// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Drawing;
using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;
using UIKit;

namespace De.HDBW.Apollo.Client.Platforms.iOS
{
    public class CustomSearchbarHandler : SearchBarHandler
    {
        protected override void ConnectHandler(MauiSearchBar platformView)
        {
            base.ConnectHandler(platformView);
            platformView.CancelButtonClicked += HandleClearButtonClicked;

            var toolbar = new UIToolbar(new RectangleF(0.0f, 0.0f, 50.0f, 44.0f));

            var doneButton = new UIBarButtonItem(Resources.Strings.Resources.Global_Close, UIBarButtonItemStyle.Done, (s, a) =>
            {
                PlatformView.EndEditing(true);
            });

            toolbar.Items = new UIBarButtonItem[]
            {
                    new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace),
                    doneButton,
            };

            PlatformView.InputAccessoryView = toolbar;
        }

        protected override void DisconnectHandler(MauiSearchBar platformView)
        {
            base.DisconnectHandler(platformView);
            var toolbar = PlatformView.InputAccessoryView as UITabBar;
            toolbar?.Dispose();
            PlatformView.AutocorrectionType = UITextAutocorrectionType.No;
            PlatformView.InputAccessoryView = null;
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