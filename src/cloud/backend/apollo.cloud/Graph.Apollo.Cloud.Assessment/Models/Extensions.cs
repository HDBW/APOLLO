using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace Microsoft.Data.Entity.Extensions
{
    public static class Extensions
    {
        public static TEntity Find<TEntity>(this DbSet<TEntity> set, params object[] keyValues) where TEntity : class
        {

            // Find DbContext, entity type, and primary key.
            var context = ((IInfrastructure<IServiceProvider>)set).GetService<DbContext>();
            var entityType = context.Model.FindEntityType(typeof(TEntity));
            var key = entityType.FindPrimaryKey();

            // Build the lambda expression for the query: (TEntity entity) => AND( entity.keyProperty[i] == keyValues[i])
            var entityParameter = Expression.Parameter(typeof(TEntity), "entity");
            Expression whereClause = Expression.Constant(true, typeof(bool));
            uint i = 0;
            foreach (var keyProperty in key.Properties)
            {
                var keyMatch = Expression.Equal(
                    Expression.Property(entityParameter, keyProperty.Name),
                    Expression.Constant(keyValues[i++])
                );
                whereClause = Expression.And(whereClause, keyMatch);
            }
            var lambdaExpression = (Expression<Func<TEntity, bool>>)Expression.Lambda(whereClause, entityParameter);

            //TODO: need to Test this ! I think it will not work!
            // Execute against the in-memory entities, which we get from ChangeTracker (but not filtering the state of the entities).
            //var entries = context.ChangeTracker.Entries<TEntity>().Select((EntityEntry e) => (TEntity)e.Entity);
            //TEntity entity = entries.AsQueryable().Where(lambdaExpression).First(); // First is what triggers the query execution.

            //// If found in memory then we're done.
            //if (entity != null) { return entity; }

            // Otherwise execute the qeuery against the database.
            return set.Where(lambdaExpression).First();


        }

        public static async System.Threading.Tasks.Task<TEntity> FindAsync<TEntity>(this DbSet<TEntity> set, params object[] keyValues) where TEntity : class
        {

            // Find DbContext, entity type, and primary key.
            var context = ((IInfrastructure<IServiceProvider>)set).GetService<DbContext>();
            var entityType = context.Model.FindEntityType(typeof(TEntity));
            var key = entityType.FindPrimaryKey();

            // Build the lambda expression for the query: (TEntity entity) => AND( entity.keyProperty[i] == keyValues[i])

            var entityParameter = Expression.Parameter(typeof(TEntity), "entity");
            Expression whereClause = Expression.Constant(true, typeof(bool));
            uint i = 0;
            foreach (var keyProperty in key.Properties)
            {
                var keyMatch = Expression.Equal(
                    Expression.Property(entityParameter, keyProperty.Name),
                    Expression.Constant(keyValues[i++])
                );
                whereClause = Expression.And(whereClause, keyMatch);
            }
            var lambdaExpression = (Expression<Func<TEntity, bool>>)Expression.Lambda(whereClause, entityParameter);

            // Execute against the in-memory entities, which we get from ChangeTracker (but not filtering the state of the entities).
            var entries = context.ChangeTracker.Entries<TEntity>().Select((EntityEntry e) => (TEntity)e.Entity);

            //TODO:local search doesn`t work -need a fix for 
            //TEntity entity = null;
            //if (entries != null)
            //    entity = await entries.AsQueryable().Where(lambdaExpression).FirstAsync(); // First is what triggers the query execution.

            //// If found in memory then we're done.
            //if (entity != null) { return entity; }

            // Otherwise execute the qeuery against the database.
            return await set.Where(lambdaExpression).FirstAsync();


        }


    }
}
