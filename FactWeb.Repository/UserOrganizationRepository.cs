using FactWeb.Model;
using FactWeb.RepositoryContracts;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FactWeb.Repository
{
    public class UserOrganizationRepository : BaseRepository<UserOrganization>, IUserOrganizationRepository
    {
        public UserOrganizationRepository(FactWebContext context) : base(context)
        {
        }

        public List<UserOrganization> GetAllForUser(Guid userId)
        {
            return base.FetchMany(x => x.UserId == userId);
        }

        public Task<List<UserOrganization>> GetAllForUserAsync(Guid userId)
        {
            return base.FetchManyAsync(x => x.UserId == userId);
        }

        public Task<UserOrganization> GetByOrgAndJobFunction(int organizationId, Guid jobFunctionId)
        {
            return base.FetchAsync(x => x.OrganizationId == organizationId && x.JobFunctionId == jobFunctionId);
        }
    }
}
