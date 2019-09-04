using FactWeb.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FactWeb.RepositoryContracts
{
    public interface IOrganizationConsultantRepository : IRepository<OrganizationConsutant>
    {
        Task<List<OrganizationConsutant>> GetByOrgIdAsync(int organizationId);

        bool IsDuplicateConsultant(int organizationConsultantId, int organizationId, string consultantId);
        Task<bool> IsDuplicateConsultantAsync(int organizationConsultantId, int organizationId, string consultantId);

    }
}
