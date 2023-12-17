// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using De.HDBW.Apollo.Client.Contracts;
using De.HDBW.Apollo.Client.Models.PropertyEditor;

namespace De.HDBW.Apollo.Client.ViewModels.PropertyEditors
{
    public class StringPropertyEditor : BasePropertyEditor<string>
    {
        protected StringPropertyEditor(string lable, StringValue editData)
            : base(lable, editData, SetValueAction, GetValueAction)
        {
        }

        public static IPropertyEditor Import(string label, StringValue editData)
        {
            return new StringPropertyEditor(label, editData);
        }

        private static string GetValueAction(BasePropertyEditor<string> editor)
        {
            var stringValue = editor.Data as StringValue;
            return stringValue?.Value ?? string.Empty;
        }

        private static void SetValueAction(BasePropertyEditor<string> editor)
        {
            var stringValue = editor.Data as StringValue;
            if (stringValue == null)
            {
                return;
            }

            if (editor.HasChanges)
            {
                stringValue.Value = editor.Value;
            }
        }
    }
}
