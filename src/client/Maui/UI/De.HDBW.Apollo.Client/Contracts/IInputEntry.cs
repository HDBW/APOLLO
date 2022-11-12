// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.
namespace De.HDBW.Apollo.Client.Contracts
{
    public interface IInputEntry
    {
        string? Value { get; set; }

        string? Hint { get; }
    }
}
