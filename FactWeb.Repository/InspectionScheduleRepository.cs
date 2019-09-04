using FactWeb.Infrastructure;
using FactWeb.Model;
using FactWeb.RepositoryContracts;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;


namespace FactWeb.Repository
{
    public class InspectionScheduleRepository : BaseRepository<InspectionSchedule>, IInspectionScheduleRepository
    {
        public InspectionScheduleRepository(FactWebContext context) : base(context)
        {
        }

        public List<InspectionSchedule> GetInspectionScheduleByOrganizationID(int? organizationId, bool? isArchive = false)
        {
            return
                base.FetchMany(
                    x =>
                        (x.OrganizationId == organizationId || organizationId == null) && x.IsActive &&
                        (x.IsArchive == isArchive || isArchive == null));

        }

        public List<InspectionSchedule> GetInspectionScheduleByAppIdOrganizationID(int organizationId, int applicationId, bool? isArchive = false)
        {
            return base.FetchMany(x => x.OrganizationId == organizationId && x.ApplicationId == applicationId && x.IsActive && (x.IsArchive == isArchive || isArchive == null));

        }

        public List<InspectionSchedule> GetAllForCompliance(Guid complianceApplicationId)
        {
            return this.Context.InspectionSchedules
                .Include(x => x.InspectionScheduleDetails)
                .Include(x => x.InspectionScheduleDetails.Select(y=>y.AccreditationRole))
                .Where(x => x.Applications.ComplianceApplicationId == complianceApplicationId && x.IsActive && !x.IsArchive)
                .ToList();
        }

        

    }
}
 