using FactWeb.Model;
using System;
using System.Collections.Generic;

namespace FactWeb.RepositoryContracts
{
    public interface IComplianceApplicationInspectionDetailRepository : IRepository<ComplianceApplicationInspectionDetail>
    {
        List<ComplianceApplicationInspectionDetail> GetByCompApp(Guid complianceApplicationId);
        ComplianceApplicationInspectionDetail GetByInspectionSched(int inspectionScheduleId);
    }
}
