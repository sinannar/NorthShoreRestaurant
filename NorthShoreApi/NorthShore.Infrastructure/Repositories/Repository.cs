using Microsoft.EntityFrameworkCore;
using NorthShore.Domain.Entities;
using NorthShore.Domain.Repositories;
using NorthShore.EfContext.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace NorthShore.Infrastructure.Repositories
{
    public abstract class Repository<Key, Entity> : IRepository<Key, Entity>
            where Key : IComparable
            where Entity : BaseEntity<Key>
    {
        protected virtual NorthShoreDbContext Context { get; set; }
        protected internal DbSet<Entity> DbSet;

        public Repository(NorthShoreDbContext context)
        {
            Context = context;
            DbSet = context.Set<Entity>();
        }

        public Entity Get(Key key)
        {
            return GetAll()
                .FirstOrDefault(x => x.Id.CompareTo(key) == 0);
        }

        public async Task<Entity> GetAsync(Key id)
        {
            return await GetAll()
                .FirstOrDefaultAsync(x => x.Id.CompareTo(id) == 0);
        }


        public async Task InsertAsync(Entity entity)
        {
            entity.CreatedAt = DateTimeOffset.Now;
            await DbSet.AddAsync(entity);
        }

        public async Task UpdateAsync(Entity entityToUpdate)
        {
            entityToUpdate.CreatedAt = DateTimeOffset.Now;
            await Task.Run(() => Context.Entry(entityToUpdate).State = EntityState.Modified);
        }

        public async Task DeleteAsync(Entity entityToDelete)
        {
            if (entityToDelete != null)
            {
                await Task.Run(() => DbSet.Remove(entityToDelete));
            }
        }

        public IQueryable<Entity> GetAllIncluding(params Expression<Func<Entity, object>>[] propertySelectors)
        {
            IQueryable<Entity> query = GetAll();

            foreach (var includeProperty in propertySelectors)
            {
                query = query.Include(includeProperty);
            }

            return query;
        }

        internal IQueryable<Entity> GetQueryable()
        {
            return DbSet.AsQueryable();
        }

        public IQueryable<Entity> GetAll()
        {
            return DbSet.Where(e => !e.DeletedAt.HasValue);
        }
    }
}
