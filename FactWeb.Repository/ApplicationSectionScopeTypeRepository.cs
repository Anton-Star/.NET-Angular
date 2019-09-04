using FactWeb.Model;
using FactWeb.RepositoryContracts;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FactWeb.Repository
{
    public class ApplicationSectionScopeTypeRepository : BaseRepository<ApplicationSectionScopeType>, IApplicationSectionScopeTypeRepository
    {
        public ApplicationSectionScopeTypeRepository(FactWebContext context) : base(context)
        {
        }

        public List<ApplicationSectionScopeType> GetAllByApplicationSectionId(Guid? id)
        {
            Guid? applicationSectionId = id.GetValueOrDefault();
            return base.FetchMany(x => x.ApplicationSectionId == applicationSectionId);
        }

        public Task<List<ApplicationSectionScopeType>> GetAllByApplicationSectionIdAsync(Guid? id)
        {
            Guid? applicationSectionId = id.GetValueOrDefault();
            return base.FetchManyAsync(x => x.ApplicationSectionId == applicationSectionId);
        }
    }
}
