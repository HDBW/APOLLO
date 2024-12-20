﻿// (c) Licensed to the HDBW under one or more agreements.
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

        protected InteractionCategoryEntry(string? headLine, string? sublineLine, List<InteractionEntry> interactions, List<InteractionEntry> filters, object? data, Func<InteractionCategoryEntry, Task> navigateHandler, Func<InteractionCategoryEntry, bool> canNavigateHandle)
        {
            HeadLine = headLine;
            SublineLine = sublineLine;
            _data = data;
            _interactions = new ObservableCollection<InteractionEntry>(interactions ?? new List<InteractionEntry>());
            _canNavigateHandle = canNavigateHandle;
            _navigateHandler = navigateHandler;
            _filters = new ObservableCollection<InteractionEntry>(filters);
        }

        public ObservableCollection<InteractionEntry> Interactions
        {
            get
            {
                return _interactions;
            }
        }

        public bool HasInteractions
        {
            get
            {
                return Interactions?.Any() ?? false;
            }
        }

        public bool HasNoInteractions
        {
            get
            {
                return !HasInteractions;
            }
        }

        public ObservableCollection<InteractionEntry> Filters
        {
            get
            {
                return _filters;
            }
        }

        public bool HasFilters
        {
            get
            {
                return Filters?.Any() ?? false;
            }
        }

        public static InteractionCategoryEntry Import(string? headLine, string? sublineLine, List<InteractionEntry> interactions, List<InteractionEntry> filters, object? data, Func<InteractionCategoryEntry, Task> navigateHandler, Func<InteractionCategoryEntry, bool> canNavigateHandle)
        {
            return new InteractionCategoryEntry(headLine, sublineLine, interactions, filters, data, navigateHandler, canNavigateHandle);
        }

        public void RefreshCommands()
        {
            ShowMoreCommand?.NotifyCanExecuteChanged();
            foreach (var filter in Filters)
            {
                filter.NavigateCommand?.NotifyCanExecuteChanged();
            }

            foreach (var interaction in Interactions)
            {
                interaction.NavigateCommand?.NotifyCanExecuteChanged();
            }
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
