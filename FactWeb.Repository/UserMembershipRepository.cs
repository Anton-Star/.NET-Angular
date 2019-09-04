using FactWeb.Model;
using FactWeb.RepositoryContracts;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FactWeb.Repository
{
    public class UserMembershipRepository : BaseRepository<UserMembership>, IUserMembershipRepository
    {
        public UserMembershipRepository(FactWebContext context) : base(context)
        {
        }

        public List<UserMembership> GetAllByUserId(Guid? id)
        {
            Guid? userId = id.GetValueOrDefault();
            return base.FetchMany(x => x.UserId == userId);
        }

        public Task<List<UserMembership>> GetAllByUserIdAsync(Guid? id)
        {
            Guid? userId = id.GetValueOrDefault();
            return base.FetchManyAsync(x => x.UserId == userId);
        }
    }
}

