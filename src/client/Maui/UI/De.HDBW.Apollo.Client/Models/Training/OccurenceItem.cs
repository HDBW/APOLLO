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
        [ObservableProperty]
        private string? _header;

        [NotifyPropertyChangedFor(nameof(CanExpand))]
        [ObservableProperty]
        private ObservableCollection<LineItem> _items = new ObservableCollection<LineItem>();

        [ObservableProperty]
        private bool _isExpanded;

        private OccurenceItem(Occurence occurence, Action<OccurenceItem>? changeStateHandler)
        {
            ChangeStateHandler = changeStateHandler;
            Header = $"{occurence.StartDate.ToUIDate().ToShortDateString()} - {occurence.EndDate.ToUIDate().ToShortDateString()}";
            if (occurence.Location != null)
            {
                Items = ContactItem.Import(null, occurence.Location, null, null, null, null).Items;
            }

            if (!string.IsNullOrWhiteSpace(occurence.Description))
            {
                Items.Add(LineWithoutIconItem.Import(null, occurence.Description));
            }
        }

        public bool CanExpand
        {
            get
            {
                return Items.Any();
            }
        }

        private Action<OccurenceItem>? ChangeStateHandler { get; }

        public static OccurenceItem Import(Occurence occurence, Action<OccurenceItem>? changeStateHandler)
        {
            return new OccurenceItem(occurence, changeStateHandler);
        }

        [RelayCommand]
        private void ToggleExpandState()
        {
            if (!CanExpand)
            {
                return;
            }

            IsExpanded = !IsExpanded;
            ChangeStateHandler?.Invoke(this);
        }
    }
}
