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

using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;
using UIKit;

namespace FedericoNembrini.Maui.CustomDatePicker.Handlers
{
    public partial class NullableDatePickerHandler
    {
        MauiDatePicker mauiDatePicker;

        UIDatePicker uiDatePicker { get => mauiDatePicker.InputView as UIDatePicker; }

        protected override MauiDatePicker CreatePlatformView()
        {
            mauiDatePicker = base.CreatePlatformView();

            uiDatePicker.PreferredDatePickerStyle = UIDatePickerStyle.Wheels;
            uiDatePicker.ValueChanged += HandleValueChanged;

            UIToolbar originalToolbar = mauiDatePicker.InputAccessoryView as UIToolbar;

            UIBarButtonItem newDoneButton = new("Ok", UIBarButtonItemStyle.Done, HandleDoneButton);
            UIBarButtonItem clearButton = new("Clear", UIBarButtonItemStyle.Plain, HandleClearButton);

            List<UIBarButtonItem> newToolbarItems = new();
            foreach (UIBarButtonItem toolbarItem in originalToolbar.Items)
            {
                if (toolbarItem.Style == UIBarButtonItemStyle.Done)
                    newToolbarItems.Add(newDoneButton);
                else
                    newToolbarItems.Add(toolbarItem);
            }

            if ((VirtualView as INullableDatePicker).IsClearButtonVisible)
                newToolbarItems.Insert(0, clearButton);

            originalToolbar.Items = newToolbarItems.ToArray();
            originalToolbar.SetNeedsDisplay();

            return mauiDatePicker;
        }

        protected override void ConnectHandler(MauiDatePicker platformView)
        {
            base.ConnectHandler(platformView);

            UpdateDisplayDate();
        }

        protected override void DisconnectHandler(MauiDatePicker platformView)
        {
            platformView.Dispose();
            base.DisconnectHandler(platformView);
        }

        public void UpdateDisplayDate()
        {
            PlatformView.Text = (VirtualView as INullableDatePicker)?.NullableDate?.ToString(VirtualView.Format);
        }

        void HandleValueChanged(object? sender, EventArgs e)
        {

        }

        void HandleDoneButton(object? sender, EventArgs e)
        {
            (VirtualView as INullableDatePicker).NullableDate = uiDatePicker.Date.ToDateTime();
            PlatformView.ResignFirstResponder();
        }

        void HandleClearButton(object? sender, EventArgs e)
        {
            (VirtualView as INullableDatePicker).NullableDate = null;
            PlatformView.ResignFirstResponder();
        }

        public static void MapNullableDate(IDatePickerHandler handler, IDatePicker datePicker)
        {
            handler.PlatformView.Text = (datePicker as INullableDatePicker)?.NullableDate?.ToString(datePicker.Format);
        }

        public static new void MapDate(IDatePickerHandler handler, IDatePicker datePicker)
        {

        }

        public static new void MapFormat(IDatePickerHandler handler, IDatePicker datePicker)
        {
            handler.PlatformView.UpdateFormat(datePicker);

            if (handler.PlatformView != null)
                handler.PlatformView.Text = (datePicker as INullableDatePicker)?.NullableDate?.ToString(datePicker.Format);
        }

        public static new void MapFlowDirection(DatePickerHandler handler, IDatePicker datePicker)
        {
            handler.PlatformView?.UpdateFlowDirection(datePicker);
            handler.PlatformView?.UpdateTextAlignment(datePicker);

            if (handler.PlatformView != null)
                handler.PlatformView.Text = (datePicker as INullableDatePicker)?.NullableDate?.ToString(datePicker.Format);
        }

        public static new void MapTextColor(IDatePickerHandler handler, IDatePicker datePicker)
        {
            handler.PlatformView?.UpdateTextColor(datePicker);

            if (handler.PlatformView != null)
                handler.PlatformView.Text = (datePicker as INullableDatePicker)?.NullableDate?.ToString(datePicker.Format);
        }
    }
}
