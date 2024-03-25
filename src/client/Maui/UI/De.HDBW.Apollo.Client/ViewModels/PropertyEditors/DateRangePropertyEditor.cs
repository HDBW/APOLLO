// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using De.HDBW.Apollo.Client.Contracts;
using De.HDBW.Apollo.Client.Models.PropertyEditor;

namespace De.HDBW.Apollo.Client.ViewModels.PropertyEditors
{
    public class DateRangePropertyEditor : BasePropertyEditor<DateRangeValue>
    {
        private DateTime? _end;

        private DateTime? _start;

        protected DateRangePropertyEditor(string label, DateRangeValue editValue, Action<BasePropertyEditor> clearValueAction)
            : base(label, editValue, SetValueAction, GetValueAction, clearValueAction)
        {
            ArgumentNullException.ThrowIfNull(editValue);
            Start = editValue.Value.Start;
            End = editValue.Value.End;
            HasChanges = false;
        }

        public DateTime? Start
        {
            get
            {
                return _start;
            }

            set
            {
                if (SetProperty(ref _start, value))
                {
                    HasChanges = true;
                }
            }
        }

        public DateTime? End
        {
            get
            {
                return _end;
            }

            set
            {
                if (SetProperty(ref _end, value))
                {
                    HasChanges = true;
                }
            }
        }

        public static IPropertyEditor Import(string label, DateRangeValue editValue, Action<BasePropertyEditor> clearValueAction)
        {
            return new DateRangePropertyEditor(label, editValue, clearValueAction);
        }

        public override void Update(BaseValue? data, bool hasChanges)
        {
            Data = data;
            Value = GetValueAction(this);
            Start = Value.Value.Start;
            End = Value.Value.End;
            HasChanges = hasChanges;
        }

        public override void Save()
        {
            SetValueAction(this);
        }

        private static DateRangeValue GetValueAction(BasePropertyEditor<DateRangeValue> editor)
        {
            var editValue = editor.Data as DateRangeValue;
            return new DateRangeValue(editValue!.Value!);
        }

        private static void SetValueAction(BasePropertyEditor<DateRangeValue> editor)
        {
            var rangeEditor = editor as DateRangePropertyEditor;
            var editValue = editor.Data as DateRangeValue;
            if (editValue == null || rangeEditor == null)
            {
                return;
            }

            if (editor.HasChanges)
            {
                editValue.Value = (rangeEditor.Start, rangeEditor.End);
            }
        }
    }
}
