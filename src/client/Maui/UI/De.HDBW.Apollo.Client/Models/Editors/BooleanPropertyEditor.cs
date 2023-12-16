// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using De.HDBW.Apollo.Client.Contracts;
using Google.Protobuf.WellKnownTypes;

namespace De.HDBW.Apollo.Client.Models.Editors
{
    public class BooleanPropertyEditor : BasePropertyEditor<bool>
    {
        protected BooleanPropertyEditor(BoolenValue configuaration)
            : base(configuaration, SetValueAction, GetValueAction, GetDefaultValueAction)
        {
        }

        public static IPropertyEditor Import(BoolenValue configuaration)
        {
            return new BooleanPropertyEditor(configuaration);
        }

        private static bool GetValueAction(BasePropertyEditor<bool> editor)
        {
            var boolValue = editor.Data as BaseValue<bool>;
            if (boolValue == null)
            {
                return editor.DefaultValue;
            }

            return boolValue.Value;
        }

        private static bool GetDefaultValueAction(BasePropertyEditor<bool> editor)
        {
            var boolValue = editor.Data as BaseValue<bool>;
            if (boolValue == null)
            {
                return false;
            }

            return boolValue.DefaultValue;
        }

        private static void SetValueAction(BasePropertyEditor<bool> editor)
        {
            var boolValue = editor.Data as BaseValue<bool>;
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
