// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using Microsoft.Maui.Handlers;

namespace De.HDBW.Apollo.Client.Behaviors
{
    public class ShowDatePickerBehavior : Behavior<ImageButton>
    {
        public static readonly BindableProperty PickerControlProperty = BindableProperty.Create(
            propertyName: nameof(PickerControl),
            returnType: typeof(DatePicker),
            declaringType: typeof(ShowDatePickerBehavior),
            defaultValue: null,
            defaultBindingMode: BindingMode.OneWay);

        public DatePicker PickerControl
        {
            get => (DatePicker)GetValue(PickerControlProperty);
            set => SetValue(PickerControlProperty, value);
        }

        protected override void OnAttachedTo(ImageButton bindable)
        {
            base.OnAttachedTo(bindable);
            bindable.Clicked += HandleClicked;
        }

        protected override void OnDetachingFrom(ImageButton bindable)
        {
            base.OnDetachingFrom(bindable);
            bindable.Clicked -= HandleClicked;
        }

        private void HandleClicked(object? sender, EventArgs e)
        {
            var datePicker = PickerControl as DatePicker;
            if (datePicker == null)
            {
                return;
            }

            var handler = datePicker.Handler as IDatePickerHandler;
            if (handler == null)
            {
                return;
            }

            handler.PlatformView.PerformClick();

            datePicker.Focus();
        }
    }
}
