// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

namespace Apollo.Common.Entities
{
    /// <summary>
    /// This is used to store Professional career information from different platforms of the User.
    /// Maybe we can integrate and automate that process in the future.
    /// </summary>
    public class WebReference
    {
        // WebReference_Link_filtered.txt
        // Freitext
        public Uri Url { get; set; }

        // WebReference_Title_filtered.txt
        // Freitext
        public string Title { get; set; }
    }
}
