using FactWeb.Model;
using System.Threading.Tasks;

namespace FactWeb.RepositoryContracts
{
    public interface IRoleRepository : IRepository<Role>
    {
        /// <summary>
        /// Gets a role by name
        /// </summary>
        /// <param name="name">Name of the Role</param>
        /// <returns>Role entity object</returns>
        Role Get(string name);

        /// <summary>
        /// Gets a role by name asynchronously
        /// </summary>
        /// <param name="name">Name of the Role</param>
        /// <returns>Role entity object</returns>
        Task<Role> GetAsync(string name);
    }
}
