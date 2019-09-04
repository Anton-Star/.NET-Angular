using FactWeb.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FactWeb.RepositoryContracts
{
    public interface IApplicationSectionScopeTypeRepository : IRepository<ApplicationSectionScopeType>
    {
        List<ApplicationSectionScopeType> GetAllByApplicationSectionId(Guid? id);

        Task<List<ApplicationSectionScopeType>> GetAllByApplicationSectionIdAsync(Guid? id);

    }
}
