// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using De.HDBW.Apollo.Client.Models.Generic;

namespace De.HDBW.Apollo.Client.Models.Profile
{
    public abstract partial class AbstractProfileEntry<TU> : ObservableObject
    {
        private readonly TU _data;

        [ObservableProperty]
        private ObservableCollection<StringValue> _allLines = new ObservableCollection<StringValue>();

        public AbstractProfileEntry(TU data, Func<AbstractProfileEntry<TU>, Task> editHandle, Func<AbstractProfileEntry<TU>, bool> canEditHandle, Func<AbstractProfileEntry<TU>, Task> deleteHandle, Func<AbstractProfileEntry<TU>, bool> canDeleteHandle)
        {
            ArgumentNullException.ThrowIfNull(data);
            ArgumentNullException.ThrowIfNull(editHandle);
            ArgumentNullException.ThrowIfNull(canEditHandle);
            ArgumentNullException.ThrowIfNull(deleteHandle);
            ArgumentNullException.ThrowIfNull(canDeleteHandle);
            _data = data;
            AllLines = GetAllLines(_data);
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

        protected abstract ObservableCollection<StringValue> GetAllLines(TU data);

        private bool CanDelete()
        {
            return CanDeleteHandle?.Invoke(this) ?? false;
        }

        private bool CanEdit()
        {
            return CanEditHandle?.Invoke(this) ?? false;
        }

        [RelayCommand(AllowConcurrentExecutions = false, CanExecute = nameof(CanDelete))]
        private Task Delete()
        {
            return DeleteHandle?.Invoke(this) ?? Task.CompletedTask;
        }

        [RelayCommand(AllowConcurrentExecutions = false, CanExecute = nameof(CanEdit))]
        private Task Edit()
        {
            return EditHandle?.Invoke(this) ?? Task.CompletedTask;
        }
    }
}
