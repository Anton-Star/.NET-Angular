using FactWeb.Model;
using FactWeb.RepositoryContracts;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FactWeb.Repository
{
    public class UserCredentialRepository : BaseRepository<UserCredential>, IUserCredentialRepository
    {
        public UserCredentialRepository(FactWebContext context) : base(context)
        {
        }

        public List<UserCredential> GetAllByUserId(Guid? id)
        {
            Guid? userId = id.GetValueOrDefault();
            return base.FetchMany(x => x.UserId == userId);
        }

        public Task<List<UserCredential>> GetAllByUserIdAsync(Guid? id)
        {
            Guid? userId = id.GetValueOrDefault();
            return base.FetchManyAsync(x => x.UserId == userId);
        }
    }
}
