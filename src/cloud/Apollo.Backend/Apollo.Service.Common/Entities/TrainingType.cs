// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Runtime.Intrinsics.X86;

namespace Apollo.Common.Entities
{
    [Flags]
    public enum TrainingType
    {
        Unknown = 0,
        Online = 1,
        Offline = 2,
        Hybrid = 4,
        OnDemand = 8
    }
}
