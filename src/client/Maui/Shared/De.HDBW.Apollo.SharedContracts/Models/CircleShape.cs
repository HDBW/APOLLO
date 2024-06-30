// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Globalization;

namespace De.HDBW.Apollo.SharedContracts.Models
{
    public class CircleShape : Shape
    {
        public CircleShape(string coordinates)
        {
            var parts = coordinates.Split(",").Select(x => x.Trim()).ToList();
            X = Convert.ToSingle(parts[0], CultureInfo.InvariantCulture);
            Y = Convert.ToSingle(parts[1], CultureInfo.InvariantCulture);
            Radius = Convert.ToDouble(parts[2], CultureInfo.InvariantCulture);
        }

        public float X { get; }

        public float Y { get; }

        public double Radius { get; }
    }
}
