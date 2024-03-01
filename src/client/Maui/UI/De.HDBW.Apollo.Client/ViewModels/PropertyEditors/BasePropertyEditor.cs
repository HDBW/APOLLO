// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using De.HDBW.Apollo.Client.Contracts;
using De.HDBW.Apollo.Client.Models.PropertyEditor;

namespace De.HDBW.Apollo.Client.ViewModels.PropertyEditors
{
    public abstract partial class BasePropertyEditor : ObservableObject, IPropertyEditor
    {
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(ClearCommand))]
        private bool _hasChanges;

        [ObservableProperty]
        private bool _isActive;

        protected BasePropertyEditor(string label, Action<BasePropertyEditor> clearAction)
            : this(label, null, clearAction)
        {
        }

        protected BasePropertyEditor(string label, BaseValue? data, Action<BasePropertyEditor> clearAction)
        {
            Label = label;
            Data = data;
            ClearAction = clearAction;
        }

        public string Label
        {
            get; private set;
        }

        public BaseValue? Data
        {
            get; set;
        }

        public Action<BasePropertyEditor> ClearAction { get; }

        public abstract void Update(BaseValue? data, bool hasChanges);

        public abstract void Save();

        [RelayCommand(CanExecute = nameof(CanClear))]
        private void Clear()
        {
            ClearAction.Invoke(this);
        }

        private bool CanClear()
        {
            return HasChanges;
        }
    }
}
