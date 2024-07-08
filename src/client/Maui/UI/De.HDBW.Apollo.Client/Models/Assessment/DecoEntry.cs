// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using CommunityToolkit.Mvvm.ComponentModel;

namespace De.HDBW.Apollo.Client.Models.Assessment
{
    public partial class DecoEntry : ObservableObject
    {
        private DecoEntry()
        {
        }

        public static DecoEntry Import()
        {
            return new DecoEntry();
        }
    }
}
