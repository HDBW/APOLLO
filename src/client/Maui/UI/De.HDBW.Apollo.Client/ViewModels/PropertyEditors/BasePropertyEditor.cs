// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using CommunityToolkit.Mvvm.ComponentModel;
using De.HDBW.Apollo.Client.Contracts;
using De.HDBW.Apollo.Client.Models.PropertyEditor;

namespace De.HDBW.Apollo.Client.ViewModels.PropertyEditors
{
    public abstract partial class BasePropertyEditor : ObservableObject, IPropertyEditor
    {
        [ObservableProperty]
        private bool _hasChanges;

        protected BasePropertyEditor(string label)
            : this(label, null)
        {
        }

        protected BasePropertyEditor(string label, BaseValue? data)
        {
            Label = label;
            Data = data;
        }

        public string Label
        {
            get; private set;
        }

        public BaseValue? Data
        {
            get; set;
        }
    }
}
