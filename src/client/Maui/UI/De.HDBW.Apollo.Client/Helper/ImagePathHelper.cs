// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

namespace De.HDBW.Apollo.Client.Helper
{
    public static class ImagePathHelper
    {
        public static string ToUniformedName(this string imageName)
        {
            imageName = imageName?.ToLower() ?? string.Empty;
            imageName = imageName.Replace("ü","ue");
            imageName = imageName.Replace("ö", "oe");
            imageName = imageName.Replace("ä", "ae");
            imageName = imageName.Replace("ß", "ss");
            return imageName;
        }
    }
}
