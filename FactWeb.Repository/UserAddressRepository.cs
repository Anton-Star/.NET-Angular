using FactWeb.Model;
using FactWeb.RepositoryContracts;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FactWeb.Repository
{
    public class UserAddressRepository : BaseRepository<UserAddress>, IUserAddressRepository
    {
        public UserAddressRepository(FactWebContext context) : base(context)
        {
        }

        public List<UserAddress> GetByUser(Guid userId)
        {
            return base.FetchMany(x => x.UserId == userId);
        }

        public Task<List<UserAddress>> GetByUserAsync(Guid userId)
        {
            return base.FetchManyAsync(x => x.UserId == userId);
        }

        public List<UserAddress> GetByAddress(int addressId)
        {
            return base.FetchMany(x => x.AddressId == addressId);
        }

        public Task<List<UserAddress>> GetByAddressAsync(int addressId)
        {
            return base.FetchManyAsync(x => x.AddressId == addressId);
        }
    }
}
