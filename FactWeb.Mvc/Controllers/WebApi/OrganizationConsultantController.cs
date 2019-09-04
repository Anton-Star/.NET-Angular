using FactWeb.BusinessFacade;
using FactWeb.Infrastructure;
using FactWeb.Model.InterfaceItems;
using FactWeb.Mvc.Models;
using SimpleInjector;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using static FactWeb.Infrastructure.Constants;

namespace FactWeb.Mvc.Controllers.WebApi
{
    public class OrganizationConsultantController : BaseWebApiController<OrganizationConsultantController>
    {
        public OrganizationConsultantController(Container container) : base(container)
        {
        }

        [HttpGet]
        [MyAuthorize]
        [Route("api/OrganizationConsultant")]
        public async Task<List<OrganizationConsultantItem>> GetAsync()
        {
            DateTime startTime = DateTime.Now;
            try
            {
                var organizationConsultantFacade = this.Container.GetInstance<OrganizationConsultantFacade>();
                var organizationConsultants = await organizationConsultantFacade.GetOrganizationConsultantsAsync();

                var organizationConsultantItems = ModelConversions.Convert(organizationConsultants);

                base.LogMessage("GetAsync", DateTime.Now - startTime);

                return organizationConsultantItems;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpPost]
        [MyAuthorize]
        [Route("api/OrganizationConsultant")]
        public async Task<ServiceResponse<bool>> SaveAsync(OrganizationConsultantItem organizationConsultantItem)
        {
            DateTime startTime = DateTime.Now;

            try
            {
                var organizationConsultantFacade = this.Container.GetInstance<OrganizationConsultantFacade>();
                bool isDuplicate = await organizationConsultantFacade.IsDuplicateConsultantAsync(organizationConsultantItem);                

                if (isDuplicate)
                {
                    base.LogMessage("SaveAsync", DateTime.Now - startTime);

                    return new ServiceResponse<bool>
                    {
                        HasError = true,
                        Message = OrganizationFacilityConstant.DuplicateRelation,
                        Item = false
                    };
                }
                else
                {
                    base.LogMessage("SaveAsync", DateTime.Now - startTime);

                    return new ServiceResponse<bool>
                    {
                        Item = await organizationConsultantFacade.SaveAsync(organizationConsultantItem.OrganizationConsultantId, organizationConsultantItem.OrganizationId, organizationConsultantItem.ConsultantId, organizationConsultantItem.StartDate, organizationConsultantItem.EndDate, Email),
                        Message = OrganizationConsultantConstant.ConsultantAssociatedSuccessfully
                    };
                }
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
        [Route("api/OrganizationConsultant/{id}")]
        public async Task<ServiceResponse<bool>> DeleteAsync(string id)
        {
            try
            {
                var organizationConsultantFacade = this.Container.GetInstance<OrganizationConsultantFacade>();
                return new ServiceResponse<bool>
                {
                    Item = await organizationConsultantFacade.DeleteAsync(Convert.ToInt32(id)),
                    Message = OrganizationConsultantConstant.ConsultantRemovedSuccessfully
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
