using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FactWeb.RepositoryContracts
{
    public interface IRepository<T>
    {
        /// <summary>
        /// Gets a entity object by its id
        /// </summary>
        /// <param name="id">Id of the record</param>
        /// <returns>Entity object</returns>
        T GetById(int id);

        /// <summary>
        /// Gets a entity object by its id
        /// </summary>
        /// <param name="id">Id of the record</param>
        /// <returns>Entity object</returns>
        T GetById(Guid id);

        /// <summary>
        /// Gets all of the records for the entity object
        /// </summary>
        /// <returns>Collection of entity objects</returns>
        List<T> GetAll();

        /// <summary>
        /// Gets all of the records for the entity object asynchronously
        /// </summary>
        /// <returns>Asynchronous task with a collection of entity objects</returns>
        Task<List<T>> GetAllAsync();

        /// <summary>
        /// Adds a new item to the associated entity database
        /// </summary>
        /// <param name="item">New entity object to be added</param>
        void Add(T item);

        /// <summary>
        /// Adds a new item to the associated entity database asynchronously
        /// </summary>
        /// <param name="item">New entity object to be added</param>
        Task AddAsync(T item);

        /// <summary>
        /// Adds a new item to the repository but doesnt commit. Requires SaveChanges to be called when complete.
        /// </summary>
        /// <param name="item">New entity object to be added</param>
        void BatchAdd(T item);

        /// <summary>
        /// commits pending changes to the repository
        /// </summary>
        void SaveChanges();

        /// <summary>
        /// commits pending changes to the repository asynchronously
        /// </summary>
        Task SaveChangesAsync();

        /// <summary>
        /// Removes an object from the repository
        /// </summary>
        /// <param name="item">Item to be removed</param>
        void Remove(T item);

        /// <summary>
        /// Removes an object from the repository asynchronously
        /// </summary>
        /// <param name="item">Item to be removed</param>
        Task RemoveAsync(T item);

        /// <summary>
        /// Removes an object from the repository
        /// </summary>
        /// <param name="id">Id of the object to be removed</param>
        void Remove(int id);

        /// <summary>
        /// Removes an object from the repository
        /// </summary>
        /// <param name="id">Id of the object to be removed</param>
        void Remove(Guid id);

        /// <summary>
        /// Removes an object from the repository asynchronously
        /// </summary>
        /// <param name="id">Id of the object to be removed</param>
        Task RemoveAsync(int id);

        /// <summary>
        /// Removes an object from the repository asynchronously
        /// </summary>
        /// <param name="id">Id of the object to be removed</param>
        Task RemoveAsync(Guid id);

        /// <summary>
        /// Removes an item from the repository but doesnt commit. Requires SaveChanges to be called when complete.
        /// </summary>
        /// <param name="item"></param>
        void BatchRemove(T item);

        /// <summary>
        /// Updates a record in the repository
        /// </summary>
        /// <param name="item">Item object of the changes</param>
        void Save(T item);

        /// <summary>
        /// Updates a record in the repository asynchronously
        /// </summary>
        /// <param name="item">Item object of the changes</param>
        Task SaveAsync(T item);

        /// <summary>
        /// Updates a record in the repository but doesnt commit. Requires SaveChanges to be called when complete.
        /// </summary>
        /// <param name="item"></param>
        void BatchSave(T item);
    }
}
