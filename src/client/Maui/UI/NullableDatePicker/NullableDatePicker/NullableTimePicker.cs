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
    public class NullableTimePicker : TimePicker
    {
        public bool IsClearButtonVisible { get; set; } = true;

        public static readonly BindableProperty NullableTimeProperty = BindableProperty.Create(
            propertyName: nameof(NullableTime),
            returnType: typeof(TimeSpan?),
            declaringType: typeof(NullableTimePicker),
            defaultValue: null,
            defaultBindingMode: BindingMode.TwoWay
        );

        public TimeSpan? NullableTime
        {
            get => (TimeSpan?)GetValue(NullableTimeProperty);
            set => SetValue(NullableTimeProperty, value);
        }

        public static readonly BindableProperty PlaceHolderProperty = BindableProperty.Create(
            propertyName: nameof(PlaceHolder),
            returnType: typeof(string),
            declaringType: typeof(NullableTimePicker),
            string.Empty
        );

        public string PlaceHolder
        {
            get => (string)GetValue(PlaceHolderProperty);
            set => SetValue(PlaceHolderProperty, value);
        }

        public NullableTimePicker() { }

        protected override void OnPropertyChanged(string? propertyName = null)
        {
            base.OnPropertyChanged(propertyName);

            if (propertyName == TimeProperty.PropertyName || (propertyName == IsFocusedProperty.PropertyName && !IsFocused && (Time.ToString("t") == DateTime.Now.ToString("t"))))
            {
                AssignValue();
            }

            if (propertyName == NullableTimeProperty.PropertyName && NullableTime.HasValue)
            {
                Time = NullableTime.Value;
            }
        }

        public void AssignValue()
        {
            NullableTime = Time;
        }
    }
}
