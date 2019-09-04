using FactWeb.Infrastructure;
using FactWeb.Model;
using FactWeb.RepositoryContracts;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace FactWeb.Repository
{
    public class InspectionScheduleDetailRepository : BaseRepository<InspectionScheduleDetail>, IInspectionScheduleDetailRepository
    {
        public InspectionScheduleDetailRepository(FactWebContext context) : base(context)
        {
        }

        /// <summary>
        /// Get all active records from Inspection Schedule against inspection schedule id
        /// </summary>
        /// <param name="inspectionScheduleId"></param>
        /// <returns></returns>
        public List<InspectionScheduleDetail> GetAllActiveByInspectionScheduleId(int inspectionScheduleId , bool? isArchive = false)
        {
            if (isArchive ==  null)
                return base.FetchMany(x => x.InspectionScheduleId == inspectionScheduleId && x.IsActive ==true); 
            else
                return base.FetchMany(x => x.InspectionScheduleId == inspectionScheduleId && x.IsActive == true && x.IsArchive == isArchive);

        }

        /// <summary>
        /// Get all active records from Inspection Schedule against inspection schedule id asynchronously
        /// </summary>
        /// <param name="inspectionScheduleId"></param>
        /// <returns></returns>
        public Task<List<InspectionScheduleDetail>> GetAllActiveByInspectionScheduleIdAsync(int inspectionScheduleId, bool? isArchive = false)
        {
            if (isArchive == null)
                return base.FetchManyAsync(x => x.InspectionScheduleId == inspectionScheduleId && x.IsActive == true);
            else
                return base.FetchManyAsync(x => x.InspectionScheduleId == inspectionScheduleId && x.IsActive == true && x.IsArchive == isArchive);
        }

        /// <summary>
        /// /// Get all records from Inspection Schedule against inspection schedule id 
        /// </summary>
        /// <param name="inspectionScheduleId"></param>
        /// <returns></returns>
        public List<InspectionScheduleDetail> GetByInspectionScheduleId(int inspectionScheduleId, bool? isArchive = false)
        {
            if(isArchive == null)
                return base.FetchMany(x => x.InspectionScheduleId == inspectionScheduleId);
            else
                return base.FetchMany(x => x.InspectionScheduleId == inspectionScheduleId && x.IsArchive == isArchive);
        }

        /// <summary>
        /// /// Get all records from Inspection Schedule against inspection schedule id 
        /// </summary>
        /// <param name="inspectionScheduleId"></param>
        /// <returns></returns>
        public Task<List<InspectionScheduleDetail>> GetByInspectionScheduleIdAsync(int inspectionScheduleId, bool? isArchive = false)
        {
            if (isArchive == null)
                return base.FetchManyAsync(x => x.InspectionScheduleId == inspectionScheduleId);            
            else
                return base.FetchManyAsync(x => x.InspectionScheduleId == inspectionScheduleId && x.IsArchive == isArchive);
        }

        public InspectionScheduleDetail GetAccreditationRoleByUserId(Guid userId, Guid uniqueId, bool? isArchive = false)
        {
            return isArchive == null
                ? base.Fetch(x => x.InspectionSchedule.Applications.UniqueId == uniqueId && x.UserId == userId)
                    : base.Fetch(
                    x =>
                        x.InspectionSchedule.Applications.UniqueId == uniqueId && x.UserId == userId &&
                        x.IsArchive == isArchive);
        }

        public List<InspectionScheduleDetail> GetAllActiveForApplication(Guid applicationUniqueId, bool? isArchive = false)
        {
            if (isArchive == null)
                return base.FetchMany(x => x.InspectionSchedule.Applications.UniqueId == applicationUniqueId);
            else
                return base.FetchMany(x => x.InspectionSchedule.Applications.UniqueId == applicationUniqueId && x.IsArchive == isArchive);
        }

        public Task<List<InspectionScheduleDetail>> GetAllActiveForApplicationAsync(Guid applicationUniqueId, bool? isArchive = false)
        {
            if (isArchive == null)
                return base.FetchManyAsync(x => x.InspectionSchedule.Applications.UniqueId == applicationUniqueId && !x.IsArchive);
            else
                return base.FetchManyAsync(x => x.InspectionSchedule.Applications.UniqueId == applicationUniqueId && x.IsArchive == isArchive);
        }

        public List<InspectionScheduleDetail> GetAllActiveForComplianceApplication(Guid complianceApplicationId, bool? isArchive = false)
        {
            if (isArchive == null)
                return base.Context.InspectionScheduleDetails
                    .Include(x => x.AccreditationRole)
                    .Include(x=>x.InspectionSchedule)
                    .Include(x=>x.Site)
                    .Where(
                        x =>
                            x.InspectionSchedule.Applications.ComplianceApplicationId.Value == complianceApplicationId)
                    .ToList();
            else
            {
                return base.Context.InspectionScheduleDetails
                    .Include(x => x.AccreditationRole)
                    .Include(x => x.InspectionSchedule)
                    .Include(x => x.Site)
                    .Where(
                        x =>
                            x.InspectionSchedule.Applications.ComplianceApplicationId.Value == complianceApplicationId &&
                            x.IsArchive == isArchive)
                    .ToList();
            }
        }

        public Task<List<InspectionScheduleDetail>> GetAllActiveForComplianceApplicationAsync(Guid complianceApplicationId, bool? isArchive = false)
        {
            if (isArchive == null)
                return base.FetchManyAsync(x => x.InspectionSchedule.Applications.ComplianceApplicationId.Value == complianceApplicationId && x.IsActive);
            else
                return base.FetchManyAsync(x => x.InspectionSchedule.Applications.ComplianceApplicationId.Value == complianceApplicationId && x.IsActive && x.IsArchive == isArchive);
        }

        public List<InspectionScheduleDetail> GetAllByUserAndOrg(string organizationName, Guid userId)
        {
            return
                base.FetchMany(
                    x => x.UserId == userId && x.InspectionSchedule.Applications.Organization.Name == organizationName);
        }

        public List<InspectionScheduleDetail> GetAllForCompliance(Guid complianceApplicationId)
        {
            return this.Context.InspectionScheduleDetails
                .Include(x => x.AccreditationRole)
                .Include(x => x.User)
                .Include(x => x.InspectionCategory)
                .Include(x => x.Site)
                .Where(x => x.InspectionSchedule.Applications.ComplianceApplicationId == complianceApplicationId)
                .ToList();
        }
    }
}
