using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Fintech.Library.DataAccess.Concrete.Repository
{
    public interface IEntityRepository<TEntity>
    {
        Task Add(TEntity entity);

        Task Delete(TEntity entity);
        Task Update(TEntity entity);

        Task<TEntity> Get(Expression<Func<TEntity, bool>> expression);
        Task<IEnumerable> GetAll(Expression<Func<TEntity, bool>> expression = null);
    }
}
