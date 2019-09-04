using FactWeb.Infrastructure;
using FactWeb.Model;
using FactWeb.RepositoryContracts;
using System;
using System.Threading.Tasks;

namespace FactWeb.BusinessLayer
{
    public class CacheStatusManager : BaseManager<CacheStatusManager, ICacheStatusRepository, CacheStatus>
    {
        public CacheStatusManager(ICacheStatusRepository repository) : base(repository)
        {
        }

        public CacheStatus GetByObjectName(string objectName)
        {
            return base.Repository.GetByObjectName(objectName);
        }

        public Task<CacheStatus> GetByObjectNameAsync(string objectName)
        {
            return base.Repository.GetByObjectNameAsync(objectName);
        }

        public void UpdateCacheDate(string objectName, string updatedBy)
        {
            var cacheStatus = this.GetByObjectName(objectName);

            if (cacheStatus == null)
            {
                throw new Exception($"Cannot find object {objectName}");
            }

            cacheStatus.LastChangeDate = DateTime.Now;
            cacheStatus.UpdatedBy = updatedBy;
            cacheStatus.UpdatedDate = DateTime.Now;

            base.Repository.Save(cacheStatus);
        }

        public async Task UpdateCacheDateAsync(string objectName, string updatedBy)
        {
            var cacheStatus = await this.GetByObjectNameAsync(objectName);

            if (cacheStatus == null)
            {
                throw new Exception($"Cannot find object {objectName}");
            }

            cacheStatus.LastChangeDate = DateTime.Now;
            cacheStatus.UpdatedBy = updatedBy;
            cacheStatus.UpdatedDate = DateTime.Now;

            await base.Repository.SaveAsync(cacheStatus);
        }

        public void UpdateOrgFacSiteCacheDate(string updatedBy)
        {
            this.UpdateCacheDate(Constants.CacheStatuses.Organizations, updatedBy);
            this.UpdateCacheDate(Constants.CacheStatuses.Facilities, updatedBy);
            this.UpdateCacheDate(Constants.CacheStatuses.Sites, updatedBy);
        }

        public async Task UpdateOrgFacSiteCacheDateAsync(string updatedBy)
        {
            await this.UpdateCacheDateAsync(Constants.CacheStatuses.Organizations, updatedBy);
            await this.UpdateCacheDateAsync(Constants.CacheStatuses.Facilities, updatedBy);
            await this.UpdateCacheDateAsync(Constants.CacheStatuses.Sites, updatedBy);
        }
    }
}
