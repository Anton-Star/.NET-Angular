using FactWeb.Model;
using FactWeb.RepositoryContracts;
using System;
using System.Threading.Tasks;

namespace FactWeb.Repository
{
    public class ApplicationResponseStatusRepository : BaseRepository<ApplicationResponseStatus>, IApplicationResponseStatusRepository
    {
        public ApplicationResponseStatusRepository(FactWebContext context) : base(context)
        {
        }

        public Task<bool> UpdateStatus(int applicationResponseStatusId, string statusForFACT, string statusForApplicant, string currentUser)
        {
            var applicationResponseStatus = base.GetById(applicationResponseStatusId);
            applicationResponseStatus.Name = statusForFACT;
            applicationResponseStatus.NameForApplicant = statusForApplicant;
            applicationResponseStatus.UpdatedBy = currentUser;
            applicationResponseStatus.UpdatedDate = DateTime.Now;

            base.Save(applicationResponseStatus);
            return Task.FromResult(true);
        }

        public ApplicationResponseStatus GetStatusByName(string statusName)
        {
            return base.Fetch(x => x.Name == statusName);
        }

        public Task<ApplicationResponseStatus> GetStatusByNameAsync(string statusName)
        {
            return base.FetchAsync(x => x.Name == statusName);
        }

    }
}
