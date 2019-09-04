using FactWeb.BusinessLayer;
using FactWeb.Infrastructure;
using FactWeb.Model.InterfaceItems;
using SimpleInjector;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Linq;
using System.Threading.Tasks;

namespace FactWeb.BusinessFacade
{
    public class CoordinatorFacade
    {
        private readonly Container container;

        public CoordinatorFacade(Container container)
        {
            this.container = container;
        }
        
        public async Task<CoordinatorViewItem> GetCoordinatorView(Guid applicationUniqueId)
        {
            var applicationFacade = this.container.GetInstance<ApplicationFacade>();
            var applicationTypeManager = this.container.GetInstance<ApplicationTypeManager>();
            var userManager = this.container.GetInstance<UserManager>();
            var organizationManager = this.container.GetInstance<OrganizationManager>();

            var inspectionScheduleManager = this.container.GetInstance<InspectionScheduleManager>();
            var coordinatorViewItem = new CoordinatorViewItem();

            var complianceApplication = applicationFacade.GetComplianceApplicationByAppUniqueId(applicationUniqueId);

            if (complianceApplication == null)
            {
                return null;
            }

           
            var applicationType = applicationTypeManager.GetByName(Constants.ApplicationTypes.Common);

            if (applicationType == null)
            {
                throw new ObjectNotFoundException(string.Format("Cannot find application type {0}", Constants.ApplicationTypes.Common));
            }

            //var application = applicationManager.GetComplianceApplicationById(appliactionUniqueId).Result;
            
            var inspectionScheduleList = inspectionScheduleManager.GetInspectionScheduleByOrganizationID(complianceApplication.OrganizationId);
            //var applicationSectionList = applicationFacade.GetApplicationSectionItems(complianceApplication.OrganizationId, applicationType.Id, null); //

            coordinatorViewItem.ComplianceApplication = complianceApplication;
            coordinatorViewItem.Personnel = userManager.GetPersonnel(complianceApplication.OrganizationId);

            coordinatorViewItem.InspectionTeamMembers = ModelConversions.ConvertToInspectionTeamMember(inspectionScheduleList).Distinct().ToList();
            coordinatorViewItem.Overview = ModelConversions.ConvertToOverviewItem(complianceApplication, inspectionScheduleList);
            coordinatorViewItem.Overview.AccreditedSince = complianceApplication.AccreditedSince;
            
            return await Task.FromResult(coordinatorViewItem);

        }

        public void SavePersonnel(int orgId, List<Personnel> personnel, string changedBy)
        {
            var userManager = this.container.GetInstance<UserManager>();

            foreach (var p in personnel)
            {
                userManager.UpdatePersonnelByCoordinator(orgId, p.UserId, p.ShowOnAccReport, p.OverrideJobFunction, changedBy);
            }
        }

        public void SaveCoordinatorViewChanges(Guid complianceAppId, string accreditationGoal,
            string inspectionScope, DateTime? accreditedSinceDate, string typeDetail, string changedBy)
        {
            var complianceApplicationManager = this.container.GetInstance<ComplianceApplicationManager>();

            var complianceApp = complianceApplicationManager.GetById(complianceAppId);

            if (complianceApp == null)
            {
                throw new Exception("Cannot find Compliance Application");
            }

            complianceApp.AccreditationGoal = accreditationGoal;
            complianceApp.InspectionScope = inspectionScope;
            complianceApp.Organization.AccreditedSince = accreditedSinceDate;
            complianceApp.UpdatedDate = DateTime.Now;
            complianceApp.UpdatedBy = changedBy;

            if (!string.IsNullOrEmpty(typeDetail))
            {
                var app = complianceApp.Applications.FirstOrDefault(x=>x.TypeDetail != null) ??
                          complianceApp.Applications.First();

                app.TypeDetail = typeDetail;
                app.UpdatedBy = changedBy;
                app.UpdatedDate = DateTime.Now;
            }

            complianceApplicationManager.Save(complianceApp);
        }
    }
}


