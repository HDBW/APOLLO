﻿// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

namespace Apollo.Common.Entities
{
    /// <summary>
    /// This is used to store Professional career information from different platforms of the User.
    /// Maybe we can integrate and automate that process in the future.
    /// </summary>
    public class WebReference : EntityBase
    {
        private Uri? _url;
        ///// <summary>
        ///// Any string describing the WebReference. Not needed by Backend. It is fully maintained by the caller.
        ///// </summary>
        //public string? Id { get; set; }

        /// <summary>
        /// WebReference_Link_filtered.txt
        /// Freitext
        /// </summary>
        public Uri? Url
        {
            get
            {
                if (_url != null)
                {
                    // Remove trailing slash if present
                    string urlString = _url.ToString();
                    if (urlString.EndsWith("/"))
                    {
                        urlString = urlString.TrimEnd('/');
                    }

                    //Console.WriteLine($"Returning URL: {urlString}");

                    return new Uri(urlString, UriKind.RelativeOrAbsolute);
                }
                else
                {
                    return null;
                }
            }
            set
            {
                _url = value;
            }
        }


        /// <summary>
        /// WebReference_Link_filtered.txt
        /// Freitext
        /// </summary>
        public string? Title { get; set; }
    }
}
