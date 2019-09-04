using FactWeb.Model;
using FactWeb.RepositoryContracts;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace FactWeb.BusinessLayer
{
    public class ApplicationVersionCacheManager
    {
        private readonly IApplicationVersionCacheRepository applicationVersionCacheRepository;

        public ApplicationVersionCacheManager(IApplicationVersionCacheRepository applicationVersionCacheRepository)
        {
            this.applicationVersionCacheRepository = applicationVersionCacheRepository;
        }

        public void AddCache(Guid applicationVersionId, List<ApplicationSection> sections)
        {
            var appSections = JsonConvert.SerializeObject(sections, Formatting.None, new JsonSerializerSettings
            {
                PreserveReferencesHandling = PreserveReferencesHandling.Objects,
            });

            this.applicationVersionCacheRepository.AddCache(applicationVersionId, appSections);
        }

        public List<ApplicationSection> GetCache(Guid applicationVersionId)
        {
            var sections = this.applicationVersionCacheRepository.GetCache(applicationVersionId);

            return sections == null ? null : JsonConvert.DeserializeObject<List<ApplicationSection>>(sections, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
        }

        public List<ApplicationVersionCache> GetAll()
        {
            return this.applicationVersionCacheRepository.GetAll();
        }

        public List<ApplicationVersionCache> GetAllActiveVersions()
        {
            return this.applicationVersionCacheRepository.GetAllActiveVersions();
        }

        public List<ApplicationVersionCache> GetAllForLatests()
        {
            return this.applicationVersionCacheRepository.GetAllForLatests();
        }
    }
}
