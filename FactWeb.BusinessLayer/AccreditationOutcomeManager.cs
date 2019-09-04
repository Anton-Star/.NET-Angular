using FactWeb.Model;
using FactWeb.Model.InterfaceItems;
using FactWeb.RepositoryContracts;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FactWeb.BusinessLayer
{
    public class AccreditationOutcomeManager : BaseManager<AccreditationOutcomeManager, IAccreditationOutcomeRepository, AccreditationOutcome>
    {
        public AccreditationOutcomeManager(IAccreditationOutcomeRepository repository) : base(repository)
        {
        }

        public List<AccreditationOutcome> GetByOrgId(int organizationId)
        {
            LogMessage("GetByOrgId (AccreditationOutcomeManager)");

            return base.Repository.GetByOrgId(organizationId);

        }

        public List<AccreditationOutcome> GetByOrgIdAndAppId(int organizationId, int applicationId)
        {
            LogMessage("GetByOrgIdAndAppId (AccreditationOutcomeManager)");

            return base.Repository.GetByOrgIdAndAppId(organizationId, applicationId);

        }

        public List<AccreditationOutcome> GetByAppId(Guid applicationUniqueId)
        {
            LogMessage("GetByAppId (AccreditationOutcomeManager)");

            return base.Repository.GetByAppId(applicationUniqueId);

        }

        public void Save(AccreditationOutcomeItem accreditationOutcomeItem, string email)
        {
            LogMessage("Save (AccreditationOutcomeManager)");

            AccreditationOutcome accreditationOutcome = new AccreditationOutcome();
            accreditationOutcome.OrganizationId = accreditationOutcomeItem.OrganizationId;
            accreditationOutcome.ApplicationId = accreditationOutcomeItem.ApplicationId;
            accreditationOutcome.OutcomeStatusId = accreditationOutcomeItem.OutcomeStatusId;
            accreditationOutcome.ReportReviewStatusId = accreditationOutcomeItem.ReportReviewStatusId;
            accreditationOutcome.CommitteeDate = Convert.ToDateTime(accreditationOutcomeItem.CommitteeDate);
            accreditationOutcome.SendEmail = accreditationOutcome.SendEmail;

            accreditationOutcome.CreatedBy = email;
            accreditationOutcome.CreatedDate = DateTime.Now;

            base.Repository.Add(accreditationOutcome);
        }

        public Task<bool> DeleteAsync(int id)
        {
            LogMessage("DeleteAsync (AccreditationOutcomeManager)");

            AccreditationOutcome accreditationOutcome = base.Repository.GetById(id);
            base.Repository.RemoveAsync(accreditationOutcome);

            return Task.FromResult(true);
        }
    }
}
