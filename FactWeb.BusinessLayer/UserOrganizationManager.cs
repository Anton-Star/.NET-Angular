using FactWeb.Model;
using FactWeb.RepositoryContracts;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FactWeb.BusinessLayer
{
    public class UserOrganizationManager : BaseManager<UserOrganizationManager, IUserOrganizationRepository, UserOrganization>
    {
        public UserOrganizationManager(IUserOrganizationRepository repository) : base(repository)
        {
        }

        public List<UserOrganization> GetAllForUser(Guid userId)
        {
            return base.Repository.GetAllForUser(userId);
        }

        public Task<List<UserOrganization>> GetAllForUserAsync(Guid userId)
        {
            return base.Repository.GetAllForUserAsync(userId);
        }

        public Task<UserOrganization> GetByOrgAndJobFunction(int organizationId, Guid jobFunctionId)
        {
            return base.Repository.GetByOrgAndJobFunction(organizationId, jobFunctionId);
        }

        public void AddRelation(int organizationId, Guid? userId, Guid jobFunctionId, bool showOnAccReport, string createdBy)
        {
            LogMessage("AddRelation (UserOrganizationManager)");

            UserOrganization userOrganization = new UserOrganization();
            userOrganization.Id = Guid.NewGuid();
            userOrganization.UserId = userId.GetValueOrDefault();
            userOrganization.JobFunctionId = jobFunctionId;
            userOrganization.OrganizationId = organizationId;
            userOrganization.ShowOnAccReport = showOnAccReport;
            userOrganization.CreatedBy = createdBy;
            userOrganization.CreatedDate = DateTime.Now;

            base.Add(userOrganization);
        }

        public void RemoveRelation(Guid userOrganizationId)
        {
            LogMessage("RemoveRelation (UserOrganizationManager)");

            base.Remove(userOrganizationId);
        }

        public void RemoveUserOrganizations(Guid userId)
        {
            var orgs = this.GetAllForUser(userId);

            foreach (var org in orgs)
            {
                base.Repository.BatchRemove(org);
            }

            base.Repository.SaveChanges();
        }

        public async Task RemoveUserOrganizationsAsync(Guid userId)
        {
            var orgs = await this.GetAllForUserAsync(userId);

            foreach (var org in orgs)
            {
                base.Repository.BatchRemove(org);
            }

            await base.Repository.SaveChangesAsync();
        }
    }
}
