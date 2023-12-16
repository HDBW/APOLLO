// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using De.HDBW.Apollo.Client.Contracts;

namespace De.HDBW.Apollo.Client.Models.Editors
{
    public class DateRangePropertyEditor : BasePropertyEditor<DateRange?>
    {
        protected DateRangePropertyEditor(DateRangeValue configuaration)
            : base(configuaration, SetValueAction, GetValueAction, GetDefaultValueAction)
        {
        }

        public static IPropertyEditor Import(DateRangeValue configuaration)
        {
            return new DateRangePropertyEditor(configuaration);
        }

        private static DateRange? GetValueAction(BasePropertyEditor<DateRange?> editor)
        {
            var stringValue = editor.Data as BaseValue<DateRange?>;
            if (stringValue == null)
            {
                return editor.DefaultValue;
            }

            return stringValue.Value;
        }

        private static DateRange? GetDefaultValueAction(BasePropertyEditor<DateRange?> editor)
        {
            var stringValue = editor.Data as BaseValue<DateRange?>;
            if (stringValue == null)
            {
                return null;
            }

            return stringValue.DefaultValue;
        }

        private static void SetValueAction(BasePropertyEditor<DateRange?> editor)
        {
            var dateRangeValue = editor.Data as BaseValue<DateRange?>;
            if (dateRangeValue == null)
            {
                return;
            }

            if (editor.HasChanges)
            {
                dateRangeValue.Value = editor.Value;
            }
        }
    }
}
