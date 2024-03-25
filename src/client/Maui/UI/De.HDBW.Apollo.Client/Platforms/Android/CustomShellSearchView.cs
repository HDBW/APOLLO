// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using Android.Content;
using AndroidX.AppCompat.Widget;
using AndroidX.CardView.Widget;
using Microsoft.Maui.Controls.Platform.Compatibility;
using Microsoft.Maui.Platform;

namespace De.HDBW.Apollo.Client.Platforms
{
    public class CustomShellSearchView
        : ShellSearchView
    {
        public CustomShellSearchView(Context context, IShellContext shellContext)
            : base(context, shellContext)
        {
        }

        protected override void Dispose(bool disposing)
        {
            var textedit = this.GetChildrenOfType<AppCompatAutoCompleteTextView>().FirstOrDefault();
            if (textedit != null)
            {
                textedit.FocusChange -= OnFocusChanged;
            }

            base.Dispose(disposing);
        }

        protected override void LoadView(SearchHandler searchHandler)
        {
            base.LoadView(searchHandler);

            var rootLayout = this.GetChildrenOfType<CardView>().FirstOrDefault();
            if (rootLayout == null)
            {
                return;
            }

            rootLayout.SetCardBackgroundColor(Colors.White.ToInt());
            rootLayout.Focusable = true;
            rootLayout.FocusableInTouchMode = true;
            var textedit = this.GetChildrenOfType<AppCompatAutoCompleteTextView>().FirstOrDefault();
            if (textedit != null)
            {
                textedit.FocusChange += OnFocusChanged;
            }
        }

        private void OnFocusChanged(object? sender, FocusChangeEventArgs e)
        {
            if (SearchHandler == null || SearchHandler.IsFocused == e.HasFocus)
            {
                return;
            }

            SearchHandler?.SetIsFocused(e.HasFocus);
            if (!e.HasFocus)
            {
                var textView = sender as AppCompatAutoCompleteTextView;
                textView?.ClearFocus();
            }
        }
    }
}
