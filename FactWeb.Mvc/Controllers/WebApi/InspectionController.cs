using FactWeb.BusinessFacade;
using FactWeb.BusinessLayer;
using FactWeb.Infrastructure;
using FactWeb.Model;
using FactWeb.Model.InterfaceItems;
using FactWeb.Mvc.Models;
using SimpleInjector;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace FactWeb.Mvc.Controllers.WebApi
{
    public class InspectionController : BaseWebApiController<InspectionController>
    {
        public InspectionController(Container container) : base(container)
        {
        }

        /// <summary>
        /// Saves Inspection
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [MyAuthorize]
        [Route("api/Inspection")]
        public async Task<ServiceResponse<bool>> SaveAsync(InspectionItem inspectionItem)
        {
            DateTime startTime = DateTime.Now;

            try
            {
                var inspectionFacade = this.Container.GetInstance<InspectionFacade>();

                base.LogMessage("SaveAsync", DateTime.Now - startTime);

                return new ServiceResponse<bool>
                {
                    Item = await inspectionFacade.SaveAsync(inspectionItem, base.UserId.GetValueOrDefault(), base.Email),
                    Message = "Inspection saved successfully"
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
        [Route("api/Inspection/Coordinator")]
        public ServiceResponse SaveCoordinatorAsync(InspectionDetail inspectionItem)
        {
            DateTime startTime = DateTime.Now;

            try
            {
                var inspectionFacade = this.Container.GetInstance<InspectionFacade>();

                base.LogMessage("SaveAsync", DateTime.Now - startTime);

                inspectionFacade.SaveCoordinator(inspectionItem, Email);

                return new ServiceResponse();
            }
            catch (Exception ex)
            {
                return base.HandleException(ex);
            }

        }

        [HttpPost]
        [MyAuthorize]
        [Route("api/Inspection/Detail")]
        public ServiceResponse SaveInspectionDetail(InspectionOverallDetail model)
        {
            DateTime startTime = DateTime.Now;

            try
            {
                var inspectionFacade = this.Container.GetInstance<InspectionFacade>();

                base.LogMessage("SaveAsync", DateTime.Now - startTime);

                inspectionFacade.UpdateDetails(model, Email);

                return new ServiceResponse();
            }
            catch (Exception ex)
            {
                return base.HandleException(ex);
            }

        }

        [HttpGet]
        [MyAuthorize]
        [Route("api/Inspection")]
        public async Task<List<InspectionItem>> GetInspection(Guid app)
        {
            var startTime = DateTime.Now;

            var inspectionFacade = this.Container.GetInstance<InspectionFacade>();

            base.LogMessage("GetInspection", DateTime.Now - startTime);

            var inspection = await inspectionFacade.GetInspectionAsync(app, base.UserId.GetValueOrDefault());

            var result = new List<InspectionItem>();

            for (var i = 0; i < inspection.Count; i++)
            {
                if (inspection[i] == null) continue;
                
                var item = ModelConversions.Convert(inspection[i]);

                item.IsReinspection = i == 0;

                result.Add(item);
            }

            return result;
        }

        [HttpPost]
        [MyAuthorize]
        [Route("api/Inspection/Outcome")]
        public ServiceResponse SetReviewedOutcome(Guid compAppId)
        {
            if (base.RoleId.GetValueOrDefault() != (int)Constants.Role.Inspector)
            {
                return new ServiceResponse
                {
                    HasError = true,
                    Message = "Not Authorized"
                };
            }

            var startTime = DateTime.Now;

            try
            {
                var inspectionFacade = this.Container.GetInstance<InspectionFacade>();


                inspectionFacade.SetReviewOutcome(compAppId, base.UserId.GetValueOrDefault(), base.Email);
                base.LogMessage("SetReviewedOutcome", DateTime.Now - startTime);

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
        [Route("api/Inspection/Details")]
        public InspectionOverallDetail GetInspectionDetails(Guid compAppId)
        {
            if (!base.IsFactStaff)
            {
                return null;
            }

            try
            {
                var inspectionFacade = this.Container.GetInstance<InspectionFacade>();

                return inspectionFacade.GetInspectionDetails(compAppId);
            }
            catch (Exception ex)
            {
                base.HandleExceptionForResponse(ex);
                throw;
            }
        }

        [HttpGet]
        [MyAuthorize]
        [Route("api/Inspection")]
        public async Task<List<InspectionItem>> GetInspectionBySite(Guid app, string site)
        {
            var startTime = DateTime.Now;

            var inspectionFacade = this.Container.GetInstance<InspectionFacade>();

            base.LogMessage("GetInspectionBySite", DateTime.Now - startTime);

            var inspection = await inspectionFacade.GetInspectionBySiteAsync(app, site, base.UserId.GetValueOrDefault());

            var result = new List<InspectionItem>();

            for (var i = 0; i < inspection.Count; i++)
            {
                if (inspection[i] == null) continue;

                var item = ModelConversions.Convert(inspection[i]);

                item.IsReinspection = i == 0;

                result.Add(item);
            }

            return result;

        }

        [HttpGet]
        [MyAuthorize]
        [Route("api/Inspection/Inspectors")]
        public List<InspectionScheduleDetailItems> GetComplianceApplicationInspectors(Guid app, string includeOthers)
        {
            var startTime = DateTime.Now;

            //Nick: Dont think we need/want this
            //if (!base.IsFactStaff && base.RoleId != (int)Constants.Role.Inspector && base.RoleName != Constants.Roles.QualityManager)
            //{
            //    return null;
            //}

            var applicationFacade = this.Container.GetInstance<ApplicationFacade>();
            var facilityFacade = this.Container.GetInstance<FacilityFacade>();
            var orgManager = this.Container.GetInstance<OrganizationManager>();

            var org = orgManager.GetByCompAppId(app);

            var cibmtrs = facilityFacade.GetAllForOrg(org.Name);

            var cibmtrDataEntred = true;

            foreach (var cibmtr in cibmtrs)
            {
                foreach (var dm in cibmtr.CibmtrDataMgmts)
                {
                    if (string.IsNullOrWhiteSpace(dm.ProgressOnImplementation) ||
                        string.IsNullOrWhiteSpace(dm.InspectorInformation) ||
                        string.IsNullOrWhiteSpace(dm.InspectorCommendablePractices))
                    {
                        cibmtrDataEntred = false;
                        break;
                    }
                }

                if (!cibmtrDataEntred) break;

                foreach (var outcome in cibmtr.CibmtrOutcomeAnalyses)
                {
                    if (outcome.IsNotRequired.GetValueOrDefault()) continue;

                    if (string.IsNullOrWhiteSpace(outcome.ProgressOnImplementation) ||
                        string.IsNullOrWhiteSpace(outcome.InspectorInformation) ||
                        string.IsNullOrWhiteSpace(outcome.InspectorCommendablePractices) ||
                        string.IsNullOrWhiteSpace(outcome.Inspector100DaySurvival) || 
                        string.IsNullOrWhiteSpace(outcome.Inspector1YearSurvival))
                    {
                        cibmtrDataEntred = false;
                        break;
                    }
                }

                if (!cibmtrDataEntred) break;
            }

            var inspectors = applicationFacade.GetAllInspectionScheduleDetailsForComplianceApplication(app);

            if (inspectors == null) return null;

            if (includeOthers != "Y")
            {
                inspectors =
                    inspectors.Where(
                            x =>
                            x.IsActive && (
                                x.AccreditationRole.Name == Constants.AccreditationsRoles.InspectorTrainees ||
                                    x.AccreditationRole.Name == Constants.AccreditationsRoles.Inspectors))
                        .ToList();
            }

            var dictionary = new Dictionary<string, InspectionScheduleDetail>();

            foreach (var inspector in inspectors)
            {
                var key = $"{inspector.UserId} - {inspector.SiteId}";
                if (dictionary.ContainsKey(key)) continue;

                dictionary.Add(key, inspector);
            }

            inspectors = dictionary.Select(x => x.Value).ToList();

            base.LogMessage("GetComplianceApplicationInspectors", DateTime.Now - startTime);

            return inspectors.Select(x=> ModelConversions.Convert(x, cibmtrDataEntred)).ToList();
        }

        [HttpGet]
        [MyAuthorize]
        [Route("api/Inspection/InspectorsByApp")]
        public async Task<List<InspectionScheduleDetailItems>> GetInspectors(Guid app)
        {
            var startTime = DateTime.Now;

            if (!base.IsFactStaff && base.RoleId != (int)Constants.Role.Inspector)
            {
                return null;
            }

            var applicationFacade = this.Container.GetInstance<ApplicationFacade>();

            var inspectors = await applicationFacade.GetAllInspectionScheduleDetailsForApplicationAsync(app);

            base.LogMessage("GetInspectors", DateTime.Now - startTime);

            return inspectors.Select(x=>ModelConversions.Convert(x)).ToList();
        }

        [HttpPost]
        [MyAuthorize]
        [Route("api/Inspection/MentorFeedback")]
        public async Task<ServiceResponse> SaveMentorFeedback(InspectionScheduleDetailItems model)
        {
            DateTime startTime = DateTime.Now;

            try
            {
                var facade = this.Container.GetInstance<InspectionScheduleFacade>();

                base.LogMessage("SaveMentorFeedback", DateTime.Now - startTime);

                await facade.SaveMentorFeedbackAsync(model.InspectionScheduleDetailId, model.MentorFeedback, base.Email);

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
        [Route("api/Inspection/MentorComplete")]
        public ServiceResponse SendMentorCompleteEmail(Guid compAppId)
        {
            DateTime startTime = DateTime.Now;

            try
            {
                var url = ConfigurationManager.AppSettings[Constants.ConfigurationConstants.Url];

                var facade = this.Container.GetInstance<InspectionFacade>();

                base.LogMessage("SendMentorCompleteEmail", DateTime.Now - startTime);

                facade.SendMentorCompleteEmail(url, compAppId, base.UserId.GetValueOrDefault());

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
    }
}
