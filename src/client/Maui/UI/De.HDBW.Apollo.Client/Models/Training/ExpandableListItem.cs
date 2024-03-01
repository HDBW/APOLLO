// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace De.HDBW.Apollo.Client.Models.Training
{
    public partial class ExpandableListItem : ObservableObject
    {
        [ObservableProperty]
        private string _header;

        [ObservableProperty]
        private ObservableCollection<string> _content;

        [ObservableProperty]
        private bool _isExpanded;

        private ExpandableListItem(string header, IEnumerable<string> content, Action<ExpandableListItem> changeStateHandler)
        {
            Header = header;
            Content = new ObservableCollection<string>(content);
            ChangeStateHandler = changeStateHandler;
        }

        private Action<ExpandableListItem>? ChangeStateHandler { get; }

        public static ExpandableListItem Import(
            string header,
            IEnumerable<string> content,
            Action<ExpandableListItem> changeStateHandler)
        {
            return new ExpandableListItem(header, content, changeStateHandler);
        }

        [RelayCommand]
        private void ToggleExpandState()
        {
            IsExpanded = !IsExpanded;
            ChangeStateHandler?.Invoke(this);
        }
    }
}
