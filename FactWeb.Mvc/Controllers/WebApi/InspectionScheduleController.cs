using FactWeb.BusinessFacade;
using FactWeb.Infrastructure;
using FactWeb.Model.InterfaceItems;
using FactWeb.Mvc.Models;
using SimpleInjector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;


namespace FactWeb.Mvc.Controllers.WebApi
{
    public class InspectionScheduleController : BaseWebApiController<InspectionScheduleController>
    {
        public InspectionScheduleController(Container container) : base(container)
        {
        }

        [HttpGet]
        [MyAuthorize]
        public ServiceResponse<InspectionScheduleDetailPageItems> GetInspectionScheduleDetail(int organizationId, int applicationId, int inspectionScheduleId)
        {
            DateTime startTime = DateTime.Now;

            try
            {
                var inspectionScheduleFacade = this.Container.GetInstance<InspectionScheduleFacade>();
                var userFacade = this.Container.GetInstance<UserFacade>();

                var inspectionScheduleDetailPageItems = new InspectionScheduleDetailPageItems
                {
                    FacilitySites = new List<FacilitySiteItems>()
                };

                var inspectionSchedules = inspectionScheduleFacade.GetInspectionScheduleByAppIdOrganizationID(organizationId, applicationId, true).OrderByDescending(x => x.CompletionDate).ToList();
                var archiveExist = inspectionSchedules.Count() > 0;

                var siteFacade = this.Container.GetInstance<SiteFacade>();
                var sites = siteFacade.GetSitesByComplianceAppId(applicationId);

                foreach (var site in sites)
                {
                    var rec = new FacilitySiteItems
                    {
                        SiteId = site.Id,
                        SiteName = site.Name,
                    };

                    

                    if (inspectionScheduleId != 0)
                    {
                        var schedSite =
                            site.InspectionScheduleSites.FirstOrDefault(x => x.InspectionScheduleId == inspectionScheduleId);

                        if (schedSite != null)
                        {
                            rec.InspectionDate = schedSite.InspectionDate.ToShortDateString();
                        }
                    }

                    inspectionScheduleDetailPageItems.FacilitySites.Add(rec);
                }


                if (inspectionScheduleId != 0) // edit Schedule
                {
                    var inspectionSchedule = inspectionScheduleFacade.GetInspectionScheduleById(inspectionScheduleId);
                    var inspectionScheduleDetailList = inspectionScheduleFacade.GetActiveByInspectionScheduleId(inspectionScheduleId);

                    inspectionScheduleDetailPageItems.InspectionScheduleDetailItems = ModelConversions.Convert(inspectionScheduleDetailList);
                    inspectionScheduleDetailPageItems.InspectionScheduleId = inspectionScheduleId;
                    inspectionScheduleDetailPageItems.InspectionDate = inspectionSchedule.InspectionDate.ToString();
                    inspectionScheduleDetailPageItems.ApplcattionId = inspectionSchedule.ApplicationId;
                    inspectionScheduleDetailPageItems.ApplicationTypeId = inspectionSchedule.Applications.ApplicationTypeId;
                    inspectionScheduleDetailPageItems.ArchiveExist = archiveExist;
                }
                else // new record
                {
                    if (!archiveExist)
                    {
                        inspectionScheduleDetailPageItems.InspectionScheduleDetailItems = new List<InspectionScheduleDetailItems>();
                        inspectionScheduleDetailPageItems.ArchiveExist = archiveExist;
                    }
                    else
                    {
                        var inspectionScheduleNew = inspectionScheduleFacade.CloneInspectionSchedule(inspectionSchedules[0].Id, Email);

                        var inspectionSchedule = inspectionScheduleFacade.GetInspectionScheduleById(inspectionScheduleNew.Id);
                        var inspectionScheduleDetailList = inspectionScheduleFacade.GetActiveByInspectionScheduleId(inspectionScheduleNew.Id);

                        inspectionScheduleDetailPageItems.InspectionScheduleDetailItems = ModelConversions.Convert(inspectionScheduleDetailList);
                        inspectionScheduleDetailPageItems.InspectionScheduleId = inspectionSchedule.Id;
                        inspectionScheduleDetailPageItems.InspectionDate = inspectionSchedule.InspectionDate.ToString();
                        inspectionScheduleDetailPageItems.ApplcattionId = inspectionSchedule.ApplicationId;
                        inspectionScheduleDetailPageItems.ApplicationTypeId = inspectionSchedule.Applications.ApplicationTypeId;
                        inspectionScheduleDetailPageItems.ArchiveExist = archiveExist;

                    }

                }

                base.LogMessage("GetInspectionScheduleDetail", DateTime.Now - startTime);

                return new ServiceResponse<InspectionScheduleDetailPageItems>
                {
                    Item = inspectionScheduleDetailPageItems
                };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<InspectionScheduleDetailPageItems>
                {
                    HasError = true,
                    Message = ex.Message
                };
            }
        }

        [HttpGet]
        [MyAuthorize]
        public ServiceResponse<InspectionScheduleItem> GetInspectionSchedule(int organizationId, int applicationId)
        {
            DateTime startTime = DateTime.Now;

            try
            {
                var inspectionScheduleFacade = this.Container.GetInstance<InspectionScheduleFacade>();
                var inspectionSchedule = inspectionScheduleFacade.GetInspectionSchedule(organizationId, applicationId);

                base.LogMessage("GetInspectionSchedule", DateTime.Now - startTime);

                return new ServiceResponse<InspectionScheduleItem>
                {
                    Item = ModelConversions.Convert(inspectionSchedule)
                };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<InspectionScheduleItem>
                {
                    HasError = true,
                    Message = ex.Message
                };
            }
        }

        [HttpGet]
        [MyAuthorize]
        public List<InspectionScheduleItem> GetAllInspectionSchedules(int organizationId, int applicationId)
        {
            DateTime startTime = DateTime.Now;

            try
            {
                var inspectionScheduleFacade = this.Container.GetInstance<InspectionScheduleFacade>();
                var inspectionScheduleList = inspectionScheduleFacade.GetInspectionScheduleByAppIdOrganizationID(organizationId, applicationId, null);

                base.LogMessage("GetAllInspectionSchedules", DateTime.Now - startTime);

                var result = ModelConversions.Convert(inspectionScheduleList.OrderByDescending(x => x.UpdatedDate).ToList());

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        [MyAuthorize]
        [Route("api/InspectionSchedule/GetAllForCompApp")]
        public List<InspectionScheduleItem> GetAllForCompApp(Guid id)
        {
            var inspectionScheduleFacade = this.Container.GetInstance<InspectionScheduleFacade>();

            var records = inspectionScheduleFacade.GetSchedulesForCompliance(id);

            return records.Select(ModelConversions.Convert).ToList();
        }

        [HttpPost]
        [MyAuthorize]
        public ServiceResponse<int> SaveInspectionSchedule(InspectionScheduleDetailModels inspectionScheduleDetailModels)
        {
            DateTime startTime = DateTime.Now;
            var inspectionScheduleFacade = this.Container.GetInstance<InspectionScheduleFacade>();

            int inspectionScheduleId = inspectionScheduleFacade.SaveInspectionSchedule(
                inspectionScheduleDetailModels.InspectionScheduleId,
                inspectionScheduleDetailModels.InspectionScheduleDetailId,
                inspectionScheduleDetailModels.OrganizationId,
                inspectionScheduleDetailModels.ApplicationId,
                inspectionScheduleDetailModels.SelectedUserId,
                inspectionScheduleDetailModels.SelectedRoleId,
                inspectionScheduleDetailModels.SelectedCategoryId,
                inspectionScheduleDetailModels.Lead,
                inspectionScheduleDetailModels.Mentor,
                inspectionScheduleDetailModels.StartDate,
                inspectionScheduleDetailModels.EndDate,
                inspectionScheduleDetailModels.SelectedSiteList,
                Email,
                inspectionScheduleDetailModels.SelectedSiteId
                );

            base.LogMessage("SaveInspectionSchedule", DateTime.Now - startTime);

            return new ServiceResponse<int>
            {
                Item = inspectionScheduleId
            };
        }

        [HttpPost]
        [MyAuthorize]
        public ServiceResponse<bool> DeleteStaff(InspectionScheduleDetailItems inspectionScheduleDetailItem)
        {
            DateTime startTime = DateTime.Now;

            try
            {
                var inspectionScheduleFacade = this.Container.GetInstance<InspectionScheduleFacade>();
                var result = inspectionScheduleFacade.DeleteStaff(inspectionScheduleDetailItem.InspectionScheduleDetailId);

                base.LogMessage("DeleteStaff", DateTime.Now - startTime);

                return new ServiceResponse<bool>
                {
                    Item = result
                };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<bool>
                {
                    HasError = true,
                    Message = ex.Message
                };
            }
        }

        [HttpPost]
        [MyAuthorize]
        public async Task<ServiceResponse<bool>> DeleteStaffAsync(InspectionScheduleDetailItems inspectionScheduleDetailItem)
        {
            DateTime startTime = DateTime.Now;
            try
            {
                var inspectionScheduleFacade = this.Container.GetInstance<InspectionScheduleFacade>();
                var result = inspectionScheduleFacade.DeleteStaffAsync(inspectionScheduleDetailItem.InspectionScheduleDetailId);

                base.LogMessage("DeleteStaffAsync", DateTime.Now - startTime);

                return new ServiceResponse<bool>
                {
                    Item = await result
                };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<bool>
                {
                    HasError = true,
                    Message = ex.Message
                };
            }
        }

        [HttpPost]
        [MyAuthorize]
        public ServiceResponse<bool> DeleteSchedule(InspectionScheduleItem inspectionScheduleItem)
        {
            DateTime startTime = DateTime.Now;
            try
            {
                var inspectionScheduleFacade = this.Container.GetInstance<InspectionScheduleFacade>();
                var result = inspectionScheduleFacade.DeleteSchedule(Convert.ToInt32(inspectionScheduleItem.InspectionScheduleId));

                base.LogMessage("DeleteSchedule", DateTime.Now - startTime);

                return new ServiceResponse<bool>
                {
                    Item = result
                };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<bool>
                {
                    HasError = true,
                    Message = ex.Message
                };
            }
        }

        [HttpPost]
        [MyAuthorize]
        public async Task<ServiceResponse<bool>> DeleteScheduleAsync(InspectionScheduleItem inspectionScheduleItem)
        {
            DateTime startTime = DateTime.Now;
            try
            {
                var inspectionScheduleFacade = this.Container.GetInstance<InspectionScheduleFacade>();
                var result = await inspectionScheduleFacade.DeleteScheduleAsync(Convert.ToInt32(inspectionScheduleItem.InspectionScheduleId));

                base.LogMessage("DeleteScheduleAsync", DateTime.Now - startTime);

                return new ServiceResponse<bool>
                {
                    Item = result
                };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<bool>
                {
                    HasError = true,
                    Message = ex.Message
                };
            }
        }

        [HttpGet]
        [MyAuthorize]
        [Route("api/InspectionSchedule/GetInspectionCategories")]
        public async Task<List<InspectionCategoryItem>> GetInspectionCategories()
        {
            DateTime startTime = DateTime.Now;
            var inspectionScheduleFacade = this.Container.GetInstance<InspectionScheduleFacade>();

            var inspectionScheduleList = await inspectionScheduleFacade.GetAllInspectionCategoriesAsync();

            base.LogMessage("GetInspectionCategories", DateTime.Now - startTime);
            return ModelConversions.Convert(inspectionScheduleList);
        }

        [HttpGet]
        [MyAuthorize]
        public async Task<List<InspectionCategoryItem>> GetInspectionCategories(int applicationId)
        {
            DateTime startTime = DateTime.Now;
            var inspectionScheduleFacade = this.Container.GetInstance<InspectionScheduleFacade>();

            var inspectionScheduleList = await inspectionScheduleFacade.GetAllInspectionCategoriesAsync(applicationId);

            base.LogMessage("GetInspectionCategories", DateTime.Now - startTime);
            return ModelConversions.Convert(inspectionScheduleList);
        }

        [HttpGet]
        [MyAuthorize]
        public async Task<List<FacilitySiteItems>> GetSitesAsync(int organizationId, int inspectionScheduleId)
        {
            DateTime startTime = DateTime.Now;
            var organizationFacilityFacade = this.Container.GetInstance<OrganizationFacilityFacade>();
            var inspectionScheduleFacade = this.Container.GetInstance<InspectionScheduleFacade>();

            var organizationFacilityList = await organizationFacilityFacade.SearchAsync(organizationId, null);
            base.LogMessage("GetSitesAsync", DateTime.Now - startTime);
            return ModelConversions.ConvertToFacilitySites(organizationFacilityList);
        }

        [HttpGet]
        [MyAuthorize]
        [Route("api/InspectionSchedule/AccreditationRoleByUserId")]
        public ServiceResponse<AccreditationRoleItem> GetAccreditationRoleByUserIdAsync(Guid? userId, Guid uniqueId)
        {
            DateTime startTime = DateTime.Now;
            try
            {
                var inspectionScheduleFacade = this.Container.GetInstance<InspectionScheduleFacade>();

                var accreditationRole = inspectionScheduleFacade.GetAccreditationRoleByUserId(userId ?? base.UserId.GetValueOrDefault(), uniqueId);
                base.LogMessage("GetAccreditationRoleByUserIdAsync", DateTime.Now - startTime);

                return new ServiceResponse<AccreditationRoleItem>
                {
                    Item = ModelConversions.Convert(accreditationRole)
                };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<AccreditationRoleItem>
                {
                    HasError = true,
                    Message = ex.Message
                };
            }

        }

        [HttpGet]
        [MyAuthorize]
        [Route("api/InspectionSchedule/Compliance")]
        public List<InspectionScheduleDetailItems> GetForCompliance(Guid appId)
        {
            var startTime = DateTime.Now;
            var inspectionScheduleFacade = this.Container.GetInstance<InspectionScheduleFacade>();

            var items = inspectionScheduleFacade.GetAllForCompliance(appId, base.UserId.GetValueOrDefault());
            base.LogMessage("GetForCompliance", DateTime.Now - startTime);

            return items.Select(x=>ModelConversions.Convert(x)).ToList();
        }
    }
}
