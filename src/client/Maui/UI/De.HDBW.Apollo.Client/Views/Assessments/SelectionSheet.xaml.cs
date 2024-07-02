// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using CommunityToolkit.Mvvm.Messaging;
using De.HDBW.Apollo.Client.Messages;

namespace De.HDBW.Apollo.Client.Views.Assessments
{
    public partial class SelectionSheet
    {
        public SelectionSheet()
        {
            InitializeComponent();
        }

        private void HandleClicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            WeakReferenceMessenger.Default.Send<SelectionMessage>(new SelectionMessage(button?.Text ?? string.Empty));
        }
    }
}
