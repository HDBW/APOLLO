// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using Android.Content;
using Android.Views.InputMethods;
using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;
using AText = Android.Text;

namespace De.HDBW.Apollo.Client.Platforms
{
    public class CustomWebView : MauiWebView
    {
        public CustomWebView(WebViewHandler handler, Context context)
            : base(handler, context)
        {
        }

        public override IInputConnection? OnCreateInputConnection(EditorInfo? outAttrs)
        {
            if (outAttrs != null)
            {
                outAttrs.InputType = AText.InputTypes.ClassText | AText.InputTypes.TextVariationVisiblePassword;
                outAttrs.PrivateImeOptions = "nm,com.google.android.inputmethod.latin.noMicrophoneKey";
            }

            var connection = base.OnCreateInputConnection(outAttrs);
            if (outAttrs != null)
            {
                outAttrs.InputType = AText.InputTypes.ClassText | AText.InputTypes.TextVariationVisiblePassword;
                outAttrs.PrivateImeOptions = "nm,com.google.android.inputmethod.latin.noMicrophoneKey";
            }

            return connection;
        }
    }
}
