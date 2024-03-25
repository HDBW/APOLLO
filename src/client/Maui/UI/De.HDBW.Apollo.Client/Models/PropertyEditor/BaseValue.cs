// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using CommunityToolkit.Mvvm.ComponentModel;

namespace De.HDBW.Apollo.Client.Models.PropertyEditor
{
    public abstract partial class BaseValue : ObservableObject
    {
        protected BaseValue(object? data)
        {
            Data = data;
        }

        public object? Data { get; set; }
    }
}
