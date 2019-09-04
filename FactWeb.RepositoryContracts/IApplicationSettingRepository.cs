using FactWeb.Model;
using System.Threading.Tasks;

namespace FactWeb.RepositoryContracts
{
    public interface IApplicationSettingRepository : IRepository<ApplicationSetting>
    {
        /// <summary>
        /// Gets an application setting by its name
        /// </summary>
        /// <param name="name">Name of the application setting</param>
        /// <returns>ApplicationSetting object</returns>
        ApplicationSetting GetByName(string name);

        /// <summary>
        /// Gets an application setting by its name asynchronously
        /// </summary>
        /// <param name="name">Name of the application setting</param>
        /// <returns>ApplicationSetting object</returns>
        Task<ApplicationSetting> GetByNameAsync(string name);
    }
}
