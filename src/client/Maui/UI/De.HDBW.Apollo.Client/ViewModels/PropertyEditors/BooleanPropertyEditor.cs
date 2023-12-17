// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using De.HDBW.Apollo.Client.Contracts;
using De.HDBW.Apollo.Client.Models.PropertyEditor;

namespace De.HDBW.Apollo.Client.ViewModels.PropertyEditors
{
    public class BooleanPropertyEditor : BasePropertyEditor<bool>
    {
        protected BooleanPropertyEditor(string label, BooleanValue editValue)
            : base(label, editValue, SetValueAction, GetValueAction)
        {
        }

        public static IPropertyEditor Import(string label, BooleanValue editValue)
        {
            return new BooleanPropertyEditor(label, editValue);
        }

        private static bool GetValueAction(BasePropertyEditor<bool> editor)
        {
            var boolValue = editor.Data as BooleanValue;
            return boolValue?.Value ?? false;
        }

        private static void SetValueAction(BasePropertyEditor<bool> editor)
        {
            var boolValue = editor.Data as BooleanValue;
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
