using De.HDBW.Apollo.Client.Contracts;

namespace De.HDBW.Apollo.Client.Models.Editors
{
    public abstract partial class BasePropertyEditor<T> : BasePropertyEditor, IPropertyEditor<T>
    {
        private readonly Func<BasePropertyEditor<T>, T> _getValueAction;
        private readonly Action<BasePropertyEditor<T>> _setValueAction;
        private readonly Func<BasePropertyEditor<T>, T> _getDefaultValueAction;
        private T _value;

        protected BasePropertyEditor(
            BaseValue<T> data,
            Action<BasePropertyEditor<T>> setValueAction,
            Func<BasePropertyEditor<T>, T> getValueAction,
            Func<BasePropertyEditor<T>, T> getDefaultValueAction)
            : base(data)
        {
            Data = data;
            _getValueAction = getValueAction;
            _setValueAction = setValueAction;
            _getDefaultValueAction = getDefaultValueAction;
            _value = getValueAction(this);
        }

        public T Value
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

        public T DefaultValue
        {
            get
            {
                return _getDefaultValueAction(this);
            }
        }
    }
}
