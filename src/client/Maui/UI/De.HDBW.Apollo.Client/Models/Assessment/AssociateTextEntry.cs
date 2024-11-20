// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using CommunityToolkit.Mvvm.ComponentModel;

namespace De.HDBW.Apollo.Client.Models.Assessment
{
    public partial class AssociateTextEntry : ObservableObject
    {
        [ObservableProperty]
        private string? _label;

        private Func<AssociateTextEntry, int> _getIndexCallback;

        private AssociateTextEntry(string? label, Func<AssociateTextEntry, int> getIndexCallback)
        {
            ArgumentNullException.ThrowIfNull(getIndexCallback);
            Label = label;
            _getIndexCallback = getIndexCallback;
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

        public static AssociateTextEntry Import(string label, Func<AssociateTextEntry, int> getIndexCallback)
        {
            return new AssociateTextEntry(label, getIndexCallback);
        }
    }
}
