using FactWeb.Model;
using FactWeb.Model.InterfaceItems;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FactWeb.RepositoryContracts
{
    public interface IInspectionRepository : IRepository<Inspection>
    {
        Task<Inspection> GetInspectionByAppIdAsync(Guid applicationUniqueId);
        Inspection GetByApplication(Guid applicationUniqueId, bool isTrainee, bool isReinspection);
        Task<Inspection> GetByApplicationAsync(Guid applicationUniqueId, bool isTrainee, bool isReinspection);

        Inspection GetByApplicationAndSite(Guid complianceAppId, string siteName, bool isTrainee, bool isReinspection);
        Task<Inspection> GetByApplicationAndSiteAsync(Guid complianceAppId, string siteName, bool isTrainee, bool isReinspection);

        List<InspectionDetail> GetInspectionDetails(Guid complianceAppId);
        InspectionOverallDetail GetInspectionOverallDetails(Guid complianceAppId);
    }
}