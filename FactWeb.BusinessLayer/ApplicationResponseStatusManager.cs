using FactWeb.Model;
using FactWeb.RepositoryContracts;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FactWeb.BusinessLayer
{
    public class ApplicationResponseStatusManager : BaseManager<ApplicationResponseStatusManager, IApplicationResponseStatusRepository, ApplicationResponseStatus>
    {
        public ApplicationResponseStatusManager(IApplicationResponseStatusRepository repository) : base(repository)
        {
        }

        /// <summary>
        /// Get all response statuses from database asynchrously 
        /// </summary>
        /// <returns></returns>
        public async Task<List<ApplicationResponseStatus>> GetAllAsync()
        {
            LogMessage("GetAllAsync (ApplicationResponseStatusManager)");

            return await base.Repository.GetAllAsync();
        }

        /// <summary>
        /// Get all response statuses from database 
        /// </summary>
        /// <returns></returns>
        public List<ApplicationResponseStatus> GetAll()
        {
            LogMessage("GetAll (ApplicationResponseStatusManager)");

            return base.Repository.GetAll();
        }

        /// <summary>
        /// Saves Application Response Status in database
        /// </summary>
        /// <param name="applicationResponseStatusId"></param>
        /// <param name="statusForFACT"></param>
        /// <param name="statusForApplicant"></param>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        public async Task<bool> SaveStatus(int applicationResponseStatusId, string statusForFACT, string statusForApplicant, string currentUser)
        {
            LogMessage("SaveStatusAsync (ApplicationResponseStatusManager)");

            return await base.Repository.UpdateStatus(applicationResponseStatusId, statusForFACT, statusForApplicant, currentUser);
        }

        /// <summary>
        /// Get status by name asynchoronously
        /// </summary>
        /// <param name="statusName"></param>
        /// <returns></returns>
        public ApplicationResponseStatus GetStatusByName(string statusName)
        {
            LogMessage("GetAll (GetStatusByName)");

            return base.Repository.GetStatusByName(statusName);
        }

        /// <summary>
        /// Get status by name asynchronously
        /// </summary>
        /// <param name="statusName"></param>
        /// <returns></returns>
        public async Task<ApplicationResponseStatus> GetStatusByNameAsync(string statusName)
        {
            LogMessage("GetAll (GetStatusByNameAsync)");

            return await base.Repository.GetStatusByNameAsync(statusName);
        }
    }
}

