// (c) Licensed to the HDBW under one or more agreements.
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

        public AbstractProfileEntry(TU data, Func<AbstractProfileEntry<TU>, Task> deleteHandle, Func<AbstractProfileEntry<TU>, bool> canDeleteHandle)
        {
            ArgumentNullException.ThrowIfNull(data);
            ArgumentNullException.ThrowIfNull(deleteHandle);
            ArgumentNullException.ThrowIfNull(canDeleteHandle);
            _data = data;
            FirstLine = GetFristLine(_data);
            AdditionalLines = GetAdditionalLines(_data);
            CanDeleteHandle = canDeleteHandle;
            DeleteHandle = deleteHandle;
        }

        public TU Export()
        {
            return _data;
        }

        protected Func<AbstractProfileEntry<TU>, bool> CanDeleteHandle { get; }

        protected Func<AbstractProfileEntry<TU>, Task> DeleteHandle { get; }

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
    }
}
