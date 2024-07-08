// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace De.HDBW.Apollo.Client.Models.Assessment
{
    public partial class SelectableTextEntry : ObservableObject
    {
        private readonly Action<SelectableTextEntry>? _changeSelection;

        private readonly Func<SelectableTextEntry, bool>? _canChangeSelection;

        [ObservableProperty]
        private bool _isSelected;

        [ObservableProperty]
        private string _text;

        private SelectableTextEntry(
            string text,
            Action<SelectableTextEntry>? changeSelection,
            Func<SelectableTextEntry, bool>? canChangeSelection)
        {
            Text = text;
            _changeSelection = changeSelection;
            _canChangeSelection = canChangeSelection;
        }

        public static SelectableTextEntry Import(
            string text,
            Action<SelectableTextEntry>? changeSelection = null,
            Func<SelectableTextEntry, bool>? canChangeSelection = null)
        {
            return new SelectableTextEntry(text, changeSelection, canChangeSelection);
        }

        [RelayCommand(AllowConcurrentExecutions = false, CanExecute = nameof(CanToggleSelection))]
        private Task ToggleSelection(CancellationToken cancellationToken)
        {
            if (_changeSelection == null)
            {
                IsSelected = !IsSelected;
            }
            else
            {
                _changeSelection.Invoke(this);
            }

            return Task.CompletedTask;
        }

        private bool CanToggleSelection()
        {
            return _canChangeSelection?.Invoke(this) ?? true;
        }
    }
}
