using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace NorthShore.Domain.Repositories
{
    public interface IRepository<Key, Entity> where Entity : class
    {
        Entity Get(Key id);
        Task<Entity> GetAsync(Key id);
        Task InsertAsync(Entity entity);
        Task UpdateAsync(Entity entityToUpdate);
        Task DeleteAsync(Entity entityToDelete);
        IQueryable<Entity> GetAll();
        IQueryable<Entity> GetAllIncluding(params Expression<Func<Entity, object>>[] propertySelectors);
    }
}
