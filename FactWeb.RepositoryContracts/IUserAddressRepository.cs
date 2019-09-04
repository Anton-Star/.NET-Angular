using FactWeb.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FactWeb.RepositoryContracts
{
    public interface IUserAddressRepository : IRepository<UserAddress>
    {
        /// <summary>
        /// Gets all User Addresses by user
        /// </summary>
        /// <param name="userId">Id of the user</param>
        /// <returns>Collection of User Addresses</returns>
        List<UserAddress> GetByUser(Guid userId);

        /// <summary>
        /// Gets all User Addresses by user asynchronously
        /// </summary>
        /// <param name="userId">Id of the user</param>
        /// <returns>Collection of User Addresses</returns>
        Task<List<UserAddress>> GetByUserAsync(Guid userId);

        /// <summary>
        /// Gets all User Addresses by address
        /// </summary>
        /// <param name="addressId">Id of the address</param>
        /// <returns>Collection of User Addresses</returns>
        List<UserAddress> GetByAddress(int addressId);

        /// <summary>
        /// Gets all User Addresses by address
        /// </summary>
        /// <param name="addressId">Id of the address</param>
        /// <returns>Collection of User Addresses</returns>
        Task<List<UserAddress>> GetByAddressAsync(int addressId);
    }
}
