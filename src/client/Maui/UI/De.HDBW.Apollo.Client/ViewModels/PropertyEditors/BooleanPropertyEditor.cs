// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using CommunityToolkit.Mvvm.Input;
using De.HDBW.Apollo.Client.Contracts;
using De.HDBW.Apollo.Client.Models.PropertyEditor;

namespace De.HDBW.Apollo.Client.ViewModels.PropertyEditors
{
    public partial class BooleanPropertyEditor : BasePropertyEditor<bool>
    {
        protected BooleanPropertyEditor(string label, BooleanValue editValue, Action<BasePropertyEditor> clearValueAction)
            : base(label, editValue, SetValueAction, GetValueAction, clearValueAction)
        {
        }

        public static IPropertyEditor Import(string label, BooleanValue editValue, Action<BasePropertyEditor> clearValueAction)
        {
            return new BooleanPropertyEditor(label, editValue, clearValueAction);
        }

        public override void Update(BaseValue? data, bool hasChanges)
        {
            Data = data;
            Value = GetValueAction(this);
            HasChanges = hasChanges;
        }

        public override void Save()
        {
            SetValueAction(this);
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

        [RelayCommand]
        private void ToggleState()
        {
            Value = !Value;
        }
    }
}
