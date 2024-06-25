// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using CommunityToolkit.Mvvm.ComponentModel;

namespace De.HDBW.Apollo.Client.Models.Assessment
{
    public partial class ProviderDecoEntry : ObservableObject
    {
        private ProviderDecoEntry()
        {
        }

        public static ProviderDecoEntry Import()
        {
            return new ProviderDecoEntry();
        }
    }
}
