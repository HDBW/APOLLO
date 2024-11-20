// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using De.HDBW.Apollo.SharedContracts.Questions;

namespace De.HDBW.Apollo.Client.Models.Assessment
{
    public partial class SelectableEaconditionEntry : EaconditionEntry
    {
        private readonly Action<SelectableEaconditionEntry> _selectionChangedHandler;

        [ObservableProperty]
        private bool _isSelected;

        private SelectableEaconditionEntry(Eacondition data, Action<SelectableEaconditionEntry> selectionChangedHandler, string basePath, int density, Dictionary<string, int> imageSizeConfig)
            : base(data, basePath, density, imageSizeConfig)
        {
            _selectionChangedHandler = selectionChangedHandler;
        }

        public static SelectableEaconditionEntry Import(Eacondition data, Action<SelectableEaconditionEntry> selectionChangedHandler, string basePath, int density, Dictionary<string, int> imageSizeConfig)
        {
            return new SelectableEaconditionEntry(data, selectionChangedHandler, basePath, density, imageSizeConfig);
        }

        [RelayCommand(AllowConcurrentExecutions = false)]
        private Task ToggleSelection(CancellationToken cancellationToken)
        {
            IsSelected = !IsSelected;
            _selectionChangedHandler?.Invoke(this);
            return Task.CompletedTask;
        }
    }
}
