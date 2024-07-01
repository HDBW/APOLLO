// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Image = De.HDBW.Apollo.SharedContracts.Models.Image;

namespace De.HDBW.Apollo.Client.Models.Assessment
{
    public partial class AssociateImageEntry : ImageEntry
    {
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(DisplayAssociatedIndex))]
        [NotifyPropertyChangedFor(nameof(IsAssociated))]
        private int? _associatedIndex;

        private Func<AssociateImageEntry, int> _getIndexCallback;

        private Func<int?, int?> _incrementCallback;

        public AssociateImageEntry(Image data, string basePath, int density, int size, Func<AssociateImageEntry, int> getIndexCallback, Func<int?, int?> incrementCallback)
            : base(data, basePath, density, size)
        {
            ArgumentNullException.ThrowIfNull(getIndexCallback);
            ArgumentNullException.ThrowIfNull(incrementCallback);
            _getIndexCallback = getIndexCallback;
            _incrementCallback = incrementCallback;
        }

        public int Index
        {
            get
            {
                return _getIndexCallback.Invoke(this);
            }
        }

        public int DisplayIndex
        {
            get
            {
                return Index + 1;
            }
        }

        public bool IsAssociated
        {
            get
            {
                return AssociatedIndex.HasValue;
            }
        }

        public int? DisplayAssociatedIndex
        {
            get
            {
                return AssociatedIndex.HasValue ? AssociatedIndex.Value + 1 : null;
            }
        }

        public static AssociateImageEntry Import(
            Image data,
            string basePath,
            int density,
            int size,
            Func<AssociateImageEntry, int> getIndexCallback,
            Func<int?, int?> incrementCallback)
        {
            return new AssociateImageEntry(data, basePath, density, size, getIndexCallback, incrementCallback);
        }

        [RelayCommand(AllowConcurrentExecutions = false)]
        private Task Increment(CancellationToken cancellationToken)
        {
            try
            {
                AssociatedIndex = _incrementCallback.Invoke(AssociatedIndex);
            }
            catch (OperationCanceledException ex)
            {
            }
            catch (ObjectDisposedException ex)
            {
            }
            catch (Exception ex)
            {
            }

            return Task.CompletedTask;
        }
    }
}
