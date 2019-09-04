using FactWeb.Infrastructure;
using FactWeb.Model;
using FactWeb.RepositoryContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FactWeb.BusinessLayer
{
    public class InspectionScheduleDetailManager : BaseManager<InspectionScheduleDetailManager, IInspectionScheduleDetailRepository, InspectionScheduleDetail>
    {
        public InspectionScheduleDetailManager(IInspectionScheduleDetailRepository repository) : base(repository)
        {
        }

        /// <summary>
        /// Get all active records from Inspection Schedule against inspection schedule id 
        /// </summary>
        /// <param name="inspectionScheduleId"></param>
        /// <returns></returns>
        public List<InspectionScheduleDetail> GetInspectionScheduleDetail(int inspectionScheduleId)
        {
            LogMessage("GetInspectionScheduleDetail (InspectionScheduleDetailManager)");

            return base.Repository.GetByInspectionScheduleId(inspectionScheduleId);
        }

        /// <summary>
        /// Get all active records from Inspection Schedule against inspection schedule id asynchronously
        /// </summary>
        /// <param name="inspectionScheduleId"></param>
        /// <returns></returns>
        public Task<List<InspectionScheduleDetail>> GetInspectionScheduleDetailAsync(int inspectionScheduleId)
        {
            LogMessage("GetInspectionScheduleDetailAsync (InspectionScheduleDetailManager)");

            return base.Repository.GetByInspectionScheduleIdAsync(inspectionScheduleId);
        }

        /// <summary>
        /// Get all active records from Inspection Schedule against inspection schedule id
        /// </summary>
        /// <param name="inspectionScheduleId"></param>
        /// <returns></returns>
        public List<InspectionScheduleDetail> GetActiveByInspectionScheduleId(int inspectionScheduleId)
        {
            LogMessage("GetActiveByInspectionScheduleId (InspectionScheduleDetailManager)");

            return base.Repository.GetAllActiveByInspectionScheduleId(inspectionScheduleId);
        }

        /// <summary>
        /// Get all active records from Inspection Schedule against inspection schedule id asynchronously
        /// </summary>
        /// <param name="inspectionScheduleId"></param>
        /// <returns></returns>
        public Task<List<InspectionScheduleDetail>> GetActiveByInspectionScheduleIdAsync(int inspectionScheduleId)
        {
            LogMessage("GetActiveByInspectionScheduleIdAsync (InspectionScheduleDetailManager)");

            return base.Repository.GetAllActiveByInspectionScheduleIdAsync(inspectionScheduleId);
        }

        /// <summary>
        /// Get user's Accreditation Role in inspection schedule
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="applicationId"></param>
        /// <returns></returns>
        public AccreditationRole GetAccreditationRoleByUserId(Guid userId, Guid uniqueId)
        {
            LogMessage("GetAccreditationRoleByUserIdAsync (InspectionScheduleDetailManager)");

            var schedules = base.Repository.GetAllActiveForApplication(uniqueId);

            if (schedules.Count == 0) return null;

            var schedule = schedules.FirstOrDefault(x => x.UserId == userId);

            if (schedule == null) return null;

            if (schedule.AccreditationRole.Name == Constants.AccreditationsRoles.InspectorTrainees && schedule.IsInspectionComplete.GetValueOrDefault())
            {
                return new AccreditationRole
                {
                    Id = 1,
                    Name = Constants.AccreditationsRoles.Inspectors
                };
            }

            return schedule.AccreditationRole;
        }

        public List<InspectionScheduleDetail> GetAllActiveByApplication(Guid applicationUniqueId)
        {
            return base.Repository.GetAllActiveForApplication(applicationUniqueId);
        }

        public Task<List<InspectionScheduleDetail>> GetAllActiveByApplicationAsync(Guid applicationUniqueId)
        {
            return base.Repository.GetAllActiveForApplicationAsync(applicationUniqueId);
        }

        public List<InspectionScheduleDetail> GetAllActiveByComplianceApplication(Guid complianceApplicationId)
        {
            return base.Repository.GetAllActiveForComplianceApplication(complianceApplicationId);
        }

        public bool CanSeeFactOnlyDocuments(string orgName, int userRoleId, Guid userId)
        {
            if (userRoleId == (int) Constants.Role.FACTAdministrator ||
                userRoleId == (int) Constants.Role.FACTCoordinator || userRoleId == (int) Constants.Role.QualityManager)
                return true;

            var details = this.GetAllByUserAndOrg(orgName, userId);

            return details.Any();
        }

        public Task<List<InspectionScheduleDetail>> GetAllActiveByComplianceApplicationAsync(Guid complianceApplicationId)
        {
            return base.Repository.GetAllActiveForComplianceApplicationAsync(complianceApplicationId);
        }

        public void SaveMentorFeedback(int inspectionScheduleDetailId, string mentorFeedback, string savedBy)
        {
            var record = this.GetById(inspectionScheduleDetailId);

            if (record == null)
            {
                return;
            }

            record.MentorFeedback = mentorFeedback;
            record.UpdatedBy = savedBy;
            record.UpdatedDate = DateTime.Now;

            base.Repository.Save(record);
        }

        public async Task SaveMentorFeedbackAsync(int inspectionScheduleDetailId, string mentorFeedback, string savedBy)
        {
            var record = this.GetById(inspectionScheduleDetailId);

            if (record == null)
            {
                return;
            }

            record.MentorFeedback = mentorFeedback;
            record.UpdatedBy = savedBy;
            record.UpdatedDate = DateTime.Now;

            await base.Repository.SaveAsync(record);
        }

        public List<InspectionScheduleDetail> GetAllByUserAndOrg(string organizationName, Guid userId)
        {
            return base.Repository.GetAllByUserAndOrg(organizationName, userId);
        }

        public List<InspectionScheduleDetail> GetAllForCompliance(Guid complianceApplicationId)
        {
            return base.Repository.GetAllForCompliance(complianceApplicationId);
        }
    }
}
