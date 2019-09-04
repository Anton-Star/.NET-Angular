using FactWeb.Model;
using FactWeb.RepositoryContracts;
using System;
using System.Collections.Generic;

namespace FactWeb.Repository
{
    public class ComplianceApplicationInspectionDetailRepository : BaseRepository<ComplianceApplicationInspectionDetail>, IComplianceApplicationInspectionDetailRepository
    {
        public ComplianceApplicationInspectionDetailRepository(FactWebContext context) : base(context)
        {
        }

        public List<ComplianceApplicationInspectionDetail> GetByCompApp(Guid complianceApplicationId)
        {
            return base.FetchMany(x => x.ComplianceApplicationId == complianceApplicationId);
        }

        public ComplianceApplicationInspectionDetail GetByInspectionSched(int inspectionScheduleId)
        {
            return base.Fetch(x => x.InspectionScheduleId == inspectionScheduleId);
        }
    }
}
