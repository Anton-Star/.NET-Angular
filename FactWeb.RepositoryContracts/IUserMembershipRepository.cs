using FactWeb.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FactWeb.RepositoryContracts
{
    public interface IUserMembershipRepository : IRepository<UserMembership>
    {
        List<UserMembership> GetAllByUserId(Guid? id);

        Task<List<UserMembership>> GetAllByUserIdAsync(Guid? id);

    }
}

