using FactWeb.Model;
using FactWeb.RepositoryContracts;
using System;
using System.Threading.Tasks;

namespace FactWeb.Repository
{
    public class ApplicationStatusRepository : BaseRepository<ApplicationStatus>, IApplicationStatusRepository
    {
        public ApplicationStatusRepository(FactWebContext context) : base(context)
        {
        }

        public ApplicationStatus GetByName(string name)
        {
            return base.Fetch(x => x.Name == name);
        }

        public Task<ApplicationStatus> GetByNameAsync(string name)
        {
            return base.FetchAsync(x => x.Name == name);
        }

        public Task<bool> UpdateStatus(int applicationStatusId, string statusForFACT, string statusForApplicant, string currentUser)
        {
            var applicationStatus = base.GetById(applicationStatusId);
            applicationStatus.Name = statusForFACT;
            applicationStatus.NameForApplicant = statusForApplicant;
            applicationStatus.UpdatedBy = currentUser;
            applicationStatus.UpdatedDate = DateTime.Now;

            base.Save(applicationStatus);
            return Task.FromResult(true);
        }
    }
}
