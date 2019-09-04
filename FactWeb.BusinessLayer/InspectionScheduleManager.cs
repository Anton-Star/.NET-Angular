using FactWeb.Model;
using FactWeb.Model.InterfaceItems;
using FactWeb.RepositoryContracts;
using System;
using System.Collections.Generic;

namespace FactWeb.BusinessLayer
{
    public class InspectionScheduleManager : BaseManager<InspectionScheduleManager, IInspectionScheduleRepository, InspectionSchedule>
    {
        public InspectionScheduleManager(IInspectionScheduleRepository repository) : base(repository)
        {
        }

        public List<InspectionSchedule> GetInspectionScheduleByOrganizationID(int? organizationId)
        {
            LogMessage("GetInspectionScheduleByOrganizationID (InspectionScheduleManager)");

            return base.Repository.GetInspectionScheduleByOrganizationID(organizationId);
        }

        public List<InspectionSchedule> GetInspectionScheduleByAppIdOrganizationID(int organizationId, int applicationId, bool? isArchive = false)
        {
            LogMessage("GetInspectionScheduleByAppIdOrganizationID (InspectionScheduleManager)");

            return base.Repository.GetInspectionScheduleByAppIdOrganizationID(organizationId, applicationId, isArchive);
        }

        public List<InspectionSchedule> GetAllForCompliance(Guid complianceApplicationId)
        {
            return base.Repository.GetAllForCompliance(complianceApplicationId);
        }

        public void UpdateDetails(InspectionOverallDetail detail, string updatedBy)
        {
            var det = this.Repository.GetById(detail.InspectionScheduleId);

            if (det != null)
            {
                det.CommendablePractices = detail.CommendablePractices;
                det.OverallImpressions = detail.OverallImpressions;
                det.SiteDescription = detail.SiteDescription;
                det.UpdatedBy = updatedBy;
                det.UpdatedDate = DateTime.Now;

                this.Repository.Save(det);
            }
        }

    }
}
