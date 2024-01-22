﻿// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace De.HDBW.Apollo.Client.Models.Profile
{
    public abstract partial class AbstractProfileEntry<TU> : ObservableObject
    {
        private readonly TU _data;

        [ObservableProperty]
        private string _firstLine;

        [ObservableProperty]
        private ObservableCollection<string> _additionalLines = new ObservableCollection<string>();

        public AbstractProfileEntry(TU data, Func<AbstractProfileEntry<TU>, Task> editHandle, Func<AbstractProfileEntry<TU>, bool> canEditHandle, Func<AbstractProfileEntry<TU>, Task> deleteHandle, Func<AbstractProfileEntry<TU>, bool> canDeleteHandle)
        {
            ArgumentNullException.ThrowIfNull(data);
            ArgumentNullException.ThrowIfNull(editHandle);
            ArgumentNullException.ThrowIfNull(canEditHandle);
            ArgumentNullException.ThrowIfNull(deleteHandle);
            ArgumentNullException.ThrowIfNull(canDeleteHandle);
            _data = data;
            FirstLine = GetFristLine(_data);
            AdditionalLines = GetAdditionalLines(_data);
            CanEditHandle = canEditHandle;
            EditHandle = editHandle;
            CanDeleteHandle = canDeleteHandle;
            DeleteHandle = deleteHandle;
        }

        protected Func<AbstractProfileEntry<TU>, bool> CanEditHandle { get; }

        protected Func<AbstractProfileEntry<TU>, Task> EditHandle { get; }

        protected Func<AbstractProfileEntry<TU>, bool> CanDeleteHandle { get; }

        protected Func<AbstractProfileEntry<TU>, Task> DeleteHandle { get; }

        public TU Export()
        {
            return _data;
        }

        protected abstract ObservableCollection<string> GetAdditionalLines(TU data);

        protected abstract string GetFristLine(TU data);

        protected virtual bool CanDelete()
        {
            return CanDeleteHandle?.Invoke(this) ?? false;
        }

        [RelayCommand(AllowConcurrentExecutions = false, CanExecute = nameof(CanDelete))]
        private Task Delete()
        {
            return DeleteHandle?.Invoke(this) ?? Task.CompletedTask;
        }

        protected virtual bool CanEdit()
        {
            return CanEditHandle?.Invoke(this) ?? false;
        }

        [RelayCommand(AllowConcurrentExecutions = false, CanExecute = nameof(CanEdit))]
        private Task Edit()
        {
            return EditHandle?.Invoke(this) ?? Task.CompletedTask;
        }
    }
}
