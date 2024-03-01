// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using CommunityToolkit.Mvvm.ComponentModel;

namespace De.HDBW.Apollo.Client.Models.Training
{
    public partial class HeaderedLineItem : LineItem
    {
        [ObservableProperty]
        private string? _header;

        protected HeaderedLineItem(string? icon, string text, string? header)
            : base(icon, text)
        {
            Header = header;
        }

        public static HeaderedLineItem Import(string? icon, string text, string? header)
        {
            return new HeaderedLineItem(icon, text, header);
        }
    }
}
