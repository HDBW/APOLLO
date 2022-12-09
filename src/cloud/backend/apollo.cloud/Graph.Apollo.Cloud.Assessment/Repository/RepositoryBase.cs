using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Invite.Apollo.App.Graph.Assessment.Data;

namespace Invite.Apollo.App.Graph.Assessment.Repository
{
    /*
    Base Class for all Repros class
    TODO:async stuff
    */

    public abstract class RepositoryBase<T> where T : class
    {

        //REVIEW: Maybe something else???
        private AssessmentContext _context;
        private DbSet<T> dbSet;
        public RepositoryBase(AssessmentContext context)
        {
            _context = context;
            _context.Set<T>().Load();
            dbSet = _context.Set<T>();
        }

        public virtual IEnumerable<T> GetAll()
        {
            return dbSet.AsEnumerable();
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            return await dbSet.ToListAsync();
        }
        
        public T GetSingle(params object[] keys)
        {
            return dbSet.Find(keys);

        }

        public T GetSingle(Expression<Func<T, bool>> predicate)
        {
            return dbSet.FirstOrDefault(predicate);
        }



        public async Task<T> GetSingleAsync(params object[] keys)
        {
            return await dbSet.FindAsync(keys);
        }

        public virtual IEnumerable<T> FindBy(Expression<Func<T, bool>> predicate)
        {
            return dbSet.Where(predicate);
        }

        public virtual async Task<IEnumerable<T>> FindByAsync(Expression<Func<T, bool>> predicate)
        {

            return await dbSet.Where(predicate).ToListAsync();
        }


        public IEnumerable<T> FindBy(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes)
        {
            var query = dbSet.Where(predicate);
            return LoadNavigationProperty(query, includes);
        }


        public IQueryable<T> FindByAndInclude(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes)
        {
            var query = dbSet.Where(predicate);
            return LoadNavigationProperty(query, includes);
        }

        private IQueryable<T> LoadNavigationProperty(IQueryable<T> query, Expression<Func<T, object>>[] includes)
        {
            if ((query != null) && (includes != null))
            {
                foreach (var navigationProperty in includes)
                {
                    query = query.Include<T, object>(navigationProperty);
                }
            }

            return query;
        }

        //https://learn.microsoft.com/en-us/ef/core/performance/efficient-updating
        //https://learn.microsoft.com/en-us/dotnet/api/microsoft.entityframeworkcore.changetracking.propertyentry.ismodified?view=efcore-6.0#microsoft-entityframeworkcore-changetracking-propertyentry-ismodified
        //https://learn.microsoft.com/en-us/ef/core/querying/tracking?source=recommendations
        //https://learn.microsoft.com/en-us/ef/core/change-tracking/miscellaneous
        //https://learn.microsoft.com/en-us/ef/core/logging-events-diagnostics/
        public virtual void Add(T entity)
        {
            EntityEntry dbEntityEntry = _context.Entry<T>(entity);
            _context.Set<T>().Add(entity);
        }

        public virtual void Edit(T entity)
        {
            EntityEntry dbEntityEntry = _context.Entry<T>(entity);
            dbEntityEntry.State = EntityState.Modified;
        }
        public virtual void Delete(T entity)
        {
            EntityEntry dbEntityEntry = _context.Entry<T>(entity);
            dbEntityEntry.State = EntityState.Deleted;
        }

        public virtual void Commit()
        {
            _context.SaveChanges();
        }

    }
}
