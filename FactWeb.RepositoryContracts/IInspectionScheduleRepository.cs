using FactWeb.Model;
using System;
using System.Collections.Generic;

namespace FactWeb.RepositoryContracts
{
    public interface IInspectionScheduleRepository : IRepository<InspectionSchedule>
    {
        List<InspectionSchedule> GetInspectionScheduleByOrganizationID(int? organizationId, bool? isArchive = false);

        List<InspectionSchedule> GetInspectionScheduleByAppIdOrganizationID(int organizationId, int applicationId, bool? isArchive = false);

        List<InspectionSchedule> GetAllForCompliance(Guid complianceApplicationId);
    }
}
