// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using CommunityToolkit.Mvvm.ComponentModel;

namespace De.HDBW.Apollo.Client.Models.Generic
{
    public partial class SeperatorValue : ObservableObject
    {

        protected SeperatorValue()
        {
        }

        public static SeperatorValue Import()
        {
            return new SeperatorValue();
        }
    }
}
