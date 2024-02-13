// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using CommunityToolkit.Mvvm.ComponentModel;

namespace De.HDBW.Apollo.Client.Models.Generic
{
    public partial class StringValue : ObservableObject
    {
        [ObservableProperty]
        private string _text;

        [ObservableProperty]
        private string? _data;

        protected StringValue(string text, string? data)
        {
            Text = text;
            _data = data;
        }

        public static StringValue Import(string text, string? data)
        {
            return new StringValue(text, data);
        }
    }
}
