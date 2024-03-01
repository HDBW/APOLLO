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

#if (IOS && !MACCATALYST) || ANDROID 
using PlatformView = Microsoft.Maui.Platform.MauiDatePicker;
#elif MACCATALYST
using PlatformView = UIKit.UIDatePicker;
using PlatformView = Microsoft.Maui.Platform.MauiDatePicker;
#elif WINDOWS
using PlatformView = Microsoft.UI.Xaml.Controls.CalendarDatePicker;
#elif TIZEN
using PlatformView = Tizen.UIExtensions.NUI.Entry;
#elif (NETSTANDARD || !PLATFORM) || (NET6_0_OR_GREATER && !IOS && !ANDROID && !TIZEN)
using PlatformView = System.Object;
#endif

namespace FedericoNembrini.Maui.CustomDatePicker.Handlers
{
    public partial class NullableDatePickerHandler : DatePickerHandler, INullableDatePickerHandler
    {
        public NullableDatePickerHandler() : base(Mapper)
        {
#if ANDROID || IOS
            Mapper.Add(nameof(INullableDatePicker.NullableDate), MapNullableDate);
            Mapper.Add(nameof(INullableDatePicker.Date), MapDate);
            Mapper.Add(nameof(INullableDatePicker.Format), MapFormat);
#endif
#if IOS
            Mapper.Add(nameof(INullableDatePicker.FlowDirection), MapFlowDirection);
            Mapper.Add(nameof(INullableDatePicker.TextColor), MapTextColor);
#endif
        }

        public NullableDatePickerHandler(IPropertyMapper? mapper)
            : base(mapper ?? Mapper, CommandMapper)
        {
#if ANDROID || IOS
            Mapper.Add(nameof(INullableDatePicker.NullableDate), MapNullableDate);
            Mapper.Add(nameof(INullableDatePicker.Date), MapDate);
            Mapper.Add(nameof(INullableDatePicker.Format), MapFormat);
#endif
#if IOS
            Mapper.Add(nameof(INullableDatePicker.FlowDirection), MapFlowDirection);
            Mapper.Add(nameof(INullableDatePicker.TextColor), MapTextColor);
#endif
        }

        public NullableDatePickerHandler(IPropertyMapper? mapper, CommandMapper? commandMapper)
            : base(mapper ?? Mapper, commandMapper ?? CommandMapper)
        {
#if ANDROID || IOS
            Mapper.Add(nameof(INullableDatePicker.NullableDate), MapNullableDate);
            Mapper.Add(nameof(INullableDatePicker.Date), MapDate);
            Mapper.Add(nameof(INullableDatePicker.Format), MapFormat);
#endif
#if IOS
            Mapper.Add(nameof(INullableDatePicker.FlowDirection), MapFlowDirection);
            Mapper.Add(nameof(INullableDatePicker.TextColor), MapTextColor);
#endif
        }

#if DEBUG
        public override void UpdateValue(string property)
        {
            System.Diagnostics.Debug.WriteLine($"NullableDatePickerHandler: Update {property}");
            base.UpdateValue(property);
        }
#endif
    }
}
