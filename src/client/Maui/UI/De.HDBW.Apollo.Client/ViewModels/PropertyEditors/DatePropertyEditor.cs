// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using De.HDBW.Apollo.Client.Contracts;
using De.HDBW.Apollo.Client.Models.PropertyEditor;

namespace De.HDBW.Apollo.Client.ViewModels.PropertyEditors
{
    public class DatePropertyEditor : BasePropertyEditor<DateTime?>
    {
        protected DatePropertyEditor(string label, DateTimeValue editValue, Action<BasePropertyEditor> clearValueAction)
            : base(label, editValue, SetValueAction, GetValueAction, clearValueAction)
        {
        }

        public static IPropertyEditor Import(string label, DateTimeValue editValue, Action<BasePropertyEditor> clearValueAction)
        {
            return new DatePropertyEditor(label, editValue, clearValueAction);
        }

        public override void Update(BaseValue? data, bool hasChanges)
        {
            Data = data;
            SetValueAction(this);
            HasChanges = hasChanges;
        }

        public override void Save()
        {
            SetValueAction(this);
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
