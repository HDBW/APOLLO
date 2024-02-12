﻿// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using Microsoft.Maui.Handlers;

namespace De.HDBW.Apollo.Client.Behaviors
{
    public class ShowPickerBehavior : Behavior<ImageButton>
    {
        public static readonly BindableProperty PickerControlProperty = BindableProperty.Create(
            propertyName: nameof(PickerControl),
            returnType: typeof(Picker),
            declaringType: typeof(ShowPickerBehavior),
            defaultValue: null,
            defaultBindingMode: BindingMode.OneWay);

        public Picker PickerControl
        {
            get => (Picker)GetValue(PickerControlProperty);
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
            var picker = PickerControl as Picker;
            if (picker == null)
            {
                return;
            }

            var handler = picker.Handler as IPickerHandler;
            if (handler == null)
            {
                return;
            }
#if ANDROID
            handler.PlatformView.PerformClick();
#elif IOS
            handler.PlatformView.BecomeFirstResponder();
#endif
            picker.Focus();
        }
    }
}