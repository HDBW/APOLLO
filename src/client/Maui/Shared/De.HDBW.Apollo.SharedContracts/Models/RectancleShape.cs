// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Globalization;

namespace De.HDBW.Apollo.SharedContracts.Models
{
    public class RectancleShape : Shape
    {
        public RectancleShape(string coordinates)
        {
            var parts = coordinates.Split(",").Select(x => x.Trim()).ToList();
            X = Convert.ToSingle(parts[0], CultureInfo.InvariantCulture);
            Y = Convert.ToSingle(parts[1], CultureInfo.InvariantCulture);
            Width = Convert.ToSingle(parts[2], CultureInfo.InvariantCulture) - X;
            Height = Convert.ToSingle(parts[3], CultureInfo.InvariantCulture) - Y;
        }

        public float X { get; }

        public float Y { get; }

        public float Width { get; }

        public float Height { get; }
    }
}
