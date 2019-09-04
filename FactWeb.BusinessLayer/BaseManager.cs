using FactWeb.RepositoryContracts;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FactWeb.BusinessLayer
{
    public abstract class BaseManager<TManager, TRepository, TItem> where TRepository : IRepository<TItem>
    {
        protected readonly TRepository Repository;

        protected readonly ILog Log = LogManager.GetLogger(typeof(TManager));

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="repository">Generic IRepository instance</param>
        protected BaseManager(TRepository repository)
        {
            this.Repository = repository;
        }

        /// <summary>
        /// Adds a new item to the associated entity database
        /// </summary>
        /// <param name="item">New entity object to be added</param>
        public virtual void Add(TItem item)
        {
            LogMessage("Add for " + item.GetType().ToString());

            this.Repository.Add(item);
        }

        /// <summary>
        /// Updates a record in the repository
        /// </summary>
        /// <param name="item">Item object of the changes</param>
        public virtual void Save(TItem item)
        {
            LogMessage("Save for " + item.GetType().ToString());

            this.Repository.Save(item);
        }

        /// <summary>
        /// Updates a record in the repository
        /// </summary>
        /// <param name="item">Item object of the changes</param>
        public virtual async Task SaveAsync(TItem item)
        {
            LogMessage("Save for " + item.GetType().ToString());

            await this.Repository.SaveAsync(item);
        }

        /// <summary>
        /// Adds a new item to the repository but doesnt commit. Requires SaveChanges to be called when complete.
        /// </summary>
        /// <param name="item">New entity object to be added</param>
        public void BatchAdd(TItem item)
        {
            LogMessage("BatchAdd for " + item.GetType().ToString());

            this.Repository.BatchAdd(item);
        }

        /// <summary>
        /// commits pending changes to the repository
        /// </summary>
        public void SaveChanges()
        {
            LogMessage("SaveChanges");

            this.Repository.SaveChanges();
        }

        /// <summary>
        /// commits pending changes to the repository asynchronously
        /// </summary>
        public Task SaveChangesAsync()
        {
            LogMessage("SaveChangesAsync");

            return this.Repository.SaveChangesAsync();
        }

        /// <summary>
        /// Removes an item from the repository but doesnt commit. Requires SaveChanges to be called when complete.
        /// </summary>
        /// <param name="item"></param>
        public void BatchRemove(TItem item)
        {
            LogMessage("BatchRemove for " + item.GetType().ToString());

            this.Repository.BatchRemove(item);
        }

        /// <summary>
        /// Updates a record in the repository but doesnt commit. Requires SaveChanges to be called when complete.
        /// </summary>
        /// <param name="item"></param>
        public void BatchSave(TItem item)
        {
            LogMessage("BatchSave for " + item.GetType().ToString());

            this.Repository.BatchSave(item);
        }

        /// <summary>
        /// Removes an object from the repository
        /// </summary>
        /// <param name="id">Item to be removed</param>
        public virtual void Remove(int id)
        {
            LogMessage("Remove with id");

            this.Repository.Remove(id);
        }

        /// <summary>
        /// Removes an object from the repository
        /// </summary>
        /// <param name="id">Item to be removed</param>
        public virtual void Remove(Guid id)
        {
            LogMessage("Remove with Guid");

            this.Repository.Remove(id);
        }

        /// <summary>
        /// Removes an object from the repository
        /// </summary>
        /// <param name="id">Item to be removed</param>
        public virtual async Task RemoveAsync(Guid id)
        {
            LogMessage("Remove with Guid");

            await this.Repository.RemoveAsync(id);
        }

        /// <summary>
        /// Gets all of the records for the entity object
        /// </summary>
        /// <returns>Collection of entity objects</returns>
        public virtual List<TItem> GetAll()
        {
            LogMessage("GetAll");

            return this.Repository.GetAll().ToList();
        }

        /// <summary>
        /// Gets all of the records for the entity object asynchronously
        /// </summary>
        /// <returns>Asynchronous task with a collection of entity objects</returns>
        public virtual Task<List<TItem>> GetAllAsync()
        {
            LogMessage("GetAllAsync");

            return this.Repository.GetAllAsync();
        }

        /// <summary>
        /// Gets a entity object by its id
        /// </summary>
        /// <param name="id">Id of the record</param>
        /// <returns>Entity object</returns>
        public virtual TItem GetById(int id)
        {
            LogMessage("GetById");

            return this.Repository.GetById(id);
        }

        /// <summary>
        /// Gets a entity object by its id
        /// </summary>
        /// <param name="id">Id of the record</param>
        /// <returns>Entity object</returns>
        public virtual TItem GetById(Guid id)
        {
            LogMessage("GetById");

            return this.Repository.GetById(id);
        }

        public void LogMessage(string methodName)
        {
            this.Log.Debug("Method: " + methodName + " called.");
        }
    }
}
