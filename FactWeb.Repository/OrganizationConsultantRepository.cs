using FactWeb.Model;
using FactWeb.RepositoryContracts;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace FactWeb.Repository
{
    public class OrganizationConsultantRepository : BaseRepository<OrganizationConsutant>, IOrganizationConsultantRepository
    {
        public OrganizationConsultantRepository(FactWebContext context) : base(context)
        {
        }

        public Task<List<OrganizationConsutant>> GetByOrgIdAsync(int organizationId)
        {
            var result = base.FetchMany(x => x.OrganizationId == organizationId);

            return Task.FromResult(result);
        }


        public bool IsDuplicateConsultant(int organizationConsultantId, int organizationId, string consultantId)
        {
            var result = base.FetchMany(x => x.OrganizationId == organizationId && x.ConsultantId == new Guid(consultantId));

            if (organizationConsultantId == 0)
            {
                return result.Count > 0 ? true : false;
            }
            else if (result.Count > 0)
            {
                return result[0].Id == organizationConsultantId ? false : true;
            }

            return false;
        }

        public Task<bool> IsDuplicateConsultantAsync(int organizationConsultantId, int organizationId, string consultantId)
        {
            var result = base.FetchMany(x => x.OrganizationId == organizationId && x.ConsultantId == new Guid(consultantId));

            if (organizationConsultantId == 0)
            {
                return Task.FromResult(result.Count > 0 ? true : false);
            }
            else if (result.Count > 0)
            {
                return Task.FromResult(result[0].Id == organizationConsultantId ? false : true);
            }

            return Task.FromResult(false);
        }


    }
}
