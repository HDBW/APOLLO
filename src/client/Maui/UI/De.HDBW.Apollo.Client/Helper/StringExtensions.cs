// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

namespace De.HDBW.Apollo.Client.Helper
{
    public static class StringExtensions
    {
        public static Stream ToStream(this string data)
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            if (!string.IsNullOrWhiteSpace(data))
            {
                writer.Write(data);
            }

            writer.Flush();
            if (stream.CanSeek)
            {
                stream.Seek(0, SeekOrigin.Begin);
            }
            else
            {
                stream.Position = 0;
            }

            return stream;
        }
    }
}
