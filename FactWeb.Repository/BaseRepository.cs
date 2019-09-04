using FactWeb.Model;
using FactWeb.RepositoryContracts;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace FactWeb.Repository
{
    public abstract class BaseRepository<T> : IRepository<T> where T : BaseModel
    {
        private readonly IDbSet<T> dbset;
        private readonly FactWebContext context;


        protected IDbSet<T> Dbset
        {
            get { return this.dbset; }
        }

        protected FactWebContext Context
        {
            get { return context; }
        }

        protected BaseRepository(FactWebContext context)
        {
            this.context = context;
            this.dbset = context.SetEntity<T>();
        }

        public virtual T GetById(int id)
        {
            return this.dbset.Find(id);
        }

        public virtual T GetById(Guid id)
        {
            return this.dbset.Find(id);
        }

        public virtual T Fetch(Expression<Func<T, bool>> where)
        {
            return this.dbset.FirstOrDefault(where);
        }

        public virtual Task<T> FetchAsync(Expression<Func<T, bool>> where)
        {
            return this.dbset.FirstOrDefaultAsync(where);
        }

        public virtual List<T> GetAll()
        {
            return this.dbset.ToList();
        }

        public virtual Task<List<T>> GetAllAsync()
        {
            return this.dbset.ToListAsync();
        }

        public virtual void Add(T item)
        {
            this.dbset.Add(item);
            this.context.SaveChanges();
        }

        public virtual async Task AddAsync(T item)
        {
            this.dbset.Add(item);
            await this.context.SaveChangesAsync();
        }

        public virtual void BatchAdd(T item)
        {
            this.dbset.Add(item);
        }

        public virtual void SaveChanges()
        {
            this.context.SaveChanges();
        }

        public virtual async Task SaveChangesAsync()
        {
            await this.context.SaveChangesAsync();
        }

        public virtual void Remove(T item)
        {
            if (item == null) return;
            this.dbset.Remove(item);
            this.context.SaveChanges();
        }

        public virtual async Task RemoveAsync(T item)
        {
            if (item != null)
            {
                this.dbset.Remove(item);
                await this.context.SaveChangesAsync();
            }
        }

        public virtual async Task RemoveAsync(Guid id)
        {
            await this.RemoveAsync(this.GetById(id));
        }

        public virtual void BatchRemove(T item)
        {
            if (item != null)
            {
                this.dbset.Remove(item);
            }
        }

        public virtual List<T> FetchMany(Expression<Func<T, bool>> where)
        {
            return this.dbset.Where(where).ToList();
        }

        public virtual Task<List<T>> FetchManyAsync(Expression<Func<T, bool>> where)
        {
            return this.dbset.Where(where).ToListAsync();
        }

        public virtual void Save(T item)
        {
            this.context.SetModified(item);
            this.context.SaveChanges();
        }

        public virtual async Task SaveAsync(T item)
        {
            this.context.SetModified(item);
            await this.context.SaveChangesAsync();
        }

        public virtual void BatchSave(T item)
        {
            this.context.SetModified(item);
        }

        public virtual void Remove(int id)
        {
            this.Remove(this.GetById(id));
        }

        public virtual void Remove(Guid id)
        {
            this.Remove(this.GetById(id));
        }

        public virtual async Task RemoveAsync(int id)
        {
            await this.RemoveAsync(this.GetById(id));
        }
    }
}
