using FactWeb.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FactWeb.RepositoryContracts
{
    public interface IOrganizationAccreditationCycleRepository : IRepository<OrganizationAccreditationCycle>
    {
        List<OrganizationAccreditationCycle> GetByOrganization(int organizationId);
        Task<List<OrganizationAccreditationCycle>> GetByOrganizationAsync(int organizationId);

        OrganizationAccreditationCycle GetCurrentByOrganization(int organizationId);
        Task<OrganizationAccreditationCycle> GetCurrentByOrganizationAsync(int organizationId);
    }
}
