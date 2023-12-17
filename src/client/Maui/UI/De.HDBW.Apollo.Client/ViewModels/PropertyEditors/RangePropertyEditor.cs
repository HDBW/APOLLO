// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using CommunityToolkit.Mvvm.ComponentModel;
using De.HDBW.Apollo.Client.Contracts;
using De.HDBW.Apollo.Client.Models.PropertyEditor;

namespace De.HDBW.Apollo.Client.ViewModels.PropertyEditors
{
    public partial class RangePropertyEditor : BasePropertyEditor<double>
    {
        [ObservableProperty]
        private double _minValue;

        [ObservableProperty]
        private double _maxValue;

        public RangePropertyEditor(string label, BaseValue<double> editValue, double minValue, double maxValue)
            : base(label, editValue, SetValueAction, GetValueAction)
        {
            ArgumentNullException.ThrowIfNull(editValue);
            ArgumentNullException.ThrowIfNull(minValue);
            ArgumentNullException.ThrowIfNull(maxValue);
            MinValue = minValue;
            MaxValue = maxValue;
        }

        public static IPropertyEditor Import(string label, BaseValue<double> editValue, double minValue, double maxValue)
        {
            return new RangePropertyEditor(label, editValue, minValue, maxValue);
        }

        private static double GetValueAction(BasePropertyEditor<double> editor)
        {
            var editValue = editor.Data as BaseValue<double>;
            return editValue!.Value!;
        }

        private static void SetValueAction(BasePropertyEditor<double> editor)
        {
            var editValue = editor.Data as BaseValue<double>;
            if (editValue == null)
            {
                return;
            }

            if (editor.HasChanges)
            {
                editValue.Value = editor.Value;
            }
        }
    }
}
