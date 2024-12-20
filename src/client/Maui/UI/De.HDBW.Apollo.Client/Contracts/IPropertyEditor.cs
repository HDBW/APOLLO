﻿// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;

namespace De.HDBW.Apollo.Client.Contracts
{
    public interface IPropertyEditor
    {
        string Label { get; }

        IRelayCommand ClearCommand { get; }

        void Save();
    }

    public interface IPropertyEditor<T> : IPropertyEditor
    {
        T Value { get; set; }
    }
}
