using FactWeb.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FactWeb.RepositoryContracts
{
    public interface IInspectionScheduleDetailRepository : IRepository<InspectionScheduleDetail>
    {
        List<InspectionScheduleDetail> GetAllActiveByInspectionScheduleId(int inspectionScheduleId, bool? isArchive = false);

        Task<List<InspectionScheduleDetail>> GetAllActiveByInspectionScheduleIdAsync(int inspectionScheduleId, bool? isArchive = false);
        List<InspectionScheduleDetail> GetByInspectionScheduleId(int inspectionScheduleId, bool? isArchive = false);

        Task<List<InspectionScheduleDetail>> GetByInspectionScheduleIdAsync(int inspectionScheduleId, bool? isArchive = false);
        InspectionScheduleDetail GetAccreditationRoleByUserId(Guid userId, Guid uniqueId, bool? isArchive = false);

        List<InspectionScheduleDetail> GetAllActiveForApplication(Guid applicationUniqueId, bool? isArchive = false);
        Task<List<InspectionScheduleDetail>> GetAllActiveForApplicationAsync(Guid applicationUniqueId, bool? isArchive = false);

        List<InspectionScheduleDetail> GetAllActiveForComplianceApplication(Guid complianceApplicationId, bool? isArchive = false);
        Task<List<InspectionScheduleDetail>> GetAllActiveForComplianceApplicationAsync(Guid complianceApplicationId, bool? isArchive = false);
        List<InspectionScheduleDetail> GetAllByUserAndOrg(string organizationName, Guid userId);
        List<InspectionScheduleDetail> GetAllForCompliance(Guid complianceApplicationId);
    }
}
