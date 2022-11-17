using System.Linq.Expressions;

namespace Invite.Apollo.App.Graph.Assessment.Repository
{
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> GetAll();
        Task<IEnumerable<T>> GetAllAsync();
        T GetSingle(Expression<Func<T, bool>> predicate);
        T GetSingle(params object[] keys);
        Task<T> GetSingleAsync(params object[] keys);

        IEnumerable<T> FindBy(Expression<Func<T, bool>> predicate);
        Task<IEnumerable<T>> FindByAsync(Expression<Func<T, bool>> predicate);
        IEnumerable<T> FindBy(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes);
        IQueryable<T> FindByAndInclude(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties);


        void Add(T entity);
        void Delete(T entity);
        void Edit(T entity);
        void Commit();
    }
}
