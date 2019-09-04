using FactWeb.BusinessLayer;
using FactWeb.Model;
using SimpleInjector;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FactWeb.BusinessFacade
{
    public class ApplicationStatusFacade
    {
        private readonly Container container;

        public ApplicationStatusFacade(Container container)
        {
            this.container = container;
        }

        public async Task<List<ApplicationStatus>> GetApplicationStatusAsync()
        {
            var applicationStatusManager = this.container.GetInstance<ApplicationStatusManager>();

            return await applicationStatusManager.GetAllAsync();
        }

        public Task<bool> SaveStatus(int applicationStatusId, string statusForFACT, string statusForApplicant, string currentUser)
        {
            var applicationStatusManager = this.container.GetInstance<ApplicationStatusManager>();
            return applicationStatusManager.SaveStatus(applicationStatusId, statusForFACT, statusForApplicant, currentUser);
        }
    }
}
