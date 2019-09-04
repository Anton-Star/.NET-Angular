using FactWeb.Model;
using FactWeb.RepositoryContracts;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;

namespace FactWeb.Repository
{
    public class ApplicationVersionCacheRepository : BaseRepository<ApplicationVersionCache>, IApplicationVersionCacheRepository
    {
        public ApplicationVersionCacheRepository(FactWebContext context) : base(context)
        {
        }

        public void AddCache(Guid applicationVersionId, string sections)
        {
            var record = new ApplicationVersionCache
            {
                ApplicationVersionId = applicationVersionId,
                ApplicationVersionCacheSections = sections,
                CreatedDate = DateTime.Now,
                CreatedBy = "System"
            };

            base.Add(record);
        }

        public string GetCache(Guid applicationVersionId)
        {
            var record = base.Fetch(x => x.ApplicationVersionId == applicationVersionId);

            return record?.ApplicationVersionCacheSections;
        }

        public List<ApplicationVersionCache> GetAllActiveVersions()
        {
            var objectContext = ((IObjectContextAdapter)this.Context).ObjectContext;

            var paramList = new Object[0];

            var rows = objectContext.ExecuteStoreQuery<ApplicationVersionCache>(
                "EXEC usp_getAllActiveApplicationVersionCache", paramList).ToList();

            return rows;
        }

        public List<ApplicationVersionCache> GetAllForLatests()
        {
            var objectContext = ((IObjectContextAdapter)this.Context).ObjectContext;

            var paramList = new Object[0];

            var rows = objectContext.ExecuteStoreQuery<ApplicationVersionCache>(
                "EXEC usp_getApplicationVersionCacheForLatests", paramList).ToList();

            return rows;
        }
    }
}
