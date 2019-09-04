using FactWeb.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FactWeb.RepositoryContracts
{
    public interface IUserJobFunctionRepository : IRepository<UserJobFunction>
    {
        List<UserJobFunction> GetAllByUserId(Guid? id);

        Task<List<UserJobFunction>> GetAllByUserIdAsync(Guid? id);
    }
}
