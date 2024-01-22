// MIT License
// 
// Copyright (c) 2023-2024 Federico Nembrini
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

namespace FedericoNembrini.Maui.CustomDatePicker
{
    public class NullableDatePicker : DatePicker, INullableDatePicker
    {
        bool _isCoercingDateMinOrMax;

        public bool IsClearButtonVisible { get; set; } = true;

        //public bool PersistTime { get; set; }

        public new static readonly BindableProperty DateProperty = BindableProperty.Create(
            propertyName: nameof(Date),
            returnType: typeof(DateTime),
            declaringType: typeof(NullableDatePicker),
            defaultValue: default(DateTime),
            defaultBindingMode: BindingMode.TwoWay,
            coerceValue: CoerceDate,
            propertyChanged: DatePropertyChanged,
            defaultValueCreator: (bindable) => DateTime.Today
        );

        static object CoerceDate(BindableObject bindable, object value)
        {
            var picker = (NullableDatePicker)bindable;
            DateTime dateValue = ((DateTime)value).Date;
            if (dateValue > picker.MaximumDate)
                dateValue = picker.MaximumDate;
            if (dateValue < picker.MinimumDate)
                dateValue = picker.MinimumDate;
            return dateValue;
        }

        static void DatePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var datePicker = (NullableDatePicker)bindable;

            // Keep NullableDate null when Date is changed as a result of coercing MininimumDate or MaximumumDate
            if (!datePicker._isCoercingDateMinOrMax || datePicker.NullableDate != null)
                datePicker.SetValueFromRenderer(NullableDateProperty, newValue);

            datePicker.DateSelected?.Invoke(datePicker, new DateChangedEventArgs((DateTime)oldValue, (DateTime)newValue));
        }

        public new DateTime Date
        {
            get => (DateTime)GetValue(DateProperty);
            set => SetValue(DateProperty, value);
        }

        public new static readonly BindableProperty MinimumDateProperty = BindableProperty.Create(
            propertyName: nameof(MinimumDate),
            returnType: typeof(DateTime),
            declaringType: typeof(NullableDatePicker),
            defaultValue: new DateTime(1900, 1, 1),
            validateValue: ValidateMinimumDate,
            coerceValue: CoerceMinimumDate
        );

        static bool ValidateMinimumDate(BindableObject bindable, object value)
        {
            return ((DateTime)value).Date <= ((NullableDatePicker)bindable).MaximumDate.Date;
        }

        static object CoerceMinimumDate(BindableObject bindable, object value)
        {
            DateTime dateValue = ((DateTime)value).Date;
            var picker = (NullableDatePicker)bindable;
            if (picker.Date < dateValue)
            {
                picker._isCoercingDateMinOrMax = true;
                picker.Date = dateValue;
                picker._isCoercingDateMinOrMax = false;
            }

            return dateValue;
        }

        public new DateTime MinimumDate
        {
            get { return (DateTime)GetValue(MinimumDateProperty); }
            set { SetValue(MinimumDateProperty, value); }
        }

        public new static readonly BindableProperty MaximumDateProperty = BindableProperty.Create(
            propertyName: nameof(MaximumDate),
            returnType: typeof(DateTime),
            declaringType: typeof(NullableDatePicker),
            defaultValue: new DateTime(2100, 12, 31),
            validateValue: ValidateMaximumDate,
            coerceValue: CoerceMaximumDate
        );

        static bool ValidateMaximumDate(BindableObject bindable, object value)
        {
            return ((DateTime)value).Date >= ((NullableDatePicker)bindable).MinimumDate.Date;
        }

        static object CoerceMaximumDate(BindableObject bindable, object value)
        {
            DateTime dateValue = ((DateTime)value).Date;
            var picker = (NullableDatePicker)bindable;
            if (picker.Date > dateValue)
            {
                picker._isCoercingDateMinOrMax = true;
                picker.Date = dateValue;
                picker._isCoercingDateMinOrMax = false;
            }

            return dateValue;
        }

        public new DateTime MaximumDate
        {
            get { return (DateTime)GetValue(MaximumDateProperty); }
            set { SetValue(MaximumDateProperty, value); }
        }

        public static readonly BindableProperty NullableDateProperty = BindableProperty.Create(
            propertyName: nameof(NullableDate),
            returnType: typeof(DateTime?),
            declaringType: typeof(NullableDatePicker),
            defaultValue: default(DateTime?),
            defaultBindingMode: BindingMode.TwoWay,
            coerceValue: CoerceNullableDate,
            propertyChanged: NullableDatePropertyChanged,
            defaultValueCreator: (bindable) => DateTime.Today
        );

        static object? CoerceNullableDate(BindableObject bindable, object value)
        {
            DateTime? newNullableDate = ((DateTime?)value)?.Date;
            if (newNullableDate == null)
                return null;

            var picker = (NullableDatePicker)bindable;
            if (newNullableDate > picker.MaximumDate)
                newNullableDate = picker.MaximumDate;

            if (newNullableDate < picker.MinimumDate)
                newNullableDate = picker.MinimumDate;

            return newNullableDate;
        }

        static void NullableDatePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var datePicker = (NullableDatePicker)bindable;

            if (newValue is not null)
                datePicker.SetValue(DateProperty, newValue);

            datePicker.NullableDateChanged?.Invoke(datePicker, new NullableDateChangedEventArgs((DateTime?)oldValue, (DateTime?)newValue));
        }

        public DateTime? NullableDate
        {
            get { return (DateTime?)GetValue(NullableDateProperty); }
            set { SetValue(NullableDateProperty, value); }
        }

        public event EventHandler<NullableDateChangedEventArgs>? NullableDateChanged;

        public new event EventHandler<DateChangedEventArgs>? DateSelected;

        //public static readonly BindableProperty PlaceHolderProperty = BindableProperty.Create(
        //	propertyName: nameof(PlaceHolder),
        //	returnType: typeof(string),
        //	declaringType: typeof(NullableDatePicker),
        //	defaultValue: string.Empty
        //);

        //public string PlaceHolder
        //{
        //	get => (string)GetValue(PlaceHolderProperty);
        //	set => SetValue(PlaceHolderProperty, value);
        //}
    }

    public class NullableDateChangedEventArgs : EventArgs
    {
        public DateTime? OldDate { get; }

        public DateTime? NewDate { get; }

        public NullableDateChangedEventArgs(DateTime? oldDate, DateTime? newDate)
        {
            OldDate = oldDate;
            NewDate = newDate;
        }
    }
}
