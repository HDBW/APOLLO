using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using De.HDBW.Apollo.Client.Contracts;

namespace De.HDBW.Apollo.Client.Models.Editors
{
    public class PickerPropertyEditor : BasePropertyEditor, IPropertyEditor<OptionValue?>
    {
        private readonly OptionValueList _configuaration;
        private OptionValue? _value;
        private ObservableCollection<OptionValue?> _values;

        private PickerPropertyEditor(OptionValueList configuaration)
           : base(configuaration)
        {
            _configuaration = configuaration;
            _values = new ObservableCollection<OptionValue?>(configuaration.Options);
            _value = GetValueAction(this);
        }

        public ObservableCollection<OptionValue?> Values
        {
            get
            {
                return _values;
            }

            set
            {
                SetProperty(ref _values, value);
            }
        }

        public OptionValue? Value
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

        public static IPropertyEditor Import(
            OptionValueList configuaration)
        {
            return new PickerPropertyEditor(configuaration);
        }

        private static OptionValue? GetValueAction(BasePropertyEditor editor)
        {
            var optionValue = editor.Data as OptionValueList;
            if (optionValue == null)
            {
                return null;
            }

            if (optionValue.Value == null)
            {
                return GetDefaultValueAction(editor);
            }

            return optionValue.Value;
        }

        private static OptionValue? GetDefaultValueAction(BasePropertyEditor editor)
        {
            var optionValue = editor.Data as OptionValueList;
            if (optionValue == null)
            {
                return null;
            }

            return optionValue.DefaultValue;
        }
    }
}
