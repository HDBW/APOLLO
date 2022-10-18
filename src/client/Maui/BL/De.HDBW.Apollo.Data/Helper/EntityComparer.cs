// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using Google.Protobuf.WellKnownTypes;
using Graph.Apollo.Cloud.Common.Models.Assessment;

namespace De.HDBW.Apollo.Data.Helper
{
    public class EntityComparer<TU> : IEqualityComparer<TU>
        where TU : IEntity, new()
    {
        public bool Equals(TU? x, TU? y)
        {
            if (x == null && y == null)
            {
                return true;
            }

            if (x == null || y == null)
            {
                return false;
            }

            return x.Id == y.Id;
        }

        public int GetHashCode([DisallowNull] TU obj)
        {
            return Convert.ToInt32(obj.Id);
        }
    }
}
