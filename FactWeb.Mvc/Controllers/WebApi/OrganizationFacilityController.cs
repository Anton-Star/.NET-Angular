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
using static FactWeb.Infrastructure.Constants;

namespace FactWeb.Mvc.Controllers.WebApi
{
    public class OrganizationFacilityController : BaseWebApiController<OrganizationFacilityController>
    {
        public OrganizationFacilityController(Container container) : base(container)
        {
        }

        /// <summary>
        /// Get all organiztion facility records asynchronously
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [MyAuthorize]
        [Route("api/OrganizationFacility")]
        public async Task<List<OrganizationFacilityItems>> GetAllAsync()
        {
            DateTime startTime = DateTime.Now;

            var organizationFacilityFacade = this.Container.GetInstance<OrganizationFacilityFacade>();
            var organizationFacilities = organizationFacilityFacade.GetOrgFacilities();

            base.LogMessage("GetAllAsync", DateTime.Now - startTime);

            return organizationFacilities;
        }

        [HttpGet]  
        [MyAuthorize]      
        public async Task<List<OrganizationFacilityItems>> Search(int? organizationId, int? facilityId)
        {
            DateTime startTime = DateTime.Now;

            var facade = this.Container.GetInstance<OrganizationFacilityFacade>();

            var results = await facade.SearchAsync(organizationId, facilityId);

            base.LogMessage("Search", DateTime.Now - startTime);

            return results.Select(ModelConversions.Convert).ToList();
        }
        
        [HttpGet]
        [MyAuthorize]
        public async Task<List<FacilitySiteItems>> GetSitesByOrganization(int? organizationId)
        {
            DateTime startTime = DateTime.Now;

            var facade = this.Container.GetInstance<OrganizationFacilityFacade>();

            var results = await facade.SearchAsync(organizationId,null);
         
            base.LogMessage("Search", DateTime.Now - startTime);

            return ModelConversions.ConvertToFacilitySites(results);
         
        }

        /// <summary>
        /// Adds organization facility relation 
        /// </summary>
        /// <param name="organizationId"></param>
        /// <param name="facilityId"></param>
        /// <param name="relation"></param>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        [HttpPost]
        [MyAuthorize]
        public async Task<ServiceResponse<bool>> SaveRelationAsync(OrganizationFacilityModel organizationFacilityModel)
        {
            DateTime startTime = DateTime.Now;

            try
            {
                var organizationFacilityFacade = this.Container.GetInstance<OrganizationFacilityFacade>();
                bool isDuplicate = await organizationFacilityFacade.IsDuplicateRelationAsync(organizationFacilityModel.OrganizationFacilityId, organizationFacilityModel.OrganizationId, organizationFacilityModel.FacilityId);
                string warningMessage = await organizationFacilityFacade.CheckBusinessRulesAsync(organizationFacilityModel.OrganizationFacilityId, organizationFacilityModel.OrganizationId, organizationFacilityModel.FacilityId, Email);

                if (isDuplicate)
                {
                    base.LogMessage("SaveRelationAsync", DateTime.Now - startTime);

                    return new ServiceResponse<bool>
                    {
                        HasError = true,
                        Message = OrganizationFacilityConstant.DuplicateRelation,
                        Item = false
                    };
                }
                else if (!string.IsNullOrEmpty(warningMessage))
                {
                    base.LogMessage("SaveRelationAsync", DateTime.Now - startTime);

                    return new ServiceResponse<bool>
                    {
                        HasError = false,
                        Message = warningMessage,
                        Item = await organizationFacilityFacade.SaveRelationAsync(organizationFacilityModel.OrganizationFacilityId, organizationFacilityModel.OrganizationId, organizationFacilityModel.FacilityId, organizationFacilityModel.Relation, Email)

                    };
                }
                else
                {
                    base.LogMessage("SaveRelationAsync", DateTime.Now - startTime);

                    return new ServiceResponse<bool>
                    {
                        Item = await organizationFacilityFacade.SaveRelationAsync(organizationFacilityModel.OrganizationFacilityId, organizationFacilityModel.OrganizationId, organizationFacilityModel.FacilityId, organizationFacilityModel.Relation, Email),
                        Message = OrganizationFacilityConstant.RelationSavedSuccessfully
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

        /// <summary>
        /// Deletes organization facility relation
        /// </summary>
        /// <param name="organizationFacilityId"></param>
        /// <returns></returns>
        [HttpPost]
        [MyAuthorize]
        public async Task<ServiceResponse<bool>> DeleteRelationAsync(OrganizationFacilityModel organizationFacilityModel)
        {
            try
            {
                var organizationFacilityFacade = this.Container.GetInstance<OrganizationFacilityFacade>();
                return new ServiceResponse<bool>
                {
                    Item = await organizationFacilityFacade.DeleteRelationAsync(organizationFacilityModel.OrganizationFacilityId),
                    Message = OrganizationFacilityConstant.RelationDeletedSuccessfully
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
