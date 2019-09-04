using FactWeb.BusinessLayer;
using FactWeb.Infrastructure;
using FactWeb.Model;
using SimpleInjector;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FactWeb.BusinessFacade
{
    public class ApplicationSettingFacade
    {
        private readonly Container container;

        public ApplicationSettingFacade(Container container)
        {
            this.container = container;
        }

        /// <summary>
        /// Gets all application settings
        /// </summary>
        /// <returns>Collection of ApplicationSetting objects</returns>
        public List<ApplicationSetting> GetAll()
        {
            var manager = this.container.GetInstance<ApplicationSettingManager>();

            return manager.GetAll();
        }

        /// <summary>
        /// Gets all application settings asynchronously
        /// </summary>
        /// <returns>Collection of ApplicationSetting objects</returns>
        public Task<List<ApplicationSetting>> GetAllAsync()
        {
            var manager = this.container.GetInstance<ApplicationSettingManager>();

            return manager.GetAllAsync();
        }

        /// <summary>
        /// Gets an application setting by its name
        /// </summary>
        /// <param name="name">Name of the application setting</param>
        /// <returns>ApplicationSetting object</returns>
        public ApplicationSetting GetByName(string name)
        {
            var manager = this.container.GetInstance<ApplicationSettingManager>();

            return manager.GetByName(name);
        }

        /// <summary>
        /// Gets an application setting by its name asynchronously
        /// </summary>
        /// <param name="name">Name of the application setting</param>
        /// <returns>ApplicationSetting object</returns>
        public Task<ApplicationSetting> GetByNameAsync(string name)
        {
            var manager = this.container.GetInstance<ApplicationSettingManager>();

            return manager.GetByNameAsync(name);
        }

        /// <summary>
        /// Sets a setting to the given value
        /// </summary>
        /// <param name="name">Name of the application setting</param>
        /// <param name="value">New value of the application setting</param>
        /// <param name="updatedBy">Who is doing the update</param>
        public void SetValue(string name, string value, string updatedBy)
        {
            var manager = this.container.GetInstance<ApplicationSettingManager>();

            manager.SetValue(name, value, updatedBy);

            this.InvalidateCache(updatedBy);
        }

        /// <summary>
        /// Sets a setting to the given value
        /// </summary>
        /// <param name="name">Name of the application setting</param>
        /// <param name="value">New value of the application setting</param>
        /// <param name="updatedBy">Who is doing the update</param>
        public async Task SetValueAsync(string name, string value, string updatedBy)
        {
            var manager = this.container.GetInstance<ApplicationSettingManager>();

            await manager.SetValueAsync(name, value, updatedBy);

            await this.InvalidateCacheAsync(updatedBy);
        }

        /// <summary>
        /// Sets a setting to the given value
        /// </summary>
        /// <param name="settings">Settings to be saved</param>
        /// <param name="updatedBy">Who is doing the update</param>
        public void SetValues(IEnumerable<ApplicationSetting> settings, string updatedBy)
        {
            foreach (var setting in settings)
            {
                this.SetValue(setting.Name, setting.Value, updatedBy);
            }

            this.InvalidateCache(updatedBy);
        }

        /// <summary>
        /// Sets a setting to the given value
        /// </summary>
        /// <param name="settings">Settings to be saved</param>
        /// <param name="updatedBy">Who is doing the update</param>
        public async Task SetValuesAsync(IEnumerable<ApplicationSetting> settings, string updatedBy)
        {
            foreach (var setting in settings)
            {
                await this.SetValueAsync(setting.Name, setting.Value, updatedBy);
            }

            await this.InvalidateCacheAsync(updatedBy);
        }

        public async Task InvalidateCacheAsync(string updatedBy)
        {
            var cacheStatusManager = this.container.GetInstance<CacheStatusManager>();
            await cacheStatusManager.UpdateCacheDateAsync(Constants.CacheStatuses.AppSettings, updatedBy);
        }

        public void InvalidateCache(string updatedBy)
        {
            var cacheStatusManager = this.container.GetInstance<CacheStatusManager>();
            cacheStatusManager.UpdateCacheDate(Constants.CacheStatuses.AppSettings, updatedBy);
        }
    }
}
