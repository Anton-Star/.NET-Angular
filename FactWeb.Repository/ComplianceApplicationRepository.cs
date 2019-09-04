using FactWeb.Infrastructure;
using FactWeb.Model;
using FactWeb.Model.InterfaceItems;
using FactWeb.RepositoryContracts;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading.Tasks;

namespace FactWeb.Repository
{
    public class ComplianceApplicationRepository : BaseRepository<ComplianceApplication>, IComplianceApplicationRepository
    {
        public ComplianceApplicationRepository(FactWebContext context) : base(context)
        {
        }

        public List<ComplianceApplication> GetByOrg(int orgId)
        {
            return base.Context.ComplianceApplications
                .Include(x => x.Organization)
                .Include(x => x.ReportReviewStatus)
                .Include(x => x.Coordinator)
                .Include(x => x.ApplicationStatus)
                .Where(x => x.OrganizationId == orgId && x.IsActive)
                .ToList();
        }

        public Task<List<ComplianceApplication>> GetByOrgAsync(int orgId)
        {
            return base.FetchManyAsync(x => x.OrganizationId == orgId && x.IsActive);
        }

        public ComplianceApplication GetByIdInclusive(Guid id)
        {
            base.Context.Configuration.LazyLoadingEnabled = false;

            return base.Context.ComplianceApplications
                .Include(x=>x.Organization)
                .Include(x=>x.ReportReviewStatus)
                .Include(x=>x.Coordinator)
                .Include(x=>x.ApplicationStatus)
                .Include(x => x.Applications.Select(y=>y.AccreditationOutcomes.Select(z => z.OutcomeStatus)))
                .Include(x => x.Applications.Select(y=>y.Coordinator))
                .Include(x => x.Applications.Select(y=>y.ApplicationResponses))
                .Include(x => x.Applications.Select(y => y.ApplicationResponses.Select(z=>z.ApplicationSectionQuestion)))
                .Include(x => x.Applications.Select(y => y.ApplicationResponses.Select(z => z.ApplicationResponseStatus)))
                .Include(x => x.Applications.Select(y => y.ApplicationResponsesTrainee))
                .Include(x => x.Applications.Select(y => y.ApplicationStatus))
                .Include(x => x.Applications.Select(y => y.ApplicationType))
                .Include(x => x.Applications.Select(y => y.ApplicationVersion))
                .Include(x => x.Applications.Select(y => y.ComplianceApplication))
                .Include(x => x.Applications.Select(y => y.InspectionSchedules.Select(z => z.InspectionScheduleDetails)))
                .Include(x => x.Applications.Select(y => y.Inspections))
                .Include(x => x.Applications.Select(y => y.Organization))
                .Include(x => x.Applications.Select(y => y.Organization))
                .Include(x=>x.Applications.Select(y=>y.ApplicationQuestionNotApplicables))
                .Include(x => x.Applications.Select(y => y.Organization.OrganizationFacilities.Select(z => z.Facility.FacilitySites.Select(zz => zz.Site.InspectionScheduleSites))))
                .Include(x => x.Applications.Select(y => y.Organization.OrganizationFacilities.Select(z => z.Facility.FacilitySites.Select(zz => zz.Site.SiteTransplantTypes))))
                .Include(x => x.Applications.Select(y => y.Organization.OrganizationConsutants))
                .Include(x => x.Applications.Select(y => y.Site))
                .SingleOrDefault(x => x.Id == id);
        }

        public ComplianceApplication GetByIdSemiInclusive(Guid id)
        {
            base.Context.Configuration.LazyLoadingEnabled = false;

            return base.Context.ComplianceApplications
                .Include(x => x.Organization)
                .Include(x => x.ReportReviewStatus)
                .Include(x => x.Coordinator)
                .Include(x => x.ApplicationStatus)
                .Include(x => x.Applications.Select(y => y.Coordinator))
                .Include(x => x.Applications.Select(y => y.ApplicationStatus))
                .Include(x => x.Applications.Select(y => y.ApplicationType))
                .Include(x => x.Applications.Select(y => y.ApplicationVersion))
                .Include(x => x.Applications.Select(y => y.InspectionSchedules.Select(z => z.InspectionScheduleDetails)))
                .Include(x => x.Applications.Select(y => y.Inspections))
                .Include(x => x.Applications.Select(y => y.Organization.OrganizationConsutants))
                .Include(x => x.Applications.Select(y => y.Site))
                .SingleOrDefault(x => x.Id == id);
        }

        public ComplianceApplication GetByIdWithApps(Guid id)
        {
            base.Context.Configuration.LazyLoadingEnabled = false;

            return base.Context.ComplianceApplications
                .Include(x => x.Organization)
                .Include(x => x.Coordinator)
                .Include(x => x.ApplicationStatus)
                .Include(x => x.Applications)
                .SingleOrDefault(x => x.Id == id);
        }

        public List<CbTotal> GetCbTotals(Guid complianceApplicationId)
        {
            var objectContext = ((IObjectContextAdapter)Context).ObjectContext;

            var paramList = new Object[1];

            paramList[0] = complianceApplicationId;

            var data = objectContext.ExecuteStoreQuery<CbTotal>(
                "EXEC usp_CbTotals @complianceApplicationId={0}", paramList).ToList();

            return data;
        }

        public List<CtTotal> GetCtTotals(Guid complianceApplicationId)
        {
            var objectContext = ((IObjectContextAdapter)Context).ObjectContext;

            var paramList = new Object[1];

            paramList[0] = complianceApplicationId;

            var data = objectContext.ExecuteStoreQuery<CtTotal>(
                "EXEC usp_ctTotals @complianceApplicationId={0}", paramList).ToList();

            return data;
        }

        public DoesInspectorHaveAccessModel DoesInspectorHaveAccess(Guid? applicationUniqueId, Guid? complianceApplicationId, Guid userId)
        {
            var objectContext = ((IObjectContextAdapter)Context).ObjectContext;

            var paramList = new Object[3];

            paramList[0] = applicationUniqueId;
            paramList[1] = complianceApplicationId;
            paramList[2] = userId;

            var data = objectContext.ExecuteStoreQuery<DoesInspectorHaveAccessModel>(
                "EXEC usp_getInspectorHasAccess @applicationUniqueId={0}, @complianceApplicationId={1}, @userId={2}", paramList).FirstOrDefault();

            return data;
        }

        public void UpdateComplianceApplicationStatus(Guid complianceApplicationId, string statusName, string updatedBy)
        {
            var objectContext = ((IObjectContextAdapter)Context).ObjectContext;

            var paramList = new Object[3];

            paramList[0] = complianceApplicationId;
            paramList[1] = statusName;
            paramList[2] = updatedBy;

            objectContext.ExecuteStoreCommand("usp_updateComplianceApplicationStatus @complianceApplicationId={0}, @statusName={1}, @updatedBy={2}", paramList);
        }

        public void CreateComplianceApplicationSections(Guid complianceApplicationId)
        {
            var objectContext = ((IObjectContextAdapter)Context).ObjectContext;

            var paramList = new object[1];

            paramList[0] = complianceApplicationId;

            objectContext.ExecuteStoreCommand("usp_CreateComplianceApplicationSections @complianceApplicationId={0}", paramList);
        }

        public bool AppHasRfis(Guid complianceApplicationId)
        {
            return
                base.Context.ComplianceApplications.Any(
                    x =>
                        x.Id == complianceApplicationId &&
                        x.Applications.Any(
                            y =>
                                y.ApplicationResponses.Any(
                                    z => z.ApplicationResponseStatusId == (int)Constants.ApplicationResponseStatuses.Rfi)));
        }
    }
}
