// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using De.HDBW.Apollo.Client.Helper;
using Invite.Apollo.App.Graph.Common.Models.Trainings;

namespace De.HDBW.Apollo.Client.Models.Training
{
    public partial class OccurenceItem : ObservableObject
    {
        private Occurence _occurence;

        [ObservableProperty]
        private string? _header;

        [ObservableProperty]
        private ObservableCollection<LineItem> _items = new ObservableCollection<LineItem>();

        [ObservableProperty]
        private bool _isExpanded;

        private OccurenceItem(Occurence occurence, Action<OccurenceItem> changeStateHandler)
        {
            _occurence = occurence;
            Header = $"{occurence.StartDate.ToUIDate()} - {occurence.EndDate.ToUIDate()}";
            if (occurence.Location != null)
            {
                Items = ContactItem.Import(null, occurence.Location, null, null, null, null).Items;
            }

            if (!string.IsNullOrWhiteSpace(occurence.Description))
            {
                Items.Add(LineItem.Import(null, occurence.Description));
            }
        }

        private Action<OccurenceItem>? ChangeStateHandler { get; }

        public static OccurenceItem Import(Occurence occurence, Action<OccurenceItem> changeStateHandler)
        {
            return new OccurenceItem(occurence, changeStateHandler);
        }

        [RelayCommand]
        private void ToggleExpandState()
        {
            IsExpanded = !IsExpanded;
            ChangeStateHandler?.Invoke(this);
        }
    }
}
