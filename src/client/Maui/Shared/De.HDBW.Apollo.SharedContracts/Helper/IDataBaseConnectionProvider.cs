// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using SQLite;

namespace De.HDBW.Apollo.SharedContracts.Helper
{
    public interface IDataBaseConnectionProvider
    {
        Task<SQLiteAsyncConnection> GetConnectionAsync(CancellationToken token);
    }
}