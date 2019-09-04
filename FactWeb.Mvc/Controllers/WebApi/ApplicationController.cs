using FactWeb.BusinessFacade;
using FactWeb.BusinessLayer;
using FactWeb.Infrastructure;
using FactWeb.Infrastructure.Exceptions;
using FactWeb.Model;
using FactWeb.Model.InterfaceItems;
using FactWeb.Mvc.Models;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using OfficeOpenXml.Style.XmlAccess;
using SimpleInjector;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http;

namespace FactWeb.Mvc.Controllers.WebApi
{
    public class ApplicationController : BaseWebApiController<ApplicationController>
    {
        public ApplicationController(Container container) : base(container)
        {
        }

        [HttpGet]
        [MyAuthorize]
        [Route("api/Application")]
        public async Task<List<ApplicationItem>> GetApplications(string orgName)
        {
            var start = DateTime.Now;
            var applicationFacade = this.Container.GetInstance<ApplicationFacade>();

            var applications = await applicationFacade.GetApplicationsAsync(orgName);

            base.LogMessage("GetApplications", DateTime.Now - start);

            return applications.Select(x=>ModelConversions.Convert(x)).ToList();
        }

        [HttpGet]
        [MyAuthorize]
        [Route("api/Application/Simple")]
        public async Task<List<ApplicationItem>> GetSimpleApplications(string orgName)
        {
            var start = DateTime.Now;
            var applicationFacade = this.Container.GetInstance<ApplicationFacade>();

            var applications = await applicationFacade.GetApplicationsAsync(orgName);

            base.LogMessage("GetApplications", DateTime.Now - start);

            return applications.Select(x => ModelConversions.Convert(x, false, false, false, false)).ToList();
        }

        [HttpGet]
        [MyAuthorize]
        [Route("api/Application/ApplicationsByComplianceId")]
        public async Task<List<ApplicationItem>> ApplicationsByComplianceId(string complianceId)
        {
            var start = DateTime.Now;
            var applicationFacade = this.Container.GetInstance<ApplicationFacade>();

            var applications = await applicationFacade.GetApplicationsByComplianceId(complianceId);

            base.LogMessage("GetApplications", DateTime.Now - start);

            return applications.Select(x => ModelConversions.Convert(x, true)).ToList();
        }

        [HttpGet]
        [MyAuthorize]
        [Route("api/Application/Compliance/Submitted")]
        public SubmittedComplianceModel GetSubmittedComplianceApp(Guid app)
        {
            var start = DateTime.Now;
            var applicationFacade = this.Container.GetInstance<ApplicationFacade>();

            var application = applicationFacade.GetSubmittedComplainceApplication(app);

            base.LogMessage("GetSubmittedComplianceApp", DateTime.Now - start);

            return application;
        }

        [HttpGet]
        [MyAuthorize]
        [Route("api/Application/StatusView")]
        public ApplicationStatusView GetApplicationStatusView(Guid id)
        {
            var start = DateTime.Now;
            var applicationFacade = this.Container.GetInstance<ApplicationFacade>();

            var items = applicationFacade.GetApplicationStatusView(id, RoleName);

            base.LogMessage("GetApplicationStatusView", DateTime.Now - start);

            return items;
        }

        [HttpGet]
        [MyAuthorize]
        [Route("api/Application")]
        public ApplicationItem GetApplication(Guid id)
        {
            try
            {
                var start = DateTime.Now;
                var applicationFacade = this.Container.GetInstance<ApplicationFacade>();

                var application = applicationFacade.GetApplicationIgnoreActive(id);
                if (application == null) return null;

                DateTime? inspectionDate = null;

                if (application.ComplianceApplicationId.HasValue)
                {
                    var details =
                        applicationFacade.GetAllInspectionScheduleDetailsForComplianceApplication(
                            application.ComplianceApplicationId.Value);

                    if (details != null && details.Count > 0) 
                        inspectionDate = details.Select(x => x.InspectionSchedule.StartDate).Max();
                }

                var qmRestrictions = applicationFacade.HasQmRestrictions(application.OrganizationId);
                var apps = applicationFacade.GetApplicationsWithRfis(application.OrganizationId, id, Constants.ApplicationResponseStatus.RFI);

                base.LogMessage("GetApplication", DateTime.Now - start);


                var applicationItem = ModelConversions.Convert(application, false, true, true, true, inspectionDate);
                applicationItem.HasQmRestriction = qmRestrictions;
                applicationItem.ApplicationsWithRfis = apps;

                //if (application.ComplianceApplicationId.HasValue)
                //{
                //    applicationItem.InspectionDate =
                //        applicationFacade.GetInspectionDateByCompliance(application.ComplianceApplicationId.Value,
                //            application.SiteId.GetValueOrDefault());
                //}

                return applicationItem;
            }
            catch (Exception ex)
            {
                base.HandleException(ex);
                throw;
            }
        }

        [HttpGet]
        [MyAuthorize]
        [Route("api/Application/Compliance/AccreditationReport")]
        public bool ShowAccreditationReport(Guid complianceApplicationId)
        {
            try
            {
                var start = DateTime.Now;
                var applicationFacade = this.Container.GetInstance<ApplicationFacade>();

                var result = applicationFacade.ShowAccreditationReport(complianceApplicationId);

                base.LogMessage("ShowAccreditationReport", DateTime.Now - start);

                return result;
            }
            catch (Exception ex)
            {
                base.HandleException(ex);
                throw;
            }
        }

        //[HttpGet]
        //[MyAuthorize]
        //[Route("api/Application/Eligibility")]
        //public List<ApplicationSectionItem> EligibilityApplication()
        //{
        //    if (base.RoleId == (int)Constants.Role.User)
        //    {
        //        return null;
        //    }

        //    var applicationFacade = this.Container.GetInstance<ApplicationFacade>();

        //    try
        //    {
        //        return applicationFacade.BuildApplication(base.OrganizationId,
        //                    Constants.ApplicationTypes.Eligibility, UserId);
        //    }
        //    catch (Exception ex)
        //    {
        //        base.HandleException(ex);
        //        throw;
        //    }
        //}

        [HttpGet]
        [MyAuthorize]
        [Route("api/Application/Inspectors")]
        public async Task<List<InspectionScheduleDetailItems>> GetApplicationInspectors(Guid app)
        {
            var startTime = DateTime.Now;

            if (!base.IsFactStaff && base.RoleId != (int)Constants.Role.Inspector)
            {
                return null;
            }

            var applicationFacade = this.Container.GetInstance<ApplicationFacade>();

            var inspectors = await applicationFacade.GetAllInspectionScheduleDetailsForApplicationAsync(app);

            base.LogMessage("GetApplicationInspectors", DateTime.Now - startTime);

            return inspectors.Select(x=>ModelConversions.Convert(x)).ToList();
        }

        [HttpGet]
        [MyAuthorize]
        [Route("api/Application/RfiView")]
        public RfiViewItem RFIView(Guid? app, Guid? compAppId)
        {
            //if (base.RoleId == (int)Constants.Role.User)
            //{
            //    return null;
            //}

            var applicationFacade = this.Container.GetInstance<ApplicationFacade>();

            try
            {
                if (compAppId.HasValue)
                {
                    return new RfiViewItem
                    {
                        SiteApplicationSection = applicationFacade.BuildRFIComments(compAppId.Value, base.UserId.GetValueOrDefault(), true, base.IsFactStaff || base.IsReviewer)
                    };
                } else if (app.HasValue)
                {
                    return new RfiViewItem
                    {
                        SiteApplicationSection =
                            applicationFacade.BuildRFICommentsForApplication(app.Value, base.UserId.GetValueOrDefault(),
                                true, base.IsFactStaff || base.IsReviewer)
                    };
                }
                else
                {
                    return new RfiViewItem();
                }
                
            }
            catch (Exception ex)
            {
                base.HandleException(ex);
                throw;
            }
        }

        [HttpGet]
        [MyAuthorize]
        [Route("api/Application/Sections")]
        public List<ApplicationSectionItem> GetApplicationSections(string orgName, string type)
        {
            var applicationFacade = this.Container.GetInstance<ApplicationFacade>();

            try
            {
                var applicationSectionItem = applicationFacade.BuildApplication(orgName, type, this.UserId, base.IsFactStaff || base.IsReviewer);

                return applicationSectionItem;
            }
            catch (Exception ex)
            {
                base.HandleException(ex);
                throw;
            }
        }
        
        [HttpGet]
        [MyAuthorize]
        [Route("api/Application/ApplicationStatusByUniqueId")]
        public ApplicationStatusItem GetApplicationStatusByUniqueId(string appUniqueId)
        {
            var applicationFacade = this.Container.GetInstance<ApplicationFacade>();

            try
            {
                var applicationStatus = applicationFacade.GetApplicationStatusByUniqueId(new Guid(appUniqueId));

                return ModelConversions.Convert(applicationStatus);
            }
            catch (Exception ex)
            {
                base.HandleException(ex);
                throw;
            }
        }

        [HttpGet]
        [MyAuthorize]
        [Route("api/Application/Compliance/Access")]
        public string ComplianceAccessType(Guid id)
        {
            var applicationFacade = this.Container.GetInstance<ApplicationFacade>();

            try
            {
                return applicationFacade.GetComplianceAccessType(base.UserId.GetValueOrDefault(), base.IsFactStaff, id);
            }
            catch (Exception ex)
            {
                base.HandleException(ex);
                throw;
            }
        }

        [HttpGet]
        [MyAuthorize]
        [Route("api/Application/AccessType")]
        public string AccessType(Guid id)
        {
            var applicationFacade = this.Container.GetInstance<ApplicationFacade>();

            try
            {
                return applicationFacade.GetAccessType(base.UserId.GetValueOrDefault(), base.IsFactStaff, id);
            }
            catch (Exception ex)
            {
                base.HandleException(ex);
                throw;
            }
        }

        [HttpGet]
        [MyAuthorize]
        [Route("api/Application/Sections")]
        public List<ApplicationSectionItem> GetApplicationSections(Guid id)
        {
            var applicationFacade = this.Container.GetInstance<ApplicationFacade>();

            try
            {
                return applicationFacade.BuildApplication(base.UserId.GetValueOrDefault(), base.IsFactStaff, id, base.IsFactStaff || base.IsReviewer, null, null, null, null, null, null, null, null, true, null, null, base.IsFactStaff);
            }
            catch (Exception ex)
            {
                base.HandleException(ex);
                throw;
            }
        }

        [HttpGet]
        [Route("api/Application")]
        [MyAuthorize]
        public List<ApplicationItem> GetAllAsync()
        {
            var applicationFacade = this.Container.GetInstance<ApplicationFacade>();
            List<ApplicationItem> application = null;

            if (base.IsFactStaff)
                application = applicationFacade.GetAll();
            else if (base.IsConsultant)
            {
                application = applicationFacade.GetAllForUser(base.UserId.GetValueOrDefault(), false);
                application.AddRange(applicationFacade.GetAllForConsultant(base.UserId.GetValueOrDefault()));
                application = application.Distinct().ToList();
            }
            else
                application = applicationFacade.GetAllForUser(base.UserId.GetValueOrDefault(), base.IsFactStaff);
           

            return application;
        }

        [HttpGet]
        [Route("api/Application")]
        [MyAuthorize]
        public async Task<ApplicationItem> Get(string orgName, string type)
        {
            var applicationFacade = this.Container.GetInstance<ApplicationFacade>();

            var application = await applicationFacade.GetByOrgAndTypeAsync(orgName, type);// base.OrganizationId

            return ModelConversions.Convert(application);
        }

        //[HttpPut]
        //[MyAuthorize]
        //[Route("api/Application/Eligibility")]
        //public async Task<ServiceResponse> SaveEligibilityApplication(SaveAppModel model)
        //{
        //    DateTime startTime = DateTime.Now;
        //    if (base.RoleId == (int)Constants.Role.User)
        //    {
        //        throw new NotAuthorizedException();
        //    }

        //    try
        //    {
        //        var applicationFacade = this.Container.GetInstance<ApplicationFacade>();

        //        await applicationFacade.SaveApplicationAsync(model.OrgName, Constants.ApplicationTypes.Eligibility,
        //            model.Sections, base.Email, base.RoleId);

        //        base.LogMessage("SaveEligibilityApplication", DateTime.Now - startTime);

        //        return new ServiceResponse();
        //    }
        //    catch (Exception ex)
        //    {
        //        return new ServiceResponse
        //        {
        //            HasError = true,
        //            Message = ex.Message
        //        };
        //    }
        //}

        [HttpPost]
        [MyAuthorize]
        [Route("api/Application/SendToFact")]
        public ServiceResponse SendToFact(SendToFactModel model)
        {
            var startTime = DateTime.Now;
            
            try
            {
                var applicationFacade = this.Container.GetInstance<ApplicationFacade>();

                var factPortalEmailAddress = ConfigurationManager.AppSettings[Constants.ConfigurationConstants.FactPortalEmailAddress];
                var url = ConfigurationManager.AppSettings[Constants.ConfigurationConstants.Url];
                string applicationStatus = Constants.ApplicationStatus.RFIReview;

                var accreditationOutcomeFacade = this.Container.GetInstance<AccreditationOutcomeFacade>();

                base.LogMessage("GetAccreditationOutcomeByOrgId", DateTime.Now - startTime);

                var accreditationOutcomes = accreditationOutcomeFacade.GetAccreditationOutcomeByOrgId(model.OrganizationId);

                if (accreditationOutcomes.Count > 0)
                {
                    applicationStatus = Constants.ApplicationStatus.ApplicantResponseReview;
                }

                var result = applicationFacade.UpdateApplicationStatusAsync(model.ApplicationTypeId, applicationStatus, model.OrganizationId, null, string.Empty, base.Email);

                applicationFacade.SendToFact(model.App, url, factPortalEmailAddress, model.LeadName, model.OrgName, model.AppTypeName, model.CoordinatorEmail);

                base.LogMessage("SendToFact", DateTime.Now - startTime);

                return new ServiceResponse();
            }
            catch (Exception ex)
            {
                return new ServiceResponse
                {
                    HasError = true,
                    Message = ex.Message
                };
            }
        }

        [HttpPost]
        [MyAuthorize]
        [Route("api/Application/Save")]
        public ServiceResponse SaveSection(SaveSectionModel model)
        {
            DateTime startTime = DateTime.Now;

            //if (base.RoleId == (int)Constants.Role.User)
            //{
            //    throw new NotAuthorizedException();
            //}

            try
            {

                var applicationFacade = this.Container.GetInstance<ApplicationFacade>();

                applicationFacade.SaveApplicationSection(model.AppUniqueId, model.Section, base.Email, base.RoleId);

                base.LogMessage("SaveSection", DateTime.Now - startTime);

                return new ServiceResponse();
            }
            catch (Exception ex)
            {
                return new ServiceResponse
                {
                    HasError = true,
                    Message = ex.Message
                };
            }
        }

        [HttpPost]
        [MyAuthorize]
        [Route("api/Application/SaveMultiview")]
        public ServiceResponse SaveMultiviewSection(SaveMultiViewSectionModel model)
        {
            DateTime startTime = DateTime.Now;

            //if (base.RoleId == (int)Constants.Role.User)
            //{
            //    throw new NotAuthorizedException();
            //}

            try
            {
                var applicationFacade = this.Container.GetInstance<ApplicationFacade>();

                applicationFacade.SaveMultiViewSections(model.AppUniqueId, model.Sections, base.Email, base.RoleId);

                base.LogMessage("SaveMultiviewSection", DateTime.Now - startTime);

                return new ServiceResponse();
            }
            catch (Exception ex)
            {
                return new ServiceResponse
                {
                    HasError = true,
                    Message = ex.Message
                };
            }
        }

        [HttpPost]
        [MyAuthorize]
        [Route("api/Application/SaveMultiviewResponseStatus")]
        public ServiceResponse SaveMultiviewResponseStatus(SaveMultiViewSectionModel model)
        {
            DateTime startTime = DateTime.Now;

            try
            {
                var applicationFacade = this.Container.GetInstance<ApplicationFacade>();

                applicationFacade.UpdateAnswerResponseStatus(model.Sections, base.Email);

                base.LogMessage("SaveMultiviewResponseStatus", DateTime.Now - startTime);

                return new ServiceResponse();
            }
            catch (Exception ex)
            {
                return new ServiceResponse
                {
                    HasError = true,
                    Message = ex.Message
                };
            }
        }



        [HttpPost]
        [MyAuthorize]
        [Route("api/Application/UpdateAnswerResponseStatus")]
        public ServiceResponse UpdateAnswerResponseStatus(List<ApplicationSectionItem> section)
        {
            DateTime startTime = DateTime.Now;

            if (base.RoleId == (int)Constants.Role.User)
            {
                throw new NotAuthorizedException();
            }

            try
            {
                var applicationFacade = this.Container.GetInstance<ApplicationFacade>();

                applicationFacade.UpdateAnswerResponseStatus(new List<ApplicationSectionResponse>(), base.Email);

                base.LogMessage("UpdateAnswerResponseStatus", DateTime.Now - startTime);

                return new ServiceResponse();
            }
            catch (Exception ex)
            {
                return new ServiceResponse
                {
                    HasError = true,
                    Message = ex.Message
                };
            }
        }

        [HttpPost]
        [MyAuthorize]
        [Route("api/Application/UpdateSection")]
        public ServiceResponse UpdateSection(List<ApplicationSectionItem> section)
        {
            DateTime startTime = DateTime.Now;

            if (base.RoleId == (int)Constants.Role.User)
            {
                throw new NotAuthorizedException();
            }

            try
            {
                var applicationFacade = this.Container.GetInstance<ApplicationFacade>();

                applicationFacade.UpdateSection(section, base.Email);

                base.LogMessage("UpdateSection", DateTime.Now - startTime);

                return new ServiceResponse();
            }
            catch (Exception ex)
            {
                return new ServiceResponse
                {
                    HasError = true,
                    Message = ex.Message
                };
            }
        }



        [HttpPost]
        [MyAuthorize]
        [Route("api/Application/SaveSectionTrainee")]
        public ServiceResponse SaveSectionTrainee(SaveSectionModel model)
        {
            DateTime startTime = DateTime.Now;

            if (base.RoleId == (int)Constants.Role.User)
            {
                throw new NotAuthorizedException();
            }

            try
            {
                var applicationFacade = this.Container.GetInstance<ApplicationFacade>();

                applicationFacade.SaveApplicationSectionTrainee(model.AppUniqueId,
                    model.Section, base.Email, base.RoleId);

                base.LogMessage("SaveSectionTrainee", DateTime.Now - startTime);

                return new ServiceResponse();
            }
            catch (Exception ex)
            {
                return new ServiceResponse
                {
                    HasError = true,
                    Message = ex.Message
                };
            }
        }

        [HttpPost]
        [MyAuthorize]
        [Route("api/Application/Submit")]
        public async Task<ServiceResponse> Submit(string orgName, string applicationType)
        {
            DateTime startTime = DateTime.Now;

            try
            {
                var applicationFacade = this.Container.GetInstance<ApplicationFacade>();
                var url = ConfigurationManager.AppSettings[Constants.ConfigurationConstants.Url];
                var staffEmailList = ConfigurationManager.AppSettings[Constants.ConfigurationConstants.StaffEmailList];
                var factPortalEmailAddress = ConfigurationManager.AppSettings[Constants.ConfigurationConstants.FactPortalEmailAddress];

                var useAnnualApproval =
                    ConfigurationManager.AppSettings[Constants.ConfigurationConstants.UseAnnualApproval];

                //await applicationFacade.ChangeApplicationStatusAsync(base.OrganizationId,Constants.ApplicationTypes.Eligibility, Constants.ApplicationStatus.Applied, base.Email);// Appied replace by For Review

                if (applicationType == Constants.ApplicationTypes.Annual && useAnnualApproval == "Y")
                {
                    await applicationFacade.ProcessAnnualReport(url, staffEmailList, factPortalEmailAddress, orgName,
                        base.UserId.GetValueOrDefault(), base.Email);
                }
                else
                {
                    await applicationFacade.ChangeApplicationStatusAsync(url, staffEmailList, factPortalEmailAddress, orgName, applicationType, Constants.ApplicationStatus.ForReview, base.Email);
                }
                

                base.LogMessage("SubmitEligibilityApplication", DateTime.Now - startTime);

                return new ServiceResponse();
            }
            catch (Exception ex)
            {
                return new ServiceResponse
                {
                    HasError = true,
                    Message = ex.Message
                };
            }
        }

        [HttpGet]
        [Route("api/Application/Compliance/K2Submit")]
        public HttpResponseMessage SubmitComplianceFromK2(Guid app, string authKey)
        {
            if (authKey != "CSr8I7oysrU8HyvRTVjRcLgvZ3M9CzHCIXiCo1IUqmytYdIzfjf6dS38Fz2qQtm0GxpGh")
            {
                return this.Request.CreateErrorResponse(HttpStatusCode.ProxyAuthenticationRequired, "Not Authorized");
            }

            DateTime startTime = DateTime.Now;

            try
            {
                var applicationFacade = this.Container.GetInstance<ApplicationFacade>();
                var url = ConfigurationManager.AppSettings[Constants.ConfigurationConstants.Url];
                var staffEmailList = ConfigurationManager.AppSettings[Constants.ConfigurationConstants.StaffEmailList];
                var factPortalEmailAddress = ConfigurationManager.AppSettings[Constants.ConfigurationConstants.FactPortalEmailAddress];

                applicationFacade.ChangeComplainceApplicationStatus(url, staffEmailList, factPortalEmailAddress, app, Constants.ApplicationStatus.ForReview, "System", true);

                base.LogMessage("SubmitCompliance", DateTime.Now - startTime);

                return this.Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                base.HandleExceptionForResponse(ex);
                return this.Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        [HttpPost]
        [MyAuthorize]
        [Route("api/Application/Compliance/Submit")]
        public ServiceResponse SubmitCompliance(Guid app)
        {
            DateTime startTime = DateTime.Now;

            try
            {
                var applicationFacade = this.Container.GetInstance<ApplicationFacade>();
                var url = ConfigurationManager.AppSettings[Constants.ConfigurationConstants.Url];
                var staffEmailList = ConfigurationManager.AppSettings[Constants.ConfigurationConstants.StaffEmailList];
                var factPortalEmailAddress = ConfigurationManager.AppSettings[Constants.ConfigurationConstants.FactPortalEmailAddress];

                applicationFacade.ChangeComplainceApplicationStatus(url, staffEmailList, factPortalEmailAddress, app, Constants.ApplicationStatus.ForReview, base.Email, false);

                base.LogMessage("SubmitCompliance", DateTime.Now - startTime);

                return new ServiceResponse();
            }
            catch (Exception ex)
            {
                return new ServiceResponse
                {
                    HasError = true,
                    Message = ex.Message
                };
            }
        }

        [HttpPut]
        [MyAuthorize]
        [Route("api/Application/Status")]
        public async Task<ServiceResponse> UpdateApplicationStatusAsync(ApplicationItem application)
        {
            DateTime startTime = DateTime.Now;

            try
            {
                var applicationFacade = this.Container.GetInstance<ApplicationFacade>();

                await applicationFacade.UpdateApplicationStatusAsync(application.ApplicationTypeId,
                    application.ApplicationStatusName, application.OrganizationId, application.DueDate, application.Template, base.Email,
                    application.IncludeAccreditationReport, base.AccessToken);

                base.LogMessage("UpdateApplicationStatusAsync", DateTime.Now - startTime);

                return new ServiceResponse();
            }
            catch (Exception ex)
            {
                return new ServiceResponse
                {
                    HasError = true,
                    Message = ex.Message
                };
            }
        }

        [HttpGet]
        [MyAuthorize]
        [Route("api/Application/InspectionSchedules/{organizationId?}")]
        public async Task<List<InspectionScheduleItem>> InspectionSchedules(int organizationId = 0)
        {
            try
            {
                var applicationFacade = this.Container.GetInstance<ApplicationFacade>();
                List<Application> applications = null;

                if (organizationId == 0)
                {
                    applications = await applicationFacade.GetInspectionScheduleByOrgIdAppIdForInspectionSchedulerAsync(null, null);
                }
                else
                {
                    applications = await applicationFacade.GetInspectionScheduleByOrgIdAppIdForInspectionSchedulerAsync(organizationId, null);
                }

                if (applications.Count > 0 && applications[0] == null)
                    return new List<InspectionScheduleItem>();

                var items = new List<InspectionScheduleItem>();

                foreach (var app in applications)
                {
                    items.AddRange(ModelConversions.ConvertToInspectionSchedule(app));
                }

                return items;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        [HttpGet]
        [MyAuthorize]
        [Route("api/Application/Types")]
        public async Task<List<ApplicationTypeItem>> GetApplicationTypes()
        {
            DateTime startTime = DateTime.Now;
            var applicationFacade = this.Container.GetInstance<ApplicationFacade>();

            var applications = await applicationFacade.GetAllApplicationTypesAsync();

            base.LogMessage("GetApplicationTypes", DateTime.Now - startTime);

            return ModelConversions.Convert(applications.OrderBy(x => x.Name).ToList());
        }

        [HttpPost]
        [MyAuthorize]
        [Route("api/Application/Type")]
        public async Task<ServiceResponse<ApplicationTypeItem>> SaveApplicationType(ApplicationTypeItem model)
        {
            DateTime startTime = DateTime.Now;
            var applicationFacade = this.Container.GetInstance<ApplicationFacade>();

            try
            {
                var item = await applicationFacade.AddOrEditApplicationTypeAsync(model, base.Email);

                base.LogMessage("SaveApplicationType", DateTime.Now - startTime);

                return new ServiceResponse<ApplicationTypeItem>
                {

                    Item = ModelConversions.Convert(item)
                };
            }
            catch (Exception ex)
            {
                return base.HandleException<ApplicationTypeItem>(ex);
            }

        }

        [HttpGet]
        [MyAuthorize]
        [Route("api/Application/Status")]
        public async Task<List<ApplicationStatusItem>> GetApplicationStatus()
        {
            DateTime startTime = DateTime.Now;
            var applicationFacade = this.Container.GetInstance<ApplicationFacade>();

            var applicationStatus = await applicationFacade.GetApplicationStatusAsync();

            base.LogMessage("GetApplicationStatus", DateTime.Now - startTime);

            return ModelConversions.Convert(applicationStatus).ToList();
        }

        [HttpGet]
        [MyAuthorize]
        [Route("api/Application/StatusHistory")]
        public List<ApplicationStatusHistoryItem> GetApplicationStatusHistory(Guid? app, Guid? compApp)
        {
            DateTime startTime = DateTime.Now;
            var applicationFacade = this.Container.GetInstance<ApplicationFacade>();

            if (compApp.HasValue)
            {
                var applicationStatusHistory = applicationFacade.GetCompAppStatusHistory(compApp.Value);
                base.LogMessage("GetApplicationStatusHistory", DateTime.Now - startTime);
                applicationStatusHistory = applicationStatusHistory.OrderBy(x => x.CreatedDate).ToList();

                var histories = ModelConversions.Convert(applicationStatusHistory).ToList();

                var results = new List<ApplicationStatusHistoryItem>();

                foreach (var row in histories)
                {
                    if (
                        !results.Any(
                            x =>
                                x.ApplicationStatusOld.Name == row.ApplicationStatusOld.Name &&
                                x.CreatedDate == row.CreatedDate &&
                                x.ApplicationStatusNew.Name == row.ApplicationStatusNew.Name))
                    {
                        results.Add(row);
                    }
                }

                return results;
            }
            else if (app.HasValue)
            {
                var applicationStatusHistory = applicationFacade.GetApplicationStatusHistory(app.Value);
                base.LogMessage("GetApplicationStatusHistory", DateTime.Now - startTime);
                return ModelConversions.Convert(applicationStatusHistory).ToList();
            }

            return null;
        }


        [HttpGet]
        [MyAuthorize]
        [Route("api/Application/ResponseStatus")]
        public async Task<List<ApplicationResponseStatusItem>> GetAllApplicationResponseStatus()
        {
            DateTime startTime = DateTime.Now;
            var applicationFacade = this.Container.GetInstance<ApplicationFacade>();

            var applicationResponseStatuses = await applicationFacade.GetAllApplicationResponseStatusAsync();

            base.LogMessage("GetAllApplicationResponseStatus", DateTime.Now - startTime);

            return ModelConversions.Convert(applicationResponseStatuses);
        }

        [HttpGet]
        [MyAuthorize]
        [Route("api/Application/Compliance")]
        public List<ComplianceApplicationItem> GetComplianceApplications(string name)
        {
            var applicationFacade = this.Container.GetInstance<ApplicationFacade>();

            return applicationFacade.GetAllComplianceApplications(name, base.UserId.GetValueOrDefault());
        }

        //[HttpGet]
        //[MyAuthorize]
        //[Route("api/Application/Compliance")]
        //public ComplianceApplicationItem GetComplianceApplication(string name)
        //{
        //    var applicationFacade = this.Container.GetInstance<ApplicationFacade>();

        //    return applicationFacade.GetAllComplianceApplication(name);
        //}

        [HttpPost]
        [MyAuthorize]
        [Route("api/Application/Compliance/Cancel")]
        public ServiceResponse CancelComplianceApplication(Guid id)
        {
            if (!base.IsFactStaff)
            {
                return base.HandleException(new Exception("Not Authorized"));
            }

            try
            {
                var applicationFacade = this.Container.GetInstance<ApplicationFacade>();

                applicationFacade.CancelComplianceApplication(id, base.Email);

                return new ServiceResponse();
            }
            catch (Exception ex)
            {
                return base.HandleException(ex);
            }
        }

        [HttpGet]
        [Authorize]
        [Route("api/Application/Compliance")]
        public ComplianceApplicationItem GetComplianceApplicationById(Guid complianceApplicationId, string useCache)
        {
            var applicationFacade = this.Container.GetInstance<ApplicationFacade>();

            return applicationFacade.GetComplianceApplicationById(base.UserId.GetValueOrDefault(), base.IsFactStaff || base.IsConsultantCoordinator, base.RoleId.GetValueOrDefault(), base.IsUser, complianceApplicationId);
        }

        [HttpGet]
        [Authorize]
        [Route("api/Application/Compliance/Simple")]
        public CompApplication GetComplianceSections(Guid complianceApplicationId, Guid? appId)
        {
            var applicationFacade = this.Container.GetInstance<ApplicationFacade>();

            return applicationFacade.GetComplianceResponses(base.UserId.GetValueOrDefault(), base.IsFactStaff || base.IsConsultantCoordinator, base.RoleId.GetValueOrDefault(), base.IsUser, complianceApplicationId, appId);
        }

        [HttpGet]
        [Authorize]
        [Route("api/Application/Compliance/ServiceType")]
        public ComplianceApplicationItem GetComplianceApplicationByServiceType(Guid complianceApplicationId)
        {
            var facade = this.Container.GetInstance<ApplicationFacade>();

            return facade.GetComplianceApplicationSortedByServiceType(complianceApplicationId, base.IsUser);
        }

        [HttpPost]
        [MyAuthorize]
        [Route("api/Application/Compliance/Setup")]
        public ServiceResponse<Guid> SaveComplianceApplication(ComplianceApplicationItem model)
        {
            if (!base.IsFactStaff)
            {
                return base.HandleException<Guid>(new Exception("Not Authorized"));
            }

            var start = DateTime.Now;

            try
            {
                var applicationFacade = this.Container.GetInstance<ApplicationFacade>();

                var complianceApp = applicationFacade.SaveComplianceApplication(model, base.Email);

                base.LogMessage("SaveComplianceApplication", DateTime.Now - start);

                return new ServiceResponse<Guid>
                {
                    Item = complianceApp.Id
                };
            }
            catch (Exception ex)
            {
                return base.HandleException<Guid>(ex);
            }

        }

        [HttpPost]
        [MyAuthorize]
        [Route("api/Application/Compliance/Copy")]
        public ServiceResponse<ComplianceApplicationItem> CopyComplianceApplication(CopyComplianceApplicationModel model)
        {
            if (!base.IsFactStaff)
            {
                return base.HandleException<ComplianceApplicationItem>(new Exception("Not Authorized"));
            }

            var start = DateTime.Now;

            try
            {
                var applicationFacade = this.Container.GetInstance<ApplicationFacade>();

                applicationFacade.CopyComplianceApplication(model, base.Email);
                var compApp = applicationFacade.GetComplianceApplication(model.ComplianceApplicationId);

                base.LogMessage("CopyComplianceApplication", DateTime.Now - start);

                return new ServiceResponse<ComplianceApplicationItem>
                {
                    Item = compApp
                };
            }
            catch (Exception ex)
            {
                return base.HandleException<ComplianceApplicationItem>(ex);
            }
        }

        [HttpPost]
        [MyAuthorize]
        [Route("api/Application/Compliance/ApprovalStatus")]
        public ServiceResponse SetComplianceApplicationApprovalStatus(SetComplianceApplicationApprovalStatusModel model)
        {
            if (!model.ComplianceApplication.Id.HasValue)
            {
                return base.HandleException(new Exception("No Compliance Application Id found"));
            }

            

            var start = DateTime.Now;

            try
            {
                var applicationFacade = this.Container.GetInstance<ApplicationFacade>();

                if (!base.IsFactStaff)
                {
                    var hasAccess = applicationFacade.HasAccessToApplication(base.IsFactStaff, null, model.ComplianceApplication.Id, base.UserId.GetValueOrDefault());

                    if (!hasAccess)
                    {
                        throw new Exception("Not Authorized");
                    }
                }

                var url = ConfigurationManager.AppSettings[Constants.ConfigurationConstants.Url];

                applicationFacade.SetComplianceApprovalStatus(url, model.ComplianceApplication.ApprovalStatus.Name,
                    model.ComplianceApplication.Id.Value, model.ComplianceApplication.RejectionComments,
                    model.SerialNumber, base.Email, base.UserId.GetValueOrDefault());

                base.LogMessage("SetComplianceApplicationApprovalStatus", DateTime.Now - start);

                return new ServiceResponse();
            }
            catch (Exception ex)
            {
                return base.HandleException(ex);
            }
        }

        [HttpGet]
        [MyAuthorize]
        [Route("api/Application/Approvals")]
        public List<CompAppApproval> GetApplicationApprovals(Guid appUniqueId)
        {
            var start = DateTime.Now;

            var applicationFacade = this.Container.GetInstance<ApplicationFacade>();

            if (!base.IsFactStaff)
            {
                var hasAccess = applicationFacade.HasAccessToApplication(base.IsFactStaff, appUniqueId, null, base.UserId.GetValueOrDefault());

                if (!hasAccess)
                {
                    throw new Exception("Not Authorized");
                }
            }

            var approvals = applicationFacade.GetApprovals(appUniqueId);

            base.LogMessage("GetApplicationApprovals", DateTime.Now - start);

            return approvals.Select(ModelConversions.Convert).ToList();
        }

        [HttpGet]
        [MyAuthorize]
        [Route("api/Application/Compliance/Approvals")]
        public List<CompAppApproval> GetComplianceApprovals(Guid complianceApplicationId)
        {
            var start = DateTime.Now;

            var applicationFacade = this.Container.GetInstance<ApplicationFacade>();

            if (!base.IsFactStaff)
            {
                var hasAccess = applicationFacade.HasAccessToApplication(base.IsFactStaff, null, complianceApplicationId, base.UserId.GetValueOrDefault());

                if (!hasAccess)
                {
                    throw new Exception("Not Authorized");
                }
            }

            var approvals = applicationFacade.GetApprovalsByCompliance(complianceApplicationId);

            base.LogMessage("SetComplianceApplicationApprovalStatus", DateTime.Now - start);

            return approvals.Select(ModelConversions.Convert).ToList();
        }

        [HttpPost]
        [MyAuthorize]
        [Route("api/Application/Eligibility/Approve")]
        public ServiceResponse SetEligibilityApplicationApprovalStatus(ComplianceApplicationItem model)
        {
            if (!base.IsFactStaff)
            {
                return base.HandleException(new Exception("Not Authorized"));
            }

            if (!model.Id.HasValue)
            {
                return base.HandleException(new Exception("No Compliance Application Id found"));
            }

            var start = DateTime.Now;

            try
            {
                var type = Constants.ApplicationTypes.Eligibility;

                var applicationFacade = this.Container.GetInstance<ApplicationFacade>();

                var app = applicationFacade.GetByOrgAndType(model.OrganizationId, Constants.ApplicationTypes.Eligibility);

                if (app != null && app.ApplicationStatus.Name != Constants.ApplicationStatus.Complete)
                {
                    applicationFacade.UpdateApplicationStatus(app.ApplicationTypeId, 5, model.OrganizationId, base.Email);
                }
                else
                {
                    var apps = applicationFacade.GetAllByOrgAndType(model.OrganizationId, Constants.ApplicationTypes.Renewal);

                    app = apps.FirstOrDefault(x => x.ApplicationStatus.Name != Constants.ApplicationStatus.Complete);

                    type = Constants.ApplicationTypes.Renewal;

                }

                applicationFacade.SetEligibilityApplicationApprovalStatus(model, base.Email);

                var url = ConfigurationManager.AppSettings[Constants.ConfigurationConstants.Url];                
                applicationFacade.EligibilityApplicationApprovedEmail(model, url, type);

                base.LogMessage("SetEligibilityApplicationApprovalStatus", DateTime.Now - start);

                return new ServiceResponse();
            }
            catch (Exception ex)
            {
                return base.HandleException(ex);
            }
        }

        [HttpPost]
        [MyAuthorize]
        [Route("api/Application")]
        public async Task<ServiceResponse<ApplicationItem>> CreateApplication(CreateApplicationModel model)
        {
            var start = DateTime.Now;

            if (!base.IsFactStaff)
            {
                return base.HandleException<ApplicationItem>(new Exception("Not Authorized"));
            }

            try
            {
                var applicationFacade = this.Container.GetInstance<ApplicationFacade>();
                var url = ConfigurationManager.AppSettings[Constants.ConfigurationConstants.Url];
                var factPortalEmailAddress = ConfigurationManager.AppSettings[Constants.ConfigurationConstants.FactPortalEmailAddress];

                var app = await applicationFacade.CreateApplicationAsync(model.OrganizationName, model.ApplicationTypeName, model.Coordinator, url, model.DueDate, base.Email, factPortalEmailAddress);


                base.LogMessage("CreateApplication", DateTime.Now - start);

                return new ServiceResponse<ApplicationItem>
                {
                    Item = ModelConversions.Convert(app) 
                };
            }
            catch (Exception ex)
            {
                return base.HandleException<ApplicationItem>(ex);
            }
        }

        [HttpGet]
        [MyAuthorize]
        [Route("api/Application/Coordinator")]
        public List<CoordinatorApplication> GetCoordinatorApplications(string showAll)
        {
            var start = DateTime.Now;

            try
            {
                var applicationFacade = this.Container.GetInstance<ApplicationFacade>();

                var applications = applicationFacade.GetCoordinatorApplications(showAll == "Y" ? null : base.UserId);

                base.LogMessage("GetCoordinatorApplications", DateTime.Now - start);

                return applications;
            }
            catch (Exception ex)
            {
                base.HandleException(ex);
                throw;
            }
        }

        [HttpGet]
        [MyAuthorize]
        [Route("api/Application/Inspector")]
        public async Task<List<ApplicationItem>> GetInspectorApplications()
        {
            var start = DateTime.Now;

            try
            {
                var inspectionScheduleDetailManager = this.Container.GetInstance<InspectionScheduleDetailManager>();

                var applicationFacade = this.Container.GetInstance<ApplicationFacade>();

                var applications =
                    await applicationFacade.GetInspectorApplicationsAsync(base.UserId.GetValueOrDefault());

                var result = new List<ApplicationItem>();

                var compApps = new Dictionary<Guid, List<InspectionScheduleDetail>>();

                foreach (var app in applications)
                {
                    var item = ModelConversions.Convert(app);
                    item.ApplicantApplicationStatusName = item.ApplicationStatusName;

                    if (item.ComplianceApplicationId.HasValue)
                    {
                        item.ShowAccredReport =
                            applicationFacade.ShowAccreditationReport(item.ComplianceApplicationId.Value);

                        List<InspectionScheduleDetail> details = null;

                        if (compApps.ContainsKey(item.ComplianceApplicationId.Value))
                        {
                            details = compApps[item.ComplianceApplicationId.Value];
                        }
                        else
                        {
                            details = inspectionScheduleDetailManager.GetAllActiveByComplianceApplication(item.ComplianceApplicationId.Value);
                        }

                        item.IsClinical = details.Any(
                                x =>
                                    x.UserId == base.UserId.GetValueOrDefault() &&
                                    x.Site.FacilitySites.Any(
                                        y => y.Facility.ServiceType.Name == Constants.ServiceType.ClinicalProgramCT));
                    }

                    var accreditationOutcomeFacade = this.Container.GetInstance<AccreditationOutcomeFacade>();

                    var accreditationOutcome = accreditationOutcomeFacade.GetAccreditationOutcomeByAppId(app.UniqueId);

                    item.HasOutcome = accreditationOutcome.Count > 0;

                    result.Add(item);
                }


                base.LogMessage("GetInspectorApplications", DateTime.Now - start);

                return result;
            }
            catch (Exception ex)
            {
                base.HandleException(ex);
                throw;
            }
        }


        [HttpGet]
        [MyAuthorize]
        [Route("api/Application/Inspector/InspectionStatus")]
        public async Task<bool> GetInspectionCompletionStatus(Guid uniqueId)
        {
            var start = DateTime.Now;

            try
            {
                var applicationFacade = this.Container.GetInstance<ApplicationFacade>();

                var isCompleted = await applicationFacade.GetInspectionCompletionStatus(uniqueId, base.UserId.GetValueOrDefault());

                base.LogMessage("GetInspectionCompletionStatus", DateTime.Now - start);

                return isCompleted;
            }
            catch (Exception ex)
            {
                base.HandleException(ex);
                throw;
            }
        }

        [HttpPost]
        [MyAuthorize]
        [Route("api/Application/Coordinator")]
        public async Task<ServiceResponse<UserItem>> SaveApplicationCoordinator(SaveApplicationCoordinatorModel model)
        {
            var start = DateTime.Now;

            if (!base.IsFactStaff)
            {
                return base.HandleException<UserItem>(new Exception("Not Authorized"));
            }

            try
            {
                var applicationFacade = this.Container.GetInstance<ApplicationFacade>();

                var coordinator = await applicationFacade.UpdateCoordinatorAsync(model.UniqueId, model.Coordinator, model.ApplicationStatus, model.DueDate,base.Email);


                base.LogMessage("SaveApplicationCoordinator", DateTime.Now - start);

                return new ServiceResponse<UserItem>
                {
                    Item = ModelConversions.Convert(coordinator)
                };
            }
            catch (Exception ex)
            {
                return base.HandleException<UserItem>(ex);
            }
        }

        [HttpDelete]
        [MyAuthorize]
        [Route("api/Application")]
        public async Task<ServiceResponse> CancelApplication(Guid uniqueId)
        {
            var start = DateTime.Now;

            if (!base.IsFactStaff)
            {
                return base.HandleException(new Exception("Not Authorized"));
            }

            try
            {
                var applicationFacade = this.Container.GetInstance<ApplicationFacade>();

                await applicationFacade.CancelApplicationAsync(uniqueId, base.Email);


                base.LogMessage("CancelApplication", DateTime.Now - start);

                return new ServiceResponse();
            }
            catch (Exception ex)
            {
                return base.HandleException(ex);
            }
        }

        [HttpPost]
        [MyAuthorize]
        [Route("api/Application/Inspector")]
        public async Task<ServiceResponse> SetInspectorComplete(int app)
        {
            var start = DateTime.Now;

            if (!base.IsFactStaff && base.RoleId != (int)Constants.Role.Inspector)
            {
                return base.HandleException(new Exception("Not Authorized"));
            }

            try
            {
                var applicationFacade = this.Container.GetInstance<ApplicationFacade>();

                var url = ConfigurationManager.AppSettings[Constants.ConfigurationConstants.Url];

                await applicationFacade.SetInspectorCompleteAsync(app, base.UserId.GetValueOrDefault(), url, base.Email);


                base.LogMessage("SetInspectorComplete", DateTime.Now - start);

                return new ServiceResponse();
            }
            catch (Exception ex)
            {
                return base.HandleException(ex);
            }
        }

        [HttpPost]
        [MyAuthorize]
        [Route("api/Application/Inspection")]
        public async Task<ServiceResponse> SendForInspection(SendForInspectionModel model)
        {
            var start = DateTime.Now;

            if (!base.IsFactStaff)
            {
                return base.HandleException(new Exception("Not Authorized"));
            }

            try
            {
                var applicationFacade = this.Container.GetInstance<ApplicationFacade>();

                var url = ConfigurationManager.AppSettings[Constants.ConfigurationConstants.Url];
                var scheduleInspectionEmail =
                    ConfigurationManager.AppSettings[Constants.ConfigurationConstants.ScheduleInspectionEmail];

                await applicationFacade.SendComplianceToInspection(model.CompId, model.OrgName, model.CoordinatorId, model.IsCb, url, scheduleInspectionEmail, base.Email);


                base.LogMessage("SendForInspection", DateTime.Now - start);

                return new ServiceResponse();
            }
            catch (Exception ex)
            {
                return base.HandleException(ex);
            }
        }

        [HttpPost]
        [MyAuthorize]
        [Route("api/Application/SaveApplication")]
        public async Task<ServiceResponse> SaveApplication(ApplicationItem application)
        {
            var start = DateTime.Now;

            try
            {
                var applicationFacade = this.Container.GetInstance<ApplicationFacade>();

                await applicationFacade.SaveApplication(application, base.Email);

                base.LogMessage("SaveApplication", DateTime.Now - start);

                return new ServiceResponse();
            }
            catch (Exception ex)
            {
                return base.HandleException(ex);
            }
        }

        [HttpGet]
        [Route("api/Application/NotifyApplication")]
        public ServiceResponse NotifyApplicationAboutRFI(string applicantId)
        {
            DateTime startTime = DateTime.Now;

            try
            {
                var applicationFacade = this.Container.GetInstance<ApplicationFacade>();

                applicationFacade.NotifyApplicationAboutRFI(applicantId);

                base.LogMessage("NotifyApplicationAboutRFI", DateTime.Now - startTime);

                return new ServiceResponse();
            }
            catch (Exception ex)
            {
                return new ServiceResponse
                {
                    HasError = true,
                    Message = ex.Message
                };
            }
        }

        [HttpPost]
        [MyAuthorize]
        [Route("api/Application/BulkUpdateApplicationResponseStatus")]
        public ServiceResponse BulkUpdateApplicationResponseStatus(BulkUpdateAppStatusModel bulkUpdateAppStatusModel)
        {
            var start = DateTime.Now;

            try
            {
                var applicationFacade = this.Container.GetInstance<ApplicationFacade>();
                var auditLogFacade = this.Container.GetInstance<AuditLogFacade>();
                
                applicationFacade.BulkUpdateApplicationResponseStatus(bulkUpdateAppStatusModel.section, bulkUpdateAppStatusModel.fromStatus, bulkUpdateAppStatusModel.toStatus, base.Email, base.UserId.GetValueOrDefault(), new Guid(bulkUpdateAppStatusModel.appUniqueId));

                base.LogMessage("BulkUpdateApplicationResponseStatus", DateTime.Now - start);

                string logMessage = "Application Id: " + bulkUpdateAppStatusModel.appUniqueId +
                    " Organization: " + bulkUpdateAppStatusModel.organization +
                    " Application Type: " + bulkUpdateAppStatusModel.appType +
                    " From Status Id: " + bulkUpdateAppStatusModel.fromStatus.ToString() +
                    " To Status Id: " + bulkUpdateAppStatusModel.toStatus.ToString() +
                    ". Bulk Application Response Status Update done by: " + base.Email + ".";

                auditLogFacade.AddAuditLog(base.Email, base.IPAddress, logMessage);

                return new ServiceResponse()
                {
                    HasError = false
                };
            }
            catch (Exception ex)
            {
                return base.HandleException(ex);
            }
        }

        [HttpGet]
        [MyAuthorize]
        [Route("api/Application/MultiSiiteView")]
        public ServiceResponse MultiSiiteView(string appUniqueId, string siteId)
        {
            var start = DateTime.Now;

            try
            {
                var applicationFacade = this.Container.GetInstance<ApplicationFacade>();                

                applicationFacade.MultiSiiteView(appUniqueId, siteId, base.Email);

                base.LogMessage("MultiSiiteView", DateTime.Now - start);

                return new ServiceResponse()
                {
                    HasError = false
                };
            }
            catch (Exception ex)
            {
                return base.HandleException(ex);
            }
        }

        public ServiceResponse Deactivate(Guid app)
        {
            var start = DateTime.Now;

            try
            {
                var applicationFacade = this.Container.GetInstance<ApplicationFacade>();

                applicationFacade.Deactivate(app, base.Email);

                base.LogMessage("Deactivate", DateTime.Now - start);

                return new ServiceResponse()
                {
                    HasError = false
                };
            }
            catch (Exception ex)
            {
                return base.HandleException(ex);
            }
        }

        [HttpGet]
        [Route("api/Application/SetRfiFollowup")]
        public HttpResponseMessage SetRfiFollows(int organizationId, string authKey)
        {
            if (authKey != "CSr8I7oysrU8HyvRTVjRcLgvZ3M9CzHCIXiCo1IUqmytYdIzfjf6dS38Fz2qQtm0GxpGh")
            {
                return this.Request.CreateResponse(HttpStatusCode.NotFound);
            }

            try
            {

                var facade = this.Container.GetInstance<ApplicationFacade>();

                facade.ChangeRfiFollowupResponses(organizationId, base.Email);

                return this.Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return this.Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("api/Application/CbTotals")]
        public List<CbTotal> GetCbTotals(Guid id)
        {
            var start = DateTime.Now;

            var facade = this.Container.GetInstance<ApplicationFacade>();

            var result = facade.GetCbTotals(id);

            base.LogMessage("Deactivate", DateTime.Now - start);

            return result;
        }

        [HttpGet]
        [Route("api/Application/CtTotals")]
        public List<CtTotal> GetCtTotals(Guid id)
        {
            var start = DateTime.Now;

            var facade = this.Container.GetInstance<ApplicationFacade>();

            var result = facade.GetCtTotals(id);

            base.LogMessage("Deactivate", DateTime.Now - start);

            return result;
        }

        [HttpGet]
        [Route("api/Application/ComplianceApplication/CompInspectionDetail")]
        public CompAppInspectionDetail GetCompAppInspectionDetail(Guid id)
        {
            var start = DateTime.Now;

            var facade = this.Container.GetInstance<ApplicationFacade>();

            var result = facade.GetComplianceApplicationInspectionDetails(id);

            base.LogMessage("GetCompAppInspectionDetail", DateTime.Now - start);

            return ModelConversions.Convert(result);
        }

        [HttpPost]
        [Route("api/Application/ComplianceApplication/CompInspectionDetail")]
        public ServiceResponse<CompAppInspectionDetail> SaveCompAppInspectionDetail(CompAppInspectionDetail model)
        {
            var start = DateTime.Now;

            try
            {
                var facade = this.Container.GetInstance<ApplicationFacade>();

                var record = facade.SaveCompAppInspectionDetail(model, base.Email);

                base.LogMessage("SaveCompAppInspectionDetail", DateTime.Now - start);

                return new ServiceResponse<CompAppInspectionDetail>
                {
                    Item = ModelConversions.Convert(record)
                };
            }
            catch (Exception ex)
            {
                return base.HandleException<CompAppInspectionDetail>(ex);
            }
        }

        [HttpGet]
        [Route("api/Application/Report")]
        public List<AppReportModel> GetApplicationReport(Guid id, string siteName)
        {
            var start = DateTime.Now;

            var facade = this.Container.GetInstance<ApplicationFacade>();

            var result = facade.GetApplicationReport(id, siteName);

            base.LogMessage("GetApplicationReport", DateTime.Now - start);

            return result;

        }

        [HttpGet]
        [Route("api/Application/Report")]
        public List<AppReportModel> GetAppReport(Guid appId)
        {
            var start = DateTime.Now;

            var facade = this.Container.GetInstance<ApplicationFacade>();

            var result = facade.GetAppReport(appId);

            base.LogMessage("GetAppReport", DateTime.Now - start);

            return result;

        }

        [HttpGet]
        [Route("api/Application/Report/Export")]
        public HttpResponseMessage GetBlankReport(int type)
        {
            try
            {
                var result = new HttpResponseMessage(HttpStatusCode.OK);

                var facade = this.Container.GetInstance<ApplicationFacade>();

                var applicationType = facade.GetApplicationType(type);

                List<BlankReport> report = facade.GetBlankReport(type);
                var fileName = applicationType.Name + ".xlsx";


                var stream = new MemoryStream();
                //create a package 
                using (var package = new ExcelPackage(stream)) // disposing ExcelPackage also disposes the above MemoryStream
                {
                    var ws = package.Workbook.Worksheets.Add("Site");
                    ws.PrinterSettings.TopMargin = .3M;
                    ws.PrinterSettings.HeaderMargin = .25M;
                    ws.PrinterSettings.LeftMargin = .25M;
                    ws.PrinterSettings.RightMargin = .25M;
                    ws.PrinterSettings.BottomMargin = .3M;
                    ws.PrinterSettings.FooterMargin = .25M;
                    ws.PrinterSettings.Orientation = eOrientation.Landscape;
                    ws.PrinterSettings.FitToHeight = 0;
                    ws.PrinterSettings.FitToWidth = 1;
                    ws.PrinterSettings.FitToPage = true;
                    ws.PrinterSettings.ShowHeaders = true;
                    ws.HeaderFooter.OddHeader.CenteredText = fileName;
                    ws.HeaderFooter.OddFooter.RightAlignedText = string.Format("Page {0} of {1}", ExcelHeaderFooter.PageNumber, ExcelHeaderFooter.NumberOfPages);
                    ws.PrinterSettings.RepeatRows = ws.Cells["4:5"];
                    //ws.PrinterSettings.RepeatColumns = ws.Cells["A:E"];
                    ws.PrinterSettings.ShowGridLines = true;
                    ws.PrinterSettings.ShowHeaders = false;



                    ws.Column(1).Width = 9;
                    ws.Column(2).Width = 75;
                    ws.Column(3).Width = 75;
                    ws.Column(4).Width = 38;
                    ws.Column(5).Width = 38;

                    var siteStyle = ws.Workbook.Styles.CreateNamedStyle("site_style");
                    siteStyle.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    siteStyle.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(0, 146, 159));
                    siteStyle.Style.Font.Color.SetColor(Color.FromArgb(255, 255, 255));
                    siteStyle.Style.Font.Bold = true;

                    var appStyle = ws.Workbook.Styles.CreateNamedStyle("app_style");
                    appStyle.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    appStyle.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(97, 175, 175));
                    appStyle.Style.Font.Color.SetColor(Color.FromArgb(255, 255, 255));
                    appStyle.Style.Font.Bold = true;

                    var headerStyle = ws.Workbook.Styles.CreateNamedStyle("header_style");
                    headerStyle.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    headerStyle.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(0, 102, 164));
                    headerStyle.Style.Font.Color.SetColor(Color.FromArgb(255, 255, 255));
                    headerStyle.Style.Font.Bold = true;

                    var i = 1;

                    foreach (var row in report)
                    {
                        switch (row.RowType)
                        {
                            case "AppType":
                                ws.Cells[$"A{i}:E{i}"].Merge = true;
                                this.SetCell(ws, i, 1, row.Name, appStyle);
                                i++;
                                this.SetCell(ws, i, 1, "Standard", headerStyle);
                                this.SetCell(ws, i, 2, "Question", headerStyle);
                                this.SetCell(ws, i, 3, "Response", headerStyle);
                                this.SetCell(ws, i, 4, "Applicant Comment", headerStyle);
                                this.SetCell(ws, i, 5, "Inspector Comment", headerStyle);
                                break;
                            case "Section":
                                ws.Cells[$"A{i}:E{i}"].Merge = true;
                                this.SetCell(ws, i, 1, row.Name);
                                ws.Cells[i, 1].Style.WrapText = true;
                                ws.Row(i).Height = this.MeasureTextHeight(row.Name, ws.Cells[i, 1].Style.Font, 325) + 5;
                                break;
                            case "Question":
                                this.SetCell(ws, i, 2, row.Name);
                                this.SetCell(ws, i, 3, "");
                                this.SetCell(ws, i, 4, "");
                                break;
                        }
                        i++;
                    }

                    //ws.Cells.AutoFitColumns();

                    package.Save();

                    stream.Position = 0;

                    result.Content = new StreamContent(stream);
                }

                result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
                result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
                {
                    FileName = fileName
                };
                return result;
            }
            catch (Exception ex)
            {
                base.HandleExceptionForResponse(ex);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        [HttpGet]
        [Route("api/Application/Report/Export")]
        public HttpResponseMessage ExportXls(Guid? uniqueId, Guid? id, string siteName)
        {
            try
            {
                var result = new HttpResponseMessage(HttpStatusCode.OK);
                
                var facade = this.Container.GetInstance<ApplicationFacade>();

                List<AppReportModel> report = null;
                var fileName = string.Empty;

                if (uniqueId.HasValue && !id.HasValue)
                {
                    report = facade.GetAppReport(uniqueId.Value);
                    fileName = $"{report[0].Text} - {report[1].Text}.xlsx";
                }
                else
                {
                    report = facade.GetApplicationReport(id.Value, siteName);
                    fileName = $"{siteName.Replace("\"", "").Replace(":", "")}.xlsx";
                }
                

                var stream = new MemoryStream();
                //create a package 
                using (var package = new ExcelPackage(stream)) // disposing ExcelPackage also disposes the above MemoryStream
                {
                    var ws = package.Workbook.Worksheets.Add("Site");
                    ws.PrinterSettings.TopMargin = .3M;
                    ws.PrinterSettings.HeaderMargin = .25M;
                    ws.PrinterSettings.LeftMargin = .25M;
                    ws.PrinterSettings.RightMargin = .25M;
                    ws.PrinterSettings.BottomMargin = .3M;
                    ws.PrinterSettings.FooterMargin = .25M;
                    ws.PrinterSettings.Orientation = eOrientation.Landscape;
                    ws.PrinterSettings.FitToHeight = 0;
                    ws.PrinterSettings.FitToWidth = 1;
                    ws.PrinterSettings.FitToPage = true;
                    ws.PrinterSettings.ShowHeaders = true;
                    ws.HeaderFooter.OddHeader.CenteredText = fileName;
                    ws.HeaderFooter.OddFooter.RightAlignedText = string.Format("Page {0} of {1}", ExcelHeaderFooter.PageNumber, ExcelHeaderFooter.NumberOfPages);
                    ws.PrinterSettings.RepeatRows = uniqueId.HasValue && !id.HasValue ? ws.Cells["3:3"] : ws.Cells["4:4"];
                    //ws.PrinterSettings.RepeatColumns = ws.Cells["A:E"];
                    ws.PrinterSettings.ShowGridLines = true;
                    ws.PrinterSettings.ShowHeaders = false;
                    


                    ws.Column(1).Width = 9;
                    ws.Column(2).Width = 75;
                    ws.Column(3).Width = 75;
                    ws.Column(4).Width = 38;
                    ws.Column(5).Width = 38;

                    var siteStyle = ws.Workbook.Styles.CreateNamedStyle("site_style");
                    siteStyle.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    siteStyle.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(0, 146, 159));
                    siteStyle.Style.Font.Color.SetColor(Color.FromArgb(255, 255, 255));
                    siteStyle.Style.Font.Bold = true;

                    var appStyle = ws.Workbook.Styles.CreateNamedStyle("app_style");
                    appStyle.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    appStyle.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(97, 175, 175));
                    appStyle.Style.Font.Color.SetColor(Color.FromArgb(255, 255, 255));
                    appStyle.Style.Font.Bold = true;

                    var headerStyle = ws.Workbook.Styles.CreateNamedStyle("header_style");
                    headerStyle.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    headerStyle.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(0, 102, 164));
                    headerStyle.Style.Font.Color.SetColor(Color.FromArgb(255, 255, 255));
                    headerStyle.Style.Font.Bold = true;

                    var i = 1;

                    foreach (var row in report)
                    {
                        switch (row.Type)
                        {
                            case "Org":
                                if (uniqueId.HasValue)
                                {
                                    ws.Cells[$"A{i}:E{i}"].Merge = true;
                                    this.SetCell(ws, i, 1, row.Text, siteStyle);
                                }
                                break;
                            case "Site":
                                ws.Cells[$"A{i}:E{i}"].Merge = true;
                                this.SetCell(ws, i, 1, row.Text, siteStyle);
                                break;
                            case "App":
                                ws.Cells[$"A{i}:E{i}"].Merge = true;
                                this.SetCell(ws, i, 1, row.Text, appStyle);
                                i++;
                                this.SetCell(ws, i, 1, "Standard", headerStyle);
                                this.SetCell(ws, i, 2, "Question", headerStyle);
                                this.SetCell(ws, i, 3, "Response", headerStyle);
                                this.SetCell(ws, i, 4, "Applicant Comment", headerStyle);
                                this.SetCell(ws, i, 5, "Inspector Comment", headerStyle);
                                break;
                            case "Req":
                                ws.Cells[$"A{i}:E{i}"].Merge = true;
                                this.SetCell(ws, i, 1, row.Text);
                                ws.Cells[i, 1].Style.WrapText = true;
                                ws.Row(i).Height = this.MeasureTextHeight(row.Text, ws.Cells[i, 1].Style.Font, 325) + 5;
                                break;
                            case "Res":
                                this.SetCell(ws, i, 2, row.Text);
                                this.SetCell(ws, i, 3, row.Response);
                                this.SetCell(ws, i, 4, row.Comments);
                                break;
                        }
                        i++;
                    }

                    //ws.Cells.AutoFitColumns();

                    package.Save();

                    stream.Position = 0;

                    result.Content = new StreamContent(stream);
                }

                result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
                result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
                {
                    FileName = fileName
                };
                return result;
            }
            catch (Exception ex)
            {
                base.HandleExceptionForResponse(ex);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        private double MeasureTextHeight(string text, ExcelFont font, int width)
        {
            if (string.IsNullOrEmpty(text)) return 0.0;
            var bitmap = new Bitmap(1, 1);
            var graphics = Graphics.FromImage(bitmap);

            var pixelWidth = Convert.ToInt32(width * 7.5);  //7.5 pixels per excel column width
            var drawingFont = new Font(font.Name, font.Size);
            var size = graphics.MeasureString(text, drawingFont, pixelWidth);

            //72 DPI and 96 points per inch.  Excel height in points with max of 409 per Excel requirements.
            return Math.Min(Convert.ToDouble(size.Height) * 72 / 96, 409);
        }

        private string ConvertHtml(string text)
        {
            if (!string.IsNullOrEmpty(text) && text.Contains("<ul>"))
            {
                if (text.IndexOf("<ul>") == 0)
                {
                    text = text.Replace("<ul>", "");
                }
                else
                {
                    text = text.Replace("<ul>", "\r\n");
                }

                text = text.Replace("<li>", "");
                text = text.Replace("</li>", "\r\n");
                text = text.Replace("</ul>", "");
            }

            text = text.Replace("<p>", "");
            text = text.Replace("</p>", "\r\n\r\n");

            text = text.Replace("&nbsp;", " ");

            return text;
        }


        private void SetCell(ExcelWorksheet ws, int row, int col, string text, ExcelNamedStyleXml style = null)
        {
            ws.Cells[row, col].Value = this.ConvertHtml(text);

            if (style != null)
                ws.Cells[row, col].StyleName = style.Name;

            ws.Cells[row, col].Style.WrapText = true;
            ws.Cells[row, col].Style.VerticalAlignment = ExcelVerticalAlignment.Top;
        }

        [HttpGet]
        [Route("api/Application/CompApp/HasRfis")]
        public bool CompApplicationHasRfis(Guid id)
        {
            var facade = this.Container.GetInstance<ApplicationFacade>();

            return facade.AppHasRfis(id);
        }
    }
}

