// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace De.HDBW.Apollo.Client.Models.Assessment
{
    public partial class SelectableTextEntry : ObservableObject
    {
        [ObservableProperty]
        private bool _isSelected;

        [ObservableProperty]
        private string _text;

        private SelectableTextEntry(string text)
        {
            Text = text;
        }

        public static SelectableTextEntry Import(string text)
        {
            return new SelectableTextEntry(text);
        }

        [RelayCommand(AllowConcurrentExecutions = false)]
        private Task ToggleSelection(CancellationToken cancellationToken)
        {
            IsSelected = !IsSelected;
            return Task.CompletedTask;
        }
    }
}
