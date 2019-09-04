using FactWeb.Model;
using FactWeb.RepositoryContracts;
using System;
using System.Threading.Tasks;

namespace FactWeb.BusinessLayer
{
    public class ApplicationSettingManager : BaseManager<ApplicationSettingManager, IApplicationSettingRepository, ApplicationSetting>
    {
        public ApplicationSettingManager(IApplicationSettingRepository repository) : base(repository)
        {
        }

        /// <summary>
        /// Gets an application setting by its name
        /// </summary>
        /// <param name="name">Name of the application setting</param>
        /// <returns>ApplicationSetting object</returns>
        public ApplicationSetting GetByName(string name)
        {
            LogMessage("GetByName (ApplicationSettingManager)");

            return base.Repository.GetByName(name);
        }

        /// <summary>
        /// Gets an application setting by its name asynchronously
        /// </summary>
        /// <param name="name">Name of the application setting</param>
        /// <returns>ApplicationSetting object</returns>
        public Task<ApplicationSetting> GetByNameAsync(string name)
        {
            LogMessage("GetByNameAsync (ApplicationSettingManager)");

            return base.Repository.GetByNameAsync(name);
        }

        /// <summary>
        /// Sets a setting to the given value
        /// </summary>
        /// <param name="name">Name of the application setting</param>
        /// <param name="value">New value of the application setting</param>
        /// <param name="updatedBy">Who is doing the update</param>
        public void SetValue(string name, string value, string updatedBy)
        {
            LogMessage("SetValue (ApplicationSettingManager)");

            var applicationSetting = this.GetByName(name);

            if (applicationSetting == null) return;

            applicationSetting.Value = value;
            applicationSetting.UpdatedBy = updatedBy;
            applicationSetting.UpdatedDate = DateTime.Now;

            base.Repository.Save(applicationSetting);
        }

        /// <summary>
        /// Sets a setting to the given value
        /// </summary>
        /// <param name="name">Name of the application setting</param>
        /// <param name="value">New value of the application setting</param>
        /// <param name="updatedBy">Who is doing the update</param>
        public async Task SetValueAsync(string name, string value, string updatedBy)
        {
            LogMessage("SetValueAsync (ApplicationSettingManager)");

            var applicationSetting = await this.GetByNameAsync(name);

            if (applicationSetting == null) return;

            applicationSetting.Value = value;
            applicationSetting.UpdatedBy = updatedBy;
            applicationSetting.UpdatedDate = DateTime.Now;

            await base.Repository.SaveAsync(applicationSetting);
        }
    }
}
