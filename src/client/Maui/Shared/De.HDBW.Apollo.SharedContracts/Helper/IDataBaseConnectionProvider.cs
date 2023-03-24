using SQLite;

namespace De.HDBW.Apollo.SharedContracts.Helper
{
    public interface IDataBaseConnectionProvider
    {
        Task<SQLiteAsyncConnection> GetConnectionAsync(CancellationToken token);
    }
}