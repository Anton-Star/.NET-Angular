using FactWeb.Model;
using FactWeb.RepositoryContracts;
using System.Threading.Tasks;

namespace FactWeb.BusinessLayer
{
    public class ApplicationTypeManager : BaseManager<ApplicationTypeManager, IApplicationTypeRepository, ApplicationType>
    {
        public ApplicationTypeManager(IApplicationTypeRepository repository) : base(repository)
        {
        }

        /// <summary>
        /// Gets an Application Type by name
        /// </summary>
        /// <param name="name">Name of the application type</param>
        /// <returns>Application Type object</returns>
        public ApplicationType GetByName(string name)
        {
            LogMessage("GetByName (ApplicationTypeManager)");

            return this.Repository.GetByName(name);
        }

        /// <summary>
        /// Gets an Application Type by name async
        /// </summary>
        /// <param name="name">Name of the application type</param>
        /// <returns>Application Type object</returns>
        public Task<ApplicationType> GetByNameAsync(string name)
        {
            LogMessage("GetByNameAsync (ApplicationTypeManager)");

            return this.Repository.GetByNameAsync(name);
        }

    }
}
