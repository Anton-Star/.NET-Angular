using FactWeb.Infrastructure;
using FactWeb.Model;
using FactWeb.RepositoryContracts;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace FactWeb.Repository
{
    public class AccreditationOutcomeRepository : BaseRepository<AccreditationOutcome>, IAccreditationOutcomeRepository
    {
        public AccreditationOutcomeRepository(FactWebContext context) : base(context)
        {
        }

        public List<AccreditationOutcome> GetByOrgId(int organizationId)
        {
            return base.Context.AccreditationOutcome
                .Include(x => x.Organization)
                .Include(x=>x.Application)
                .Include(x=>x.ReportReviewStatus)
                .Include(x => x.Application.ApplicationType)
                .Where(x=>x.Organization.Id == organizationId)
                .ToList();
        }

        public List<AccreditationOutcome> GetByOrgIdAndAppId(int organizationId, int applicationId)
        {
            return base.Context.AccreditationOutcome
                .Include(x => x.Organization)
                .Include(x => x.Application)
                .Include(x => x.ReportReviewStatus)
                .Include(x => x.Application.ApplicationType)
                .Where(x => x.Organization.Id == organizationId && x.ApplicationId == applicationId)
                .ToList();
        }
        public List<AccreditationOutcome> GetByAppId(Guid applicationUniqueId)
        {
            var compAppId =
                base.Context.ComplianceApplications.SingleOrDefault(
                    x => x.Applications.Any(y => y.UniqueId == applicationUniqueId))?.Id;

            return base.Context.AccreditationOutcome
                .Include(x => x.Organization)
                .Include(x => x.Application)
                .Include(x => x.ReportReviewStatus)
                .Include(x => x.Application.ApplicationType)
                .Where(x => x.Application.UniqueId == applicationUniqueId || x.Application.ComplianceApplicationId == compAppId)
                .ToList();
        }
    }
}

