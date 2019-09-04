using FactWeb.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FactWeb.RepositoryContracts
{
    public interface IApplicationResponseStatusRepository : IRepository<ApplicationResponseStatus>
    {
        /// <summary>
        /// Updates application response status 
        /// </summary>
        /// <returns>bool</returns>
        Task<bool> UpdateStatus(int applicationResponseStatusId, string statusForFACT, string statusForApplicant, string currentUser);

        /// <summary>
        /// Get status by name
        /// </summary>
        /// <param name="statusName"></param>
        /// <returns></returns>
        ApplicationResponseStatus GetStatusByName(string statusName);

        /// <summary>
        /// Get status by name asynchoronously
        /// </summary>
        /// <param name="statusName"></param>
        /// <returns></returns>
        Task<ApplicationResponseStatus> GetStatusByNameAsync(string statusName);
    }
}
