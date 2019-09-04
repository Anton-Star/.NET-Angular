using FactWeb.Model;
using FactWeb.Model.InterfaceItems;
using FactWeb.RepositoryContracts;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FactWeb.BusinessLayer
{
    public class InspectionManager : BaseManager<InspectionManager, IInspectionRepository, Inspection>
    {
        public InspectionManager(IInspectionRepository repository) : base(repository)
        {
        }

        public bool Save(InspectionItem inspectionItem, int applicationId, Guid inspectorId, bool isTrainee, bool isReinspection, string siteDesc, string email)
        {
            LogMessage("Save (InspectionManager)");


            var inspection = this.GetInspection(inspectionItem.ApplicationUniqueId, isTrainee, isReinspection);

            if (inspection == null)
            {
                inspection = new Inspection
                {
                    ApplicationId = applicationId,
                    CommendablePractices = inspectionItem.CommendablePractices,
                    OverallImpressions = inspectionItem.OverallImpressions,
                    TraineeSiteDescription = siteDesc,
                    IsTrainee = isTrainee,
                    InspectorId = inspectorId,
                    CreatedBy = email,
                    CreatedDate = DateTime.Now,
                    IsReinspection = isReinspection
                };

                base.Repository.Add(inspection);
            }
            else
            {
                if (inspectionItem.IsOverride)
                {
                    inspection.OverridenPractices = inspection.CommendablePractices;
                    inspection.OverridenImpressions = inspection.OverallImpressions;
                }

                inspection.CommendablePractices = inspectionItem.CommendablePractices;
                inspection.OverallImpressions = inspectionItem.OverallImpressions;
                inspection.TraineeSiteDescription = siteDesc;
                inspection.UpdatedBy = email;
                inspection.UpdatedDate = DateTime.Now;

                base.Repository.Save(inspection);
            }
            
            return true;
        }

        public Task<Inspection> GetInspectionAsync(Guid applicationUniqueId, bool isTrainee, bool isReinspection)
        {
            return base.Repository.GetByApplicationAsync(applicationUniqueId, isTrainee, isReinspection);
        }

        public Inspection GetInspection(Guid applicationUniqueId, bool isTrainee, bool isReinspection)
        {
            return base.Repository.GetByApplication(applicationUniqueId, isTrainee, isReinspection);
        }

        public Task<Inspection> GetInspectionBySiteAsync(Guid complianceAppId, string siteName, bool isTrainee, bool isReinspection)
        {
            return base.Repository.GetByApplicationAndSiteAsync(complianceAppId, siteName, isTrainee, isReinspection);
        }

        public Inspection GetInspectionBySite(Guid complianceAppId, string siteName, bool isTrainee, bool isReinspection)
        {
            return base.Repository.GetByApplicationAndSite(complianceAppId, siteName, isTrainee, isReinspection);
        }

        public Task<Inspection> GetInspectionByAppIdAsync(Guid applicationUniqueId)
        {
            return base.Repository.GetInspectionByAppIdAsync(applicationUniqueId);
        }

        public List<InspectionDetail> GetInspectionDetails(Guid complianceAppId)
        {
            return base.Repository.GetInspectionDetails(complianceAppId);
        }

        public InspectionOverallDetail GetInspectionOverallDetails(Guid complianceAppId)
        {
            var detail = base.Repository.GetInspectionOverallDetails(complianceAppId);

            if (detail != null) return detail;

            var details = this.GetInspectionDetails(complianceAppId);

            detail = new InspectionOverallDetail();

            foreach (var d in details)
            {
                detail.CommendablePractices += $"<b>{d.SiteName}</b><br><b>Inspector: {d.Inspector}</b><br><br>{d.CommendablePractices}<br><br>";
                detail.SiteDescription +=
                    $"<b>{d.SiteName}</b><br><b>Inspector: {d.Inspector}</b><br><br>{d.SiteDescription}<br><br>";
                detail.OverallImpressions += $"<b>{d.SiteName}</b><br><b>Inspector: {d.Inspector}</b><br><br>{d.OverallImpressions}<br><br>";
                detail.InspectionScheduleId = d.InspectionScheduleId;
            }

            return detail;
        }
    }
}
