// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using De.HDBW.Apollo.Client.Contracts;

namespace De.HDBW.Apollo.Client.Models.Editors
{
    public class StringPropertyEditor : BasePropertyEditor<string>
    {
        protected StringPropertyEditor(StringValue configuaration)
            : base(configuaration, SetValueAction, GetValueAction, GetDefaultValueAction)
        {
        }

        public static IPropertyEditor Import(StringValue configuaration)
        {
            return new StringPropertyEditor(configuaration);
        }

        private static string GetValueAction(BasePropertyEditor<string> editor)
        {
            var stringValue = editor.Data as BaseValue<string>;
            if (stringValue == null)
            {
                return editor.DefaultValue;
            }

            return stringValue.Value;
        }

        private static string GetDefaultValueAction(BasePropertyEditor<string> editor)
        {
            var stringValue = editor.Data as BaseValue<string>;
            if (stringValue == null)
            {
                return string.Empty;
            }

            return stringValue.DefaultValue;
        }

        private static void SetValueAction(BasePropertyEditor<string> editor)
        {
            var stringValue = editor.Data as BaseValue<string>;
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
