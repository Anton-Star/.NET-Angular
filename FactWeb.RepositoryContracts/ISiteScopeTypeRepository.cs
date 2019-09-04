using FactWeb.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FactWeb.RepositoryContracts
{
    public interface ISiteScopeTypeRepository : IRepository<SiteScopeType>
    {
        List<SiteScopeType> GetAllBySiteId(int? id);

        Task<List<SiteScopeType>> GetAllBySiteIdAsync(int? id);

    }
}