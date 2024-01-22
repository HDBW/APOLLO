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

using Android.App;
using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;

namespace FedericoNembrini.Maui.CustomDatePicker.Handlers
{
    public partial class NullableDatePickerHandler
    {
        DatePickerDialog? _datePickerDialog;

        protected override MauiDatePicker CreatePlatformView()
        {
            MauiDatePicker mauiDatePicker = base.CreatePlatformView();

            mauiDatePicker.ShowPicker = ShowPickerDialog;

            return mauiDatePicker;
        }

        protected override void ConnectHandler(MauiDatePicker platformView)
        {
            base.ConnectHandler(platformView);

            UpdateDisplayDate();
        }

        protected override DatePickerDialog CreateDatePickerDialog(int year, int month, int day)
        {
            _datePickerDialog = base.CreateDatePickerDialog(year, month, day);

            _datePickerDialog.SetButton(Resources.OkButton, (sender, e) =>
            {
                PlatformView.Text = _datePickerDialog.DatePicker.DateTime.ToString(VirtualView.Format);

                VirtualView.Date = _datePickerDialog.DatePicker.DateTime;
                (VirtualView as INullableDatePicker)!.NullableDate = _datePickerDialog.DatePicker.DateTime;

                PlatformView.ClearFocus();
            });

            if (!(VirtualView as INullableDatePicker)!.IsClearButtonVisible)
            {
                _datePickerDialog.SetButton2(Resources.CancelButton, (sender, e) =>
                {
                    PlatformView.ClearFocus();
                });
            }

            if ((VirtualView as INullableDatePicker)!.IsClearButtonVisible)
            {
                _datePickerDialog.SetButton2(Resources.ClearButton, (sender, e) =>
                {
                    (VirtualView as INullableDatePicker)!.NullableDate = null;
                    PlatformView.ClearFocus();
                });

                _datePickerDialog.SetButton3(Resources.CancelButton, (sender, e) =>
                {
                    PlatformView.ClearFocus();
                });
            }

            return _datePickerDialog;
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

        void ShowPickerDialog()
        {
            if (VirtualView == null)
                return;

            if (_datePickerDialog != null && _datePickerDialog.IsShowing)
                return;

            if (VirtualView is not INullableDatePicker)
                return;

            int year, month, day;

            if ((VirtualView as INullableDatePicker)!.NullableDate.HasValue)
            {
                year = (VirtualView as INullableDatePicker)!.NullableDate!.Value.Year;
                month = (VirtualView as INullableDatePicker)!.NullableDate!.Value.Month;
                day = (VirtualView as INullableDatePicker)!.NullableDate!.Value.Day;
            }
            else
            {
                if (VirtualView.MinimumDate != new DateTime(1900, 1, 1).Date && VirtualView.MinimumDate != DateTime.MinValue)
                {
                    year = VirtualView.MinimumDate.Year;
                    month = VirtualView.MinimumDate.Month;
                    day = VirtualView.MinimumDate.Day;
                }
                else
                {
                    year = DateTime.Today.Year;
                    month = DateTime.Today.Month;
                    day = DateTime.Today.Day;
                }
            }

            ShowPickerDialog(year, month - 1, day);
        }

        void ShowPickerDialog(int year, int month, int day)
        {
            if (_datePickerDialog == null)
            {
                _datePickerDialog = CreateDatePickerDialog(year, month, day);
            }
            else
            {
                EventHandler? setDateLater = null;
                setDateLater = (sender, e) => { _datePickerDialog!.UpdateDate(year, month, day); _datePickerDialog.ShowEvent -= setDateLater; };
                _datePickerDialog.ShowEvent += setDateLater;
            }

            _datePickerDialog.Show();
        }

        public static void MapNullableDate(IDatePickerHandler handler, IDatePicker datePicker)
        {
            handler.PlatformView.Text = (datePicker as INullableDatePicker)?.NullableDate?.ToString(datePicker.Format);
        }

        public static new void MapDate(IDatePickerHandler handler, IDatePicker datePicker)
        {
            //handler.PlatformView?.UpdateDate(datePicker);
        }

        public static new void MapFormat(IDatePickerHandler handler, IDatePicker datePicker)
        {
            handler.PlatformView?.UpdateFormat(datePicker);

            if (handler.PlatformView != null)
                handler.PlatformView.Text = (datePicker as INullableDatePicker)?.NullableDate?.ToString(datePicker.Format);
        }
    }
}
