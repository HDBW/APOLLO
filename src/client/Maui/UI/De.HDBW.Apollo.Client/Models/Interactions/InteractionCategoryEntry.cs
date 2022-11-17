// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace De.HDBW.Apollo.Client.Models.Interactions
{
    public partial class InteractionCategoryEntry : ObservableObject
    {
        private readonly object? _data;

        private readonly ObservableCollection<InteractionEntry> _interactions = new ObservableCollection<InteractionEntry>();

        private readonly ObservableCollection<InteractionEntry> _filters = new ObservableCollection<InteractionEntry>();

        [ObservableProperty]
        private string? _headLine;

        [ObservableProperty]
        private string? _sublineLine;

        private Func<InteractionCategoryEntry, bool> _canNavigateHandle;

        private Func<InteractionCategoryEntry, Task> _navigateHandler;

        private InteractionCategoryEntry(string? headLine, string? sublineLine, List<InteractionEntry> interactions, object? data, Func<InteractionCategoryEntry, Task> navigateHandler, Func<InteractionCategoryEntry, bool> canNavigateHandle)
        {
            HeadLine = headLine;
            SublineLine = sublineLine;
            _data = data;
            _interactions = new ObservableCollection<InteractionEntry>(interactions ?? new List<InteractionEntry>());
            _canNavigateHandle = canNavigateHandle;
            _navigateHandler = navigateHandler;
        }

        public ObservableCollection<InteractionEntry> Interactions
        {
            get
            {
                return _interactions;
            }
        }

        public ObservableCollection<InteractionEntry> Filters
        {
            get
            {
                return _filters;
            }
        }

        public static InteractionCategoryEntry Import(string? headLine, string? sublineLine, List<InteractionEntry> interactions, object? data, Func<InteractionCategoryEntry, Task> navigateHandler, Func<InteractionCategoryEntry, bool> canNavigateHandle)
        {
            return new InteractionCategoryEntry(headLine, sublineLine, interactions, data, navigateHandler, canNavigateHandle);
        }

        [RelayCommand(AllowConcurrentExecutions = false, CanExecute = nameof(CanShowMore), FlowExceptionsToTaskScheduler = false, IncludeCancelCommand = false)]
        private Task ShowMore(CancellationToken token)
        {
            return _navigateHandler?.Invoke(this) ?? Task.CompletedTask;
        }

        private bool CanShowMore()
        {
            return _canNavigateHandle?.Invoke(this) ?? false;
        }
    }
}
