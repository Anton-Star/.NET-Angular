using FactWeb.Model;
using System.Threading.Tasks;

namespace FactWeb.RepositoryContracts
{
    public interface IApplicationStatusRepository : IRepository<ApplicationStatus>
    {
        /// <summary>
        /// Gets an application status by its name
        /// </summary>
        /// <param name="name">Name of the application status</param>
        /// <returns>ApplicationStatus object</returns>
        ApplicationStatus GetByName(string name);

        /// <summary>
        /// Gets an application status by its name asynchronously
        /// </summary>
        /// <param name="name">Name of the application status</param>
        /// <returns>ApplicationStatus object</returns>
        Task<ApplicationStatus> GetByNameAsync(string name);

        /// <summary>
        /// Updates application status 
        /// </summary>
        /// <returns>bool</returns>
        Task<bool> UpdateStatus(int applicationStatusId, string statusForFACT, string statusForApplicant, string currentUser);
    }
}
