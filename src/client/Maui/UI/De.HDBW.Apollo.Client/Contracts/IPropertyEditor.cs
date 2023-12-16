// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using De.HDBW.Apollo.Client.Models.Editors;

namespace De.HDBW.Apollo.Client.Contracts
{
    public interface IPropertyEditor
    {
        BaseValue Data { get; }

        string Label { get; }
    }

    public interface IPropertyEditor<T> : IPropertyEditor
    {
        T Value { get; set; }
    }
}
