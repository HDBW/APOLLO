// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace De.HDBW.Apollo.Client.Models.Training
{
    public partial class ExpandableItem : ObservableObject
    {
        [ObservableProperty]
        private string _header;

        [ObservableProperty]
        private string _content;

        [ObservableProperty]
        private bool _isExpanded;

        private ExpandableItem(string header, string content, Action<ExpandableItem> changeStateHandler)
        {
            Header = header;
            Content = content;
            ChangeStateHandler = changeStateHandler;
        }

        private Action<ExpandableItem>? ChangeStateHandler { get; }

        public static ExpandableItem Import(
            string header,
            string content,
            Action<ExpandableItem> changeStateHandler)
        {
            return new ExpandableItem(header, content, changeStateHandler);
        }

        [RelayCommand]
        private void ToggleExpandState()
        {
            IsExpanded = !IsExpanded;
            ChangeStateHandler?.Invoke(this);
        }
    }
}
