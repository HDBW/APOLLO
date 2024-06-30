// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using CommunityToolkit.Mvvm.ComponentModel;

namespace De.HDBW.Apollo.Client.Models.Assessment
{
    public partial class SessionStateEntry : ObservableObject
    {
        [ObservableProperty]
        private bool _canContinue;

        private SessionStateEntry(int? repeatable)
        {
            CanContinue = (repeatable ?? 0) == 0;
        }

        public static ObservableObject Import(int? repeatable)
        {
            return new SessionStateEntry(repeatable);
        }

        internal void RefreshCommands() => throw new NotImplementedException();
    }
}
