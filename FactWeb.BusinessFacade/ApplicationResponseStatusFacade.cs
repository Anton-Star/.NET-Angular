using FactWeb.BusinessLayer;
using FactWeb.Model;
using SimpleInjector;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FactWeb.BusinessFacade
{
    public class ApplicationResponseStatusFacade
    {
        private readonly Container container;

        public ApplicationResponseStatusFacade(Container container)
        {
            this.container = container;
        }

        public async Task<List<ApplicationResponseStatus>> GetApplicationResponseStatusAsync()
        {
            var applicationResponseStatusManager = this.container.GetInstance<ApplicationResponseStatusManager>();

            return await applicationResponseStatusManager.GetAllAsync();
        }

        public Task<bool> SaveStatus(int applicationResponseStatusId, string statusForFACT, string statusForApplicant, string currentUser)
        {
            var applicationResponseStatusManager = this.container.GetInstance<ApplicationResponseStatusManager>();
            return applicationResponseStatusManager.SaveStatus(applicationResponseStatusId, statusForFACT, statusForApplicant, currentUser);
        }
    }
}
