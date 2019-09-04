using FactWeb.BusinessLayer;
using FactWeb.Infrastructure;
using FactWeb.Model;
using FactWeb.Model.InterfaceItems;
using SimpleInjector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FactWeb.BusinessFacade
{
    public class InspectionScheduleFacade
    {
        private readonly Container container;

        public InspectionScheduleFacade(Container container)
        {
            this.container = container;
        }
       
        /// <summary>
        /// Delete inspection schedule and related inspeciton schedule detail record against inspection schedule id 
        /// </summary>
        /// <param name="inspectionScheduleId"></param>
        /// <returns></returns>
        public bool DeleteSchedule(int inspectionScheduleId)
        {
            var inspectionScheduleManager = this.container.GetInstance<InspectionScheduleManager>();
            var inspectionScheduleDetailManager = this.container.GetInstance<InspectionScheduleDetailManager>();
            var inspectionSchedule = inspectionScheduleManager.GetById(inspectionScheduleId);
            if (inspectionSchedule != null)
            {
                List<InspectionScheduleDetail> inspectionScheduleDetailList = inspectionScheduleDetailManager.GetActiveByInspectionScheduleId(inspectionSchedule.Id);
                inspectionSchedule.IsActive = false;
                inspectionScheduleManager.Save(inspectionSchedule);

                foreach (var inspectionScheduleDetail in inspectionScheduleDetailList)
                {
                    inspectionScheduleDetail.IsActive = false;
                    inspectionScheduleDetailManager.Save(inspectionScheduleDetail);
                }
            }            
            return true;
        }

        /// <summary>
        /// Delete inspection schedule and related inspeciton schedule detail record against inspection schedule id asynhcronously
        /// </summary>
        /// <param name="inspectionScheduleId"></param>
        /// <returns></returns>
        public Task<bool> DeleteScheduleAsync(int inspectionScheduleId)
        {
            var inspectionScheduleManager = this.container.GetInstance<InspectionScheduleManager>();
            var inspectionScheduleDetailManager = this.container.GetInstance<InspectionScheduleDetailManager>();
            var inspectionSchedule = inspectionScheduleManager.GetById(inspectionScheduleId);
            if (inspectionSchedule != null)
            {
                List<InspectionScheduleDetail> inspectionScheduleDetailList = inspectionScheduleDetailManager.GetActiveByInspectionScheduleId(inspectionSchedule.Id);
                inspectionSchedule.IsActive = false;
                inspectionScheduleManager.Save(inspectionSchedule);

                foreach (var inspectionScheduleDetail in inspectionScheduleDetailList)
                {
                    inspectionScheduleDetail.IsActive = false;
                    inspectionScheduleDetailManager.Save(inspectionScheduleDetail);
                }
            }
            return Task.FromResult(true);
        }

        /// <summary>
        /// Archive inspection schedule and related inspeciton schedule detail record against inspection schedule id 
        /// </summary>
        /// <param name="inspectionScheduleId"></param>
        /// <returns></returns>
        public bool ArchiveSchedule(int inspectionScheduleId)
        {
            var inspectionScheduleManager = this.container.GetInstance<InspectionScheduleManager>();
            var inspectionScheduleDetailManager = this.container.GetInstance<InspectionScheduleDetailManager>();
            var inspectionSchedule = inspectionScheduleManager.GetById(inspectionScheduleId);
            if (inspectionSchedule != null)
            {
                List<InspectionScheduleDetail> inspectionScheduleDetailList = inspectionScheduleDetailManager.GetActiveByInspectionScheduleId(inspectionSchedule.Id);
                inspectionSchedule.IsArchive = true;
                inspectionScheduleManager.Save(inspectionSchedule);

                foreach (var inspectionScheduleDetail in inspectionScheduleDetailList)
                {
                    inspectionScheduleDetail.IsArchive = true;
                    inspectionScheduleDetailManager.Save(inspectionScheduleDetail);
                }
            }
            return true;
        }

        /// <summary>
        /// Archive inspection schedule and related inspeciton schedule detail record against inspection schedule id asynhcronously
        /// </summary>
        /// <param name="inspectionScheduleId"></param>
        /// <returns></returns>
        public Task<bool> ArchiveScheduleAsync(int inspectionScheduleId)
        {
            var inspectionScheduleManager = this.container.GetInstance<InspectionScheduleManager>();
            var inspectionScheduleDetailManager = this.container.GetInstance<InspectionScheduleDetailManager>();
            var inspectionSchedule = inspectionScheduleManager.GetById(inspectionScheduleId);
            if (inspectionSchedule != null)
            {
                List<InspectionScheduleDetail> inspectionScheduleDetailList = inspectionScheduleDetailManager.GetActiveByInspectionScheduleId(inspectionSchedule.Id);
                inspectionSchedule.IsArchive = true;
                inspectionScheduleManager.Save(inspectionSchedule);

                foreach (var inspectionScheduleDetail in inspectionScheduleDetailList)
                {
                    inspectionScheduleDetail.IsArchive = true;
                    inspectionScheduleDetailManager.Save(inspectionScheduleDetail);
                }
            }
            return Task.FromResult(true);
        }

        /// <summary>
        /// Get all inspection schedule details record against inspection schedule Id
        /// </summary>
        /// <param name="inspectionScheduleId"></param>
        /// <returns></returns>
        public List<InspectionScheduleDetail> GetInspectionScheduleDetail(int inspectionScheduleId)
        {
            var inspectionScheduleDetailManager = this.container.GetInstance<InspectionScheduleDetailManager>();
            var inspectionScheduleDetailList = inspectionScheduleDetailManager.GetInspectionScheduleDetail(inspectionScheduleId);
            return inspectionScheduleDetailList;
        }

        /// <summary>
        /// Get all inspection schedule details record against inspection schedule Id asynchronously
        /// </summary>
        /// <param name="inspectionScheduleId"></param>
        /// <returns></returns>
        public Task<List<InspectionScheduleDetail>> GetInspectionScheduleDetailAsync(int inspectionScheduleId)
        {
            var inspectionScheduleDetailManager = this.container.GetInstance<InspectionScheduleDetailManager>();
            var inspectionScheduleDetailList = inspectionScheduleDetailManager.GetInspectionScheduleDetailAsync(inspectionScheduleId);
            return inspectionScheduleDetailList;
        }

        /// <summary>
        /// /// Get all inspection schedule records which are active against inspection schedule id
        /// </summary>
        /// <param name="inspectionScheduleId"></param>
        /// <returns></returns>
        public List<InspectionScheduleDetail> GetActiveByInspectionScheduleId(int inspectionScheduleId)
        {
            var inspectionScheduleDetailManager = this.container.GetInstance<InspectionScheduleDetailManager>();
            var inspectionScheduleDetailList = inspectionScheduleDetailManager.GetActiveByInspectionScheduleId(inspectionScheduleId);
            return inspectionScheduleDetailList;
        }

        /// <summary>
        /// Get all inspection schedule records which are active against inspection schedule id asynchronously
        /// </summary>
        /// <param name="inspectionScheduleId"></param>
        /// <returns></returns>
        public Task<List<InspectionScheduleDetail>> GetActiveByInspectionScheduleIdAsync(int inspectionScheduleId)
        {
            var inspectionScheduleDetailManager = this.container.GetInstance<InspectionScheduleDetailManager>();
            var inspectionScheduleDetailList = inspectionScheduleDetailManager.GetActiveByInspectionScheduleIdAsync(inspectionScheduleId);
            return inspectionScheduleDetailList;
        }

        /// <summary>
        /// Get Inspection schedule record against inspection schedule id
        /// </summary>
        /// <param name="inspectionScheduleId"></param>
        /// <returns></returns>
        public InspectionSchedule GetInspectionScheduleById(int inspectionScheduleId)
        {
            var inspectionSchedulManager = this.container.GetInstance<InspectionScheduleManager>();
            var inspectionSchedule = inspectionSchedulManager.GetById(inspectionScheduleId);
            return inspectionSchedule;
        }

        public List<InspectionSchedule> GetInspectionSchedulesByOrg(int? organizationId)
        {
            var inspectionSchedulManager = this.container.GetInstance<InspectionScheduleManager>();
            var inspectionSchedule = inspectionSchedulManager.GetInspectionScheduleByOrganizationID(organizationId);
            return inspectionSchedule;
        }

        /// <summary>
        /// Get all inspection schedule record against organization id and applicaiton id 
        /// </summary>
        /// <param name="organizationId"></param>
        /// <param name="applicationId"></param>
        /// <returns></returns>
        public List<InspectionSchedule> GetInspectionScheduleByAppIdOrganizationID(int organizationId, int applicationId, bool? isArchive)
        {
            var inspectionSchedulManager = this.container.GetInstance<InspectionScheduleManager>();
            var inspectionSchedule = inspectionSchedulManager.GetInspectionScheduleByAppIdOrganizationID(organizationId, applicationId, isArchive);
            return inspectionSchedule;
        }

        /// <summary>
        /// Get Inspection schedule record by organization id and application id 
        /// </summary>
        /// <param name="inspectionScheduleId"></param>
        /// <returns></returns>
        public InspectionSchedule GetInspectionSchedule(int organizationId, int applicationId)
        {
            var inspectionSchedulManager = this.container.GetInstance<InspectionScheduleManager>();
            var inspectionScheduleList = inspectionSchedulManager.GetInspectionScheduleByOrganizationID(organizationId);
            InspectionSchedule inspectionSchedule = null;

            if (inspectionScheduleList.Count() > 0)
            {
                inspectionSchedule = inspectionScheduleList.Where(x => x.ApplicationId == applicationId).First();
            }

            return inspectionSchedule;
        }

        /// <summary>
        /// Get all the inspection categories
        /// </summary>
        /// <returns></returns>
        public List<InspectionCategory> GetAllInspectionCategories()
        {
            var inspectionCategoryManager = this.container.GetInstance<InspectionCategoryManager>();

            return inspectionCategoryManager.GetAll();
        }

        /// <summary>
        /// Get all the inspection categories asynchronously
        /// </summary>
        /// <returns></returns>
        public Task<List<InspectionCategory>> GetAllInspectionCategoriesAsync()
        {
            var inspectionCategoryManager = this.container.GetInstance<InspectionCategoryManager>();

            return Task.FromResult(inspectionCategoryManager.GetAll());
        }

        /// <summary>
        /// Get all the inspection categories with application id
        /// </summary>
        /// <returns></returns>
        public List<InspectionCategory> GetAllInspectionCategories(int applicationId)
        {
            var inspectionCategoryManager = this.container.GetInstance<InspectionCategoryManager>();
            var applicationTypeCategoryManager = this.container.GetInstance<ApplicationTypeCategoryManager>();
            var applcationManager = this.container.GetInstance<ApplicationManager>();
            
            var applicaiton = applcationManager.GetById(applicationId);
            var applicationTypeCategoryList = applicationTypeCategoryManager.GetAll().Where(x => x.ApplicationTypeId == applicaiton.ApplicationTypeId);
            
            var result = from insCat in inspectionCategoryManager.GetAll()
                         join appTypeCat in applicationTypeCategoryList on insCat.Id equals appTypeCat.InspectionCategoryId
                                 select insCat;

            return result.ToList();
        }

        /// <summary>
        /// Get all the inspection categories with application id asynchronously
        /// </summary>
        /// <returns></returns>
        public Task<List<InspectionCategory>> GetAllInspectionCategoriesAsync(int applicationId)
        {
            var inspectionCategoryManager = this.container.GetInstance<InspectionCategoryManager>();
            var applicationTypeCategoryManager = this.container.GetInstance<ApplicationTypeCategoryManager>();
            var applcationManager = this.container.GetInstance<ApplicationManager>();
         
            var applicaiton = applcationManager.GetById(applicationId);
            var applicationTypeCategoryList =  applicationTypeCategoryManager.GetAll().Where(x => x.ApplicationTypeId == applicaiton.ApplicationTypeId);
         
            var result = from insCat in inspectionCategoryManager.GetAll()
                         join appTypeCat in applicationTypeCategoryList on insCat.Id equals appTypeCat.InspectionCategoryId
                         select insCat;

            return Task.FromResult(result.ToList());
        }
       
        /// <summary>
        /// Adds new inspection schedule and inspection schedule details record
        /// </summary>
        /// <param name="inspectionScheduleId"></param>
        /// <param name="inspectionScheduleDetailId"></param>
        /// <param name="organizationId"></param>
        /// <param name="applicationId"></param>
        /// <param name="selectedUserId"></param>
        /// <param name="selectedRoleId"></param>
        /// <param name="selectedCategoryId"></param>
        /// <param name="lead"></param>
        /// <param name="mentor"></param>
        /// <param name="inspDate"></param>
        /// <param name="selectedFacilityList"></param>
        /// <param name="userEmail"></param>
        public int SaveInspectionSchedule(string inspectionScheduleId, string inspectionScheduleDetailId, string organizationId, string applicationId, string selectedUserId, string selectedRoleId, string selectedCategoryId, bool lead, bool mentor, string startDate, string endDate, List<FacilitySiteItems> selectedSitesList, string userEmail, string selectedSiteId)
        {            
            var inspectionScheduleManager = this.container.GetInstance<InspectionScheduleManager>();
            var inspectionScheduleDetailManager = this.container.GetInstance<InspectionScheduleDetailManager>();
            var orgManager = this.container.GetInstance<OrganizationManager>();
            var userManager = this.container.GetInstance<UserManager>();
            var trueVaultManager = this.container.GetInstance<TrueVaultManager>();

            var inspectionScheduleIdLocal = 0;

            if (inspectionScheduleId == "0") // New Schedule 
            {
                var inspectionSchedule = new InspectionSchedule
                {
                    ApplicationId = Convert.ToInt32(applicationId),
                    Id = Convert.ToInt32(inspectionScheduleId),
                    IsActive = true,
                    IsArchive = false,
                    OrganizationId = Convert.ToInt32(organizationId),
                    InspectionDate = DateTime.Now,
                    StartDate = Convert.ToDateTime(startDate),
                    EndDate = Convert.ToDateTime(endDate),
                    CompletionDate = DateTime.Now,
                    CreatedDate = DateTime.Now,
                    CreatedBy = userEmail,
                    UpdatedDate = DateTime.Now,
                    IsCompleted = false
                };
                //Convert.ToDateTime(inspDate);                

                inspectionScheduleManager.Add(inspectionSchedule);
                inspectionScheduleIdLocal = inspectionSchedule.Id;
            }
            else // Update Schedule
            {
                inspectionScheduleIdLocal = Convert.ToInt32(inspectionScheduleId);
                var inspectionSchedule = inspectionScheduleManager.GetById(inspectionScheduleIdLocal);
                inspectionSchedule.InspectionDate = DateTime.Now;//Convert.ToDateTime(inspDate);                                
                inspectionSchedule.StartDate = Convert.ToDateTime(startDate);
                inspectionSchedule.EndDate = Convert.ToDateTime(endDate);
                inspectionSchedule.UpdatedDate = DateTime.Now;
                inspectionSchedule.UpdatedBy = userEmail;
                inspectionScheduleManager.Save(inspectionSchedule);
            }


            if (!string.IsNullOrEmpty(inspectionScheduleDetailId)) // Just Save schedule not schedule detail
            {
                var userId = Guid.Parse(selectedUserId);
                var groups = trueVaultManager.GetAllGroups();

                if (inspectionScheduleDetailId == "0") // new inspection schedule detail
                {
                    var inspectionScheduleDetail = new InspectionScheduleDetail
                    {
                        AccreditationRoleId = Convert.ToInt32(selectedRoleId),
                        UserId = userId,
                        InspectionCategoryId = Convert.ToInt32(selectedCategoryId),
                        InspectionScheduleId = inspectionScheduleIdLocal,
                        SiteId = Convert.ToInt32(selectedSiteId),
                        IsLead = lead,
                        IsMentor = mentor,
                        CreatedDate = DateTime.Now,
                        UpdatedDate = DateTime.Now,
                        CreatedBy = userEmail,
                        IsActive = true,
                        IsArchive = false
                    };

                    inspectionScheduleDetailManager.Add(inspectionScheduleDetail);
                }
                else // edit inspection schedule detail
                {
                    var inspectionScheduleDetail = inspectionScheduleDetailManager.GetById(Convert.ToInt32(inspectionScheduleDetailId));
                    inspectionScheduleDetail.AccreditationRoleId = Convert.ToInt32(selectedRoleId);
                    inspectionScheduleDetail.UserId = userId;
                    inspectionScheduleDetail.InspectionCategoryId = Convert.ToInt32(selectedCategoryId);
                    inspectionScheduleDetail.SiteId = Convert.ToInt32(selectedSiteId);
                    inspectionScheduleDetail.IsLead = lead;
                    inspectionScheduleDetail.IsMentor = mentor;
                    inspectionScheduleDetail.UpdatedDate = DateTime.Now;
                    inspectionScheduleDetail.UpdatedBy = userEmail;
                    inspectionScheduleDetailManager.Save(inspectionScheduleDetail);
                }

                var user = userManager.GetById(userId);
                var org = orgManager.GetById(Convert.ToInt32(organizationId));

                var userOrgs = new List<UserOrganizationItem>
                {
                    new UserOrganizationItem
                    {
                        Organization = new OrganizationItem
                        {
                            OrganizationName = org.Name,
                            DocumentLibraryGroupId = org.DocumentLibraryGroupId
                        }
                    }
                };

                trueVaultManager.AddUserToGroups(userOrgs, user.DocumentLibraryUserId, groups);
            }
         
            UpdateInspectionScheduleSite(selectedSitesList, inspectionScheduleIdLocal, userEmail);

            return inspectionScheduleIdLocal;
        }

        /// <summary>
        /// Creates a copy of last archived inspection schedule and save in database
        /// </summary>
        /// <param name="inspectionScheduleId"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        public InspectionSchedule CloneInspectionSchedule(int inspectionScheduleId, string email)
        {
            var inspectionScheduleManager = this.container.GetInstance<InspectionScheduleManager>();
            var inspectionScheduleDetailManager = this.container.GetInstance<InspectionScheduleDetailManager>();
            var inspectionScheduleSiteManager = this.container.GetInstance<InspectionScheduleSiteManager>();

            var inspectionScheduleOld = inspectionScheduleManager.GetById(inspectionScheduleId);
            InspectionSchedule inspectionSchedule = new InspectionSchedule();

            inspectionSchedule.ApplicationId = inspectionScheduleOld.ApplicationId;
            inspectionSchedule.IsActive = true;
            inspectionSchedule.IsArchive = false;
            inspectionSchedule.OrganizationId = inspectionScheduleOld.OrganizationId;
            inspectionSchedule.InspectionDate = inspectionScheduleOld.InspectionDate;
            inspectionSchedule.StartDate = inspectionScheduleOld.StartDate;
            inspectionSchedule.EndDate = inspectionScheduleOld.EndDate;
            inspectionSchedule.CompletionDate = inspectionScheduleOld.CompletionDate;
            inspectionSchedule.CreatedDate = DateTime.Now;
            inspectionSchedule.CreatedBy = email;
            inspectionSchedule.UpdatedDate = DateTime.Now;
            inspectionSchedule.IsCompleted = false;

            inspectionScheduleManager.Add(inspectionSchedule);
            
            var inspectionScheduleDetailListOld = GetActiveByInspectionScheduleId(inspectionScheduleOld.Id);

            foreach (var inspectionScheduleDetailOld in inspectionScheduleOld.InspectionScheduleDetails)
            {
                InspectionScheduleDetail inspectionScheduleDetail = new InspectionScheduleDetail();

                inspectionScheduleDetail.InspectionScheduleId = inspectionSchedule.Id;
                inspectionScheduleDetail.AccreditationRoleId = inspectionScheduleDetailOld.AccreditationRoleId;
                inspectionScheduleDetail.UserId = inspectionScheduleDetailOld.UserId;
                inspectionScheduleDetail.InspectionCategoryId = inspectionScheduleDetailOld.InspectionCategoryId;
                inspectionScheduleDetail.SiteId = inspectionScheduleDetailOld.SiteId;
                inspectionScheduleDetail.IsLead = inspectionScheduleDetailOld.IsLead;
                inspectionScheduleDetail.IsMentor = inspectionScheduleDetailOld.IsMentor;
                inspectionScheduleDetail.IsActive = true;
                inspectionScheduleDetail.IsArchive = false;
                inspectionScheduleDetail.CreatedDate = DateTime.Now;
                inspectionScheduleDetail.CreatedBy = email;

                inspectionScheduleDetailManager.BatchAdd(inspectionScheduleDetail);

            }

            inspectionScheduleDetailManager.SaveChanges();

            var existingInspectionScheduleSites = inspectionScheduleSiteManager.GetSiteByInspectionScheduleId(inspectionScheduleOld.Id);
            
            foreach (var site in existingInspectionScheduleSites)
            {
                InspectionScheduleSite inspectionScheduleSite = new InspectionScheduleSite();
                inspectionScheduleSite.SiteID = site.SiteID;
                inspectionScheduleSite.InspectionScheduleId = inspectionSchedule.Id;
                inspectionScheduleSite.InspectionDate = site.InspectionDate;
                inspectionScheduleSite.CreatedBy = email;
                inspectionScheduleSite.CreatedDate = DateTime.Now;

                inspectionScheduleSiteManager.BatchAdd(inspectionScheduleSite);
            }

            inspectionScheduleSiteManager.SaveChanges();

            return inspectionSchedule;            
        }

        /// <summary>
        /// Removes old record and insert newly selected facilities against inspection schedule id
        /// </summary>
        /// <param name="selectedFacilities"></param>
        /// <param name="inspectionScheduleId"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        public bool UpdateInspectionScheduleSite(List<FacilitySiteItems> selectedSites, int inspectionScheduleId, string email)
        {
            var inspectionScheduleSiteManager = this.container.GetInstance<InspectionScheduleSiteManager>();
            var existingInspectionScheduleSites = inspectionScheduleSiteManager.GetSiteByInspectionScheduleId(inspectionScheduleId);

            foreach (var item in existingInspectionScheduleSites)// remove previous facilities
            {
                inspectionScheduleSiteManager.BatchRemove(item);
            }

            inspectionScheduleSiteManager.SaveChanges();

            foreach (var site in selectedSites)
            {
                InspectionScheduleSite inspectionScheduleFacility = new InspectionScheduleSite();
                inspectionScheduleFacility.SiteID = site.SiteId;
                inspectionScheduleFacility.InspectionScheduleId = inspectionScheduleId;
                inspectionScheduleFacility.InspectionDate = Convert.ToDateTime(site.InspectionDate);
                inspectionScheduleFacility.CreatedBy = email;
                inspectionScheduleFacility.CreatedDate = DateTime.Now;

                inspectionScheduleSiteManager.Add(inspectionScheduleFacility);
            }
            return true;
        }

        /// <summary>
        /// Deletes inspection schedule detail record against id asynchronously
        /// </summary>
        /// <param name="inspectionScheduleDetailId"></param>
        /// <returns></returns>
        public bool DeleteStaff(int inspectionScheduleDetailId)
        {
            var inspectionScheduleDetailManager = this.container.GetInstance<InspectionScheduleDetailManager>();
            var inspectionScheduleDetail = inspectionScheduleDetailManager.GetById(inspectionScheduleDetailId);
            inspectionScheduleDetail.IsActive = false;
            inspectionScheduleDetailManager.Save(inspectionScheduleDetail);
            inspectionScheduleDetailManager.SaveChanges();
            return true;
        }
        
        /// <summary>
        /// Deletes inspection schedule detail record against id
        /// </summary>
        /// <param name="inspectionScheduleDetailId"></param>
        /// <returns></returns>
        public Task<bool> DeleteStaffAsync(int inspectionScheduleDetailId)
        {
            var inspectionScheduleDetailManager = this.container.GetInstance<InspectionScheduleDetailManager>();
            var inspectionScheduleDetail = inspectionScheduleDetailManager.GetById(inspectionScheduleDetailId);
            inspectionScheduleDetail.IsActive = false;
            inspectionScheduleDetailManager.Save(inspectionScheduleDetail);
            inspectionScheduleDetailManager.SaveChanges();
            return Task.FromResult(true);
        }

        /// <summary>
        /// Get all inspection schedule record against inspection schedule id 
        /// </summary>
        /// <param name="inspectionScheduleId"></param>
        /// <returns></returns>

        public List<InspectionScheduleSite> GetFacilityByInspectionScheduleId(int inspectionScheduleId)
        {
            var inspectionScheduleFacilityManager = this.container.GetInstance<InspectionScheduleSiteManager>();
            return inspectionScheduleFacilityManager.GetSiteByInspectionScheduleId(inspectionScheduleId);
        }
        /// <summary>
        /// Get all inspection schedule record against inspection schedule id asynchronously
        /// </summary>
        /// <param name="inspectionScheduleId"></param>
        /// <returns></returns>
        public Task<List<InspectionScheduleSite>> GetFacilityByInspectionScheduleIdAsync(int inspectionScheduleId)
        {
            var inspectionScheduleFacilityManager = this.container.GetInstance<InspectionScheduleSiteManager>();
            return inspectionScheduleFacilityManager.GetFacilityByInspectionScheduleIdAsync(inspectionScheduleId);
        }

        //public List<OrganizationFacility> GetSavedSiteByInspectionScheduleId(int inspectionScheduleId, int organiziationId)
        //{
        //    var inspectionScheduleFacilityManager = this.container.GetInstance<InspectionScheduleSiteManager>();
        //    var organizationFacilityManager = this.container.GetInstance<OrganizationFacilityManager>();
        //    List<OrganizationFacility> organizationFacilityList = new List<OrganizationFacility>();

        //    var inspectionScheduleSiteList =  inspectionScheduleFacilityManager.GetSiteByInspectionScheduleId(inspectionScheduleId);

        //    foreach (var site in inspectionScheduleSiteList)
        //    {
        //        var findResult = organizationFacilityManager.GetByOrganizationIdFacilityId(organiziationId, site.FacilityID);
        //        if (findResult.Count > 0)
        //        {
        //            organizationFacilityList.Add(findResult[0]);
        //        }                
        //    }
        //    return organizationFacilityList;

        //}

        public List<FacilitySiteItems> GetSitesAsync(int inspectionScheduleId, int organiziationId)
        {
            var inspectionScheduleSiteManager = this.container.GetInstance<InspectionScheduleSiteManager>();
            var organizationFacilityManager = this.container.GetInstance<OrganizationFacilityManager>();            

            var inspectionScheduleSiteList = inspectionScheduleSiteManager.GetSiteByInspectionScheduleId(inspectionScheduleId);
            var organizationFacilityList = organizationFacilityManager.Search(organiziationId,null);
            
            return ModelConversions.ConvertToFacilitySites(organizationFacilityList, inspectionScheduleSiteList); 

        }

        public AccreditationRole GetAccreditationRoleByUserId(Guid userId, Guid uniqueId)
        {
            var applicationManager = this.container.GetInstance<ApplicationManager>();

            var application = applicationManager.GetByUniqueId(uniqueId);

            if (application == null) return null;

            if (application.ComplianceApplicationId == null)
            {
                var inspectionScheduleDetailManager = this.container.GetInstance<InspectionScheduleDetailManager>();
                return inspectionScheduleDetailManager.GetAccreditationRoleByUserId(userId, uniqueId);
            }
            else
            {
                var inspectionScheduleManager = this.container.GetInstance<InspectionScheduleManager>();

                var schedules = inspectionScheduleManager.GetAllForCompliance(application.ComplianceApplicationId.Value);

                foreach (var sched in schedules)
                {
                    var trainee =
                        sched.InspectionScheduleDetails.FirstOrDefault(
                            x =>
                                x.UserId == userId &&
                                x.AccreditationRole.Name == Constants.AccreditationsRoles.InspectorTrainees && !x.IsInspectionComplete.GetValueOrDefault());

                    if (trainee != null)
                    {
                        return trainee.AccreditationRole;
                    }

                    var detail =
                        sched.InspectionScheduleDetails.FirstOrDefault(
                            x => x.UserId == userId && x.SiteId == application.SiteId);

                    if (detail != null)
                    {
                        if (detail.AccreditationRole.Name == Constants.AccreditationsRoles.InspectorTrainees && detail.IsInspectionComplete.GetValueOrDefault())
                        {
                            return new AccreditationRole
                            {
                                Id = 1,
                                Name = Constants.AccreditationsRoles.Inspectors
                            };
                        }

                        return detail.AccreditationRole;
                    }
                }
            }

            return null;

        }

        public List<InspectionSchedule> GetSchedulesForCompliance(Guid complianceApplicationId)
        {
            var manager = this.container.GetInstance<InspectionScheduleManager>();

            return manager.GetAllForCompliance(complianceApplicationId);
        }

        public List<InspectionScheduleDetail> GetAllForCompliance(Guid complianceApplicationId, Guid userId)
        {
            var inspectionScheduleDetailManager = this.container.GetInstance<InspectionScheduleDetailManager>();
            var records = inspectionScheduleDetailManager.GetAllForCompliance(complianceApplicationId);

            return records.Where(x => x.UserId == userId).ToList();
        }

        public void SaveMentorFeedback(int inspectionScheduleDetailId, string mentorFeedback, string savedBy)
        {
            var inspectionScheduleDetailManager = this.container.GetInstance<InspectionScheduleDetailManager>();

            inspectionScheduleDetailManager.SaveMentorFeedback(inspectionScheduleDetailId, mentorFeedback, savedBy);
        }


        public async Task SaveMentorFeedbackAsync(int inspectionScheduleDetailId, string mentorFeedback, string savedBy)
        {
            var inspectionScheduleDetailManager = this.container.GetInstance<InspectionScheduleDetailManager>();

            await inspectionScheduleDetailManager.SaveMentorFeedbackAsync(inspectionScheduleDetailId, mentorFeedback, savedBy);
        }
    }
}
