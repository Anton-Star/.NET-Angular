using FactWeb.Model;
using FactWeb.RepositoryContracts;
using System.Threading.Tasks;

namespace FactWeb.BusinessLayer
{
    public class ApplicationStatusManager : BaseManager<ApplicationStatusManager, IApplicationStatusRepository, ApplicationStatus>
    {
        public ApplicationStatusManager(IApplicationStatusRepository repository) : base(repository)
        {
        }

        /// <summary>
        /// Gets an application status by its name
        /// </summary>
        /// <param name="name">Name of the application status</param>
        /// <returns>ApplicationStatus object</returns>
        public ApplicationStatus GetByName(string name)
        {
            LogMessage("GetByName (ApplicationStatusManager)");

            return base.Repository.GetByName(name);
        }

        /// <summary>
        /// Gets an application status by its name asynchronously
        /// </summary>
        /// <param name="name">Name of the application status</param>
        /// <returns>ApplicationStatus object</returns>
        public Task<ApplicationStatus> GetByNameAsync(string name)
        {
            LogMessage("GetByNameAsync (ApplicationStatusManager)");

            return base.Repository.GetByNameAsync(name);
        }

        public Task<bool> SaveStatus(int applicationStatusId, string statusForFACT, string statusForApplicant, string currentUser)
        {
            LogMessage("SaveStatusAsync (ApplicationStatusManager)");

            return base.Repository.UpdateStatus(applicationStatusId, statusForFACT, statusForApplicant, currentUser);
        }
    }
}
