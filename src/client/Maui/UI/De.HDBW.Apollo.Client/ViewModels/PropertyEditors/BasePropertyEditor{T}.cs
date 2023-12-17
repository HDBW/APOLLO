// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using De.HDBW.Apollo.Client.Contracts;
using De.HDBW.Apollo.Client.Models.PropertyEditor;

namespace De.HDBW.Apollo.Client.ViewModels.PropertyEditors
{
    public abstract partial class BasePropertyEditor<TU> : BasePropertyEditor, IPropertyEditor<TU>
    {
        private readonly Func<BasePropertyEditor<TU>, TU> _getValueAction;
        private readonly Action<BasePropertyEditor<TU>> _setValueAction;
        private TU _value;

        protected BasePropertyEditor(
            string label,
            BaseValue data,
            Action<BasePropertyEditor<TU>> setValueAction,
            Func<BasePropertyEditor<TU>, TU> getValueAction)
            : base(label, data)
        {
            _getValueAction = getValueAction;
            _setValueAction = setValueAction;
            _value = getValueAction(this);
        }

        public TU Value
        {
            get
            {
                return _value;
            }

            set
            {
                if (SetProperty(ref _value, value))
                {
                    HasChanges = true;
                }
            }
        }
    }
}
