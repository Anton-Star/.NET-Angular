using FactWeb.Model;
using System;
using System.Collections.Generic;

namespace FactWeb.RepositoryContracts
{
    public interface IApplicationVersionCacheRepository : IRepository<ApplicationVersionCache>
    {
        string GetCache(Guid applicationVersionId);
        void AddCache(Guid applicationVersionId, string sections);

        List<ApplicationVersionCache> GetAllActiveVersions();

        List<ApplicationVersionCache> GetAllForLatests();
    }
}
