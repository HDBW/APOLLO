// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using De.HDBW.Apollo.Client.Contracts;
using De.HDBW.Apollo.Client.Models.PropertyEditor;

namespace De.HDBW.Apollo.Client.ViewModels.PropertyEditors
{
    public class DatePropertyEditor : BasePropertyEditor<DateTime?>
    {
        protected DatePropertyEditor(string label, DateTimeValue editValue)
            : base(label, editValue, SetValueAction, GetValueAction)
        {
        }

        public static IPropertyEditor Import(string label, DateTimeValue editValue)
        {
            return new DatePropertyEditor(label, editValue);
        }

        private static DateTime? GetValueAction(BasePropertyEditor<DateTime?> editor)
        {
            var boolValue = editor.Data as DateTimeValue;
            return boolValue?.Value;
        }

        private static void SetValueAction(BasePropertyEditor<DateTime?> editor)
        {
            var boolValue = editor.Data as DateTimeValue;
            if (boolValue == null)
            {
                return;
            }

            if (editor.HasChanges)
            {
                boolValue.Value = editor.Value;
            }
        }
    }
}
