using FactWeb.Model;
using FactWeb.RepositoryContracts;
using System;
using System.Collections.Generic;

namespace FactWeb.BusinessLayer
{
    public class ComplianceApplicationInspectionDetailManager : BaseManager<ComplianceApplicationInspectionDetailManager, IComplianceApplicationInspectionDetailRepository, ComplianceApplicationInspectionDetail>
    {
        public ComplianceApplicationInspectionDetailManager(IComplianceApplicationInspectionDetailRepository repository) : base(repository)
        {
        }

        public List<ComplianceApplicationInspectionDetail> GetByCompApp(Guid complianceApplicationId)
        {
            return base.Repository.GetByCompApp(complianceApplicationId);
        }

        public ComplianceApplicationInspectionDetail GetByInspectionSched(int inspectionScheduleId)
        {
            return base.Repository.GetByInspectionSched(inspectionScheduleId);
        }
    }
}
