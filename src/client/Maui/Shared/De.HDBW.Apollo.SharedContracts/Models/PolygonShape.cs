// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Globalization;

namespace De.HDBW.Apollo.SharedContracts.Models
{
    public class PolygonShape : Shape
    {
        public PolygonShape(string coordinates)
        {
            Path = coordinates;
        }

        public string Path { get; }
    }
}
