using FactWeb.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FactWeb.RepositoryContracts
{
    public interface IUserCredentialRepository : IRepository<UserCredential>
    {
        List<UserCredential> GetAllByUserId(Guid? id);

        Task<List<UserCredential>> GetAllByUserIdAsync(Guid? id);

    }
}

