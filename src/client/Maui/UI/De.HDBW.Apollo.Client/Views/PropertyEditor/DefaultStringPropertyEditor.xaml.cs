// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using CommunityToolkit.Maui.Core.Platform;

namespace De.HDBW.Apollo.Client.Views.PropertyEditor
{
    public partial class DefaultStringPropertyEditor : ContentView
    {
        public DefaultStringPropertyEditor()
        {
            InitializeComponent();
        }

        private void HandleUnfocused(object sender, FocusEventArgs e)
        {
            var entry = sender as Entry;
            if (!(entry?.IsFocused ?? true))
            {
                return;
            }

            entry?.HideKeyboardAsync(CancellationToken.None);
        }
    }
}
