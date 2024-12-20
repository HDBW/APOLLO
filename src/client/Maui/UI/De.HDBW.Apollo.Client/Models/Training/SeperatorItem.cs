﻿// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using CommunityToolkit.Mvvm.ComponentModel;

namespace De.HDBW.Apollo.Client.Models.Training
{
    public partial class SeperatorItem : ObservableObject
    {
        public static ObservableObject Import()
        {
            return new SeperatorItem();
        }
    }
}
