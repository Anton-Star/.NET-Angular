using FactWeb.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FactWeb.RepositoryContracts
{
    public interface IUserOrganizationRepository : IRepository<UserOrganization>
    {
        List<UserOrganization> GetAllForUser(Guid userId);
        Task<List<UserOrganization>> GetAllForUserAsync(Guid userId);
        Task<UserOrganization> GetByOrgAndJobFunction(int organizationId, Guid jobFunctionId);
    }
}
