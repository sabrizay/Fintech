using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Fintech.Library.DataAccess.Concrete.Repository
{
    public class EfEntityRepositoryBase<TContext, TEntity> : IEntityRepository<TEntity>
       where TEntity : class, new()
       where TContext : DbContext, new()
    {
        //Command
        public Task Add(TEntity entity)
        {
            try
            {
            using (var context = new TContext())
            {
                context.Add(entity);
                 context.SaveChanges();
            }
                return Task.CompletedTask;
            }
            catch (Exception ex)
            {
                return Task.CompletedTask;
            }

        }

        public Task Delete(TEntity entity)
        {
            using (var context = new TContext())
            {
                context.Remove(entity);
                return context.SaveChangesAsync();
            }

        }
        public async Task Update(TEntity entity)
        {
            try
            {
               
                using (var context = new TContext())
            {
                    context.Set<TEntity>().Update(entity);
                     await context.SaveChangesAsync();
            }
            }
            catch (Exception ex)
            {

                throw;
            }


        }

        //Query
        public Task<TEntity> Get(Expression<Func<TEntity, bool>> expression)
        {
            using (var context = new TContext())
            {
                var entities = context.Set<TEntity>();

                return Task.FromResult(entities.SingleOrDefault(expression));
            }
        }
        public async Task<IEnumerable> GetAll(Expression<Func<TEntity, bool>> expression = null)
        {
            using (var context = new TContext())
            {
                var entities = context.Set<TEntity>();
                if (expression == null)
                {
                    return await entities.ToListAsync();
                }
                else
                {
                    return await entities.Where(expression).ToListAsync();
                }
            }
        }

    }
}
