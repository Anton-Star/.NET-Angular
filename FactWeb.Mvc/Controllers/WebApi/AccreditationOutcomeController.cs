using FactWeb.BusinessFacade;
using FactWeb.Infrastructure;
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
    public class AccreditationOutcomeController : BaseWebApiController<AccreditationOutcomeController>
    {
        public AccreditationOutcomeController(Container container) : base(container)
        {
        }

        /// <summary>
        /// Get all application status records asynchronously
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [MyAuthorize]
        [Route("api/AccreditationOutcome")]
        public List<AccreditationOutcomeItem> GetAccreditationOutcome()
        {
            DateTime startTime = DateTime.Now;
            var accreditationOutcomeFacade = this.Container.GetInstance<AccreditationOutcomeFacade>();

            base.LogMessage("GetAccreditationOutcome", DateTime.Now - startTime);
            return ModelConversions.Convert(accreditationOutcomeFacade.GetAccreditationOutcome()).ToList();
        }

        /// <summary>
        /// Get Accreditation Outcome records by Organization Id
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [MyAuthorize]
        [Route("api/AccreditationOutcome")]
        public List<AccreditationOutcomeItem> GetAccreditationOutcomeByOrgId(int organizationId)
        {
            DateTime startTime = DateTime.Now;

            var accreditationOutcomeFacade = this.Container.GetInstance<AccreditationOutcomeFacade>();

            base.LogMessage("GetAccreditationOutcomeByOrgId", DateTime.Now - startTime);

            var accreditationOutcome = accreditationOutcomeFacade.GetAccreditationOutcomeByOrgId(organizationId);

            return ModelConversions.Convert(accreditationOutcome).ToList();
        }

        /// <summary>
        /// Get Accreditation Outcome records by Organization Id and Application Id
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [MyAuthorize]
        [Route("api/AccreditationOutcome")]
        public List<AccreditationOutcomeItem> GetAccreditationOutcomeByOrgIdAppId(int organizationId, int applicationId)
        {
            DateTime startTime = DateTime.Now;

            var accreditationOutcomeFacade = this.Container.GetInstance<AccreditationOutcomeFacade>();

            base.LogMessage("GetAccreditationOutcomeByOrgIdAppId", DateTime.Now - startTime);

            var accreditationOutcome = accreditationOutcomeFacade.GetAccreditationOutcomeByOrgIdAppId(organizationId, applicationId);

            return ModelConversions.Convert(accreditationOutcome).ToList();
        }

        /// <summary>
        /// Get Email to , cc and subject for accreditation outcome email.
        /// </summary>
        /// <param name="outcomeLevel"></param>
        /// <param name="organizationId"></param>
        /// <param name="appUniqueId"></param>
        /// <returns></returns>
        [HttpGet]
        [MyAuthorize]
        [Route("api/AccreditationOutcome/GetAccrediationEmailItems")]
        public ServiceResponse<EmailTemplateItem> GetAccrediationEmailItems(string outcomeLevel, int organizationId, string appUniqueId)
        {
            DateTime startTime = DateTime.Now;

            var accreditationOutcomeFacade = this.Container.GetInstance<AccreditationOutcomeFacade>();
            var url = ConfigurationManager.AppSettings[Constants.ConfigurationConstants.Url];

            base.LogMessage("GetAccrediationEmailItems", DateTime.Now - startTime);

            var emailTemplateItem = accreditationOutcomeFacade.GetAccrediationEmailItems(outcomeLevel, organizationId, appUniqueId, url);

            return new ServiceResponse<EmailTemplateItem>
            {
                Item = emailTemplateItem,
                HasError = false
            };
        }

        /// <summary>
        /// Get Accreditation Outcome records by Application Id
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [MyAuthorize]
        [Route("api/AccreditationOutcome")]
        public ServiceResponse<AccreditationOutcomeItem> GetAccreditationOutcomeByAppId(Guid applicationUniqueId)
        {
            DateTime startTime = DateTime.Now;

            try
            {
                var accreditationOutcomeFacade = this.Container.GetInstance<AccreditationOutcomeFacade>();

                base.LogMessage("GetAccreditationOutcomeByAppId", DateTime.Now - startTime);

                var accreditationOutcome = accreditationOutcomeFacade.GetAccreditationOutcomeByAppId(applicationUniqueId);

                if (accreditationOutcome.Count > 0)
                {
                    return new ServiceResponse<AccreditationOutcomeItem>
                    {
                        Item = ModelConversions.Convert(accreditationOutcome[0]),
                        HasError = false
                    };
                }
                else
                {
                    return new ServiceResponse<AccreditationOutcomeItem>
                    {
                        Item = null,
                        HasError = false
                    };
                }
            }
            catch (Exception ex)
            {
                return new ServiceResponse<AccreditationOutcomeItem>
                {
                    HasError = true,
                    Message = ex.Message
                };
            }
        }

        /// <summary>
        /// Update application status
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [MyAuthorize]
        [Route("api/AccreditationOutcome")]
        public ServiceResponse<bool> SaveAsync(AccreditationOutcomeItem accreditationOutcomeItem)
        {
            DateTime startTime = DateTime.Now;

            try
            {
                var accreditationOutcomeFacade = this.Container.GetInstance<AccreditationOutcomeFacade>();

                accreditationOutcomeFacade.Save(accreditationOutcomeItem, Email, AccessToken);
                base.LogMessage("SaveAsync", DateTime.Now - startTime);

                return new ServiceResponse<bool>
                {
                    Item = true,
                    Message = "Accreditation Outcome updated successfully"
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

        [HttpDelete]
        [MyAuthorize]
        [Route("api/AccreditationOutcome/{id}")]
        public async Task<ServiceResponse<bool>> DeleteAsync(int id)
        {
            DateTime startTime = DateTime.Now;

            try
            {
                var accreditationOutcomeFacade = this.Container.GetInstance<AccreditationOutcomeFacade>();

                await accreditationOutcomeFacade.DeleteAsync(id);

                base.LogMessage("DeleteAsync", DateTime.Now - startTime);

                return new ServiceResponse<bool>
                {
                    Item = true,
                    Message = "Accreditation Outcome deleted successfully."
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
    }
}
