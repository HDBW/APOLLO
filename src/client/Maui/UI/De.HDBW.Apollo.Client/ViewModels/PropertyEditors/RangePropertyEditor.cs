// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using CommunityToolkit.Mvvm.ComponentModel;
using De.HDBW.Apollo.Client.Contracts;
using De.HDBW.Apollo.Client.Models.PropertyEditor;

namespace De.HDBW.Apollo.Client.ViewModels.PropertyEditors
{
    public partial class RangePropertyEditor : BasePropertyEditor<DecimalRangeValue>
    {
        private readonly decimal _rangeValue;

        private double _endValue;
        private double _startValue;

        [ObservableProperty]
        private double _minValue;

        [ObservableProperty]
        private double _maxValue;

        public RangePropertyEditor(string label, DecimalRangeValue editValue, decimal rangeValue, Action<BasePropertyEditor> clearValueAction)
            : base(label, editValue, SetValueAction, GetValueAction, clearValueAction)
        {
            ArgumentNullException.ThrowIfNull(editValue);
            ArgumentNullException.ThrowIfNull(rangeValue);
            _rangeValue = rangeValue;
            MinValue = 0;
            MaxValue = 1;
            StartValue = Convert.ToDouble(editValue.Value.Start / RangeValue);
            EndValue = Convert.ToDouble(editValue.Value.End / RangeValue);
            HasChanges = false;
        }

        public decimal RangeValue
        {
            get
            {
                return _rangeValue;
            }
        }

        public string StartValueString
        {
            get
            {
                return $"{StartValue * Convert.ToDouble(_rangeValue):N2} €";
            }
        }

        public string EndValueString
        {
            get
            {
                return $"{EndValue * Convert.ToDouble(_rangeValue):N2} €";
            }
        }

        public double StartValue
        {
            get
            {
                return _startValue;
            }

            set
            {
                if (SetProperty(ref _startValue, value))
                {
                    EndValue = Math.Max(EndValue, StartValue);
                    OnPropertyChanged(nameof(StartValueString));
                    HasChanges = true;
                }
            }
        }

        public double EndValue
        {
            get
            {
                return _endValue;
            }

            set
            {
                if (SetProperty(ref _endValue, value))
                {
                    StartValue = Math.Min(StartValue, EndValue);
                    OnPropertyChanged(nameof(EndValueString));
                    HasChanges = true;
                }
            }
        }

        public static IPropertyEditor Import(string label, DecimalRangeValue editValue, decimal rangeValue, Action<BasePropertyEditor> clearValueAction)
        {
            return new RangePropertyEditor(label, editValue, rangeValue, clearValueAction);
        }

        public override void Update(BaseValue? data, bool hasChanges)
        {
            Data = data;
            Value = GetValueAction(this);
            StartValue = Convert.ToDouble(Value.Value.Start / RangeValue);
            EndValue = Convert.ToDouble(Value.Value.End / RangeValue);
            HasChanges = hasChanges;
        }

        public override void Save()
        {
            SetValueAction(this);
        }

        private static DecimalRangeValue GetValueAction(BasePropertyEditor<DecimalRangeValue> editor)
        {
            var editValue = editor.Data as DecimalRangeValue;
            return new DecimalRangeValue(editValue!.Value!);
        }

        private static void SetValueAction(BasePropertyEditor<DecimalRangeValue> editor)
        {
            var rangeEditor = editor as RangePropertyEditor;
            var editValue = editor.Data as DecimalRangeValue;
            if (editValue == null || rangeEditor == null)
            {
                return;
            }

            if (editor.HasChanges)
            {
                decimal start = Convert.ToDecimal(rangeEditor.StartValue) * rangeEditor.RangeValue;
                decimal end = Convert.ToDecimal(rangeEditor.EndValue) * rangeEditor.RangeValue;
                editValue.Value = (start, end);
                editValue.WasChanged = true;
            }
        }
    }
}
