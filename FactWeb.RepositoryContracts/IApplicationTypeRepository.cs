using FactWeb.Model;
using System.Threading.Tasks;

namespace FactWeb.RepositoryContracts
{
    public interface IApplicationTypeRepository : IRepository<ApplicationType>
    {
        /// <summary>
        /// Gets an Application Type by name
        /// </summary>
        /// <param name="name">Name of the application type</param>
        /// <returns>Application Type object</returns>
        ApplicationType GetByName(string name);
        /// <summary>
        /// Gets an Application Type by name async
        /// </summary>
        /// <param name="name">Name of the application type</param>
        /// <returns>Application Type object</returns>
        Task<ApplicationType> GetByNameAsync(string name);
    }
}
