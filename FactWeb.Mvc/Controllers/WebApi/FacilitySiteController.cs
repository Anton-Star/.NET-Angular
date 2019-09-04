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
    public class FacilitySiteController : BaseWebApiController<FacilitySiteController>
    {
        public FacilitySiteController(Container container) : base(container)
        {
        }

        /// <summary>
        /// Get all facility site records against current user Id
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [MyAuthorize]
        public ServiceResponse<FacilitySitePageItems> GetFacilitySiteByUserId()
        {
            DateTime startTime = DateTime.Now;
            try
            {
                var facilitySiteFacade = this.Container.GetInstance<FacilitySiteFacade>();
                var siteFacade = this.Container.GetInstance<SiteFacade>();
                var facilityFacade = this.Container.GetInstance<FacilityFacade>();
                var facilitiesSites = facilitySiteFacade.GetFacilitySiteByUserId(Email);

                FacilitySitePageItems facilitySitePageItems = new FacilitySitePageItems();
                facilitySitePageItems.FacilitySiteItems = ModelConversions.Convert(facilitiesSites);
                facilitySitePageItems.SiteItems = ModelConversions.Convert(siteFacade.GetAll());
                facilitySitePageItems.FacilityItems = ModelConversions.Convert(facilityFacade.GetAll());

                base.LogMessage("GetFacilitySiteByUserId", DateTime.Now - startTime);

                return new ServiceResponse<FacilitySitePageItems>
                {
                    Item = facilitySitePageItems
                };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<FacilitySitePageItems>
                {
                    HasError = true,
                    Message = ex.Message
                };
            }

        }

        /// <summary>
        /// Get all facility site records against current user Id asynchronously
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [MyAuthorize]
        public async Task<ServiceResponse<FacilitySitePageItems>> GetFacilitySiteByUserIdAsync()
        {
            DateTime startTime = DateTime.Now;
            try
            {
                var facilitySiteFacade = this.Container.GetInstance<FacilitySiteFacade>();
                var siteFacade = this.Container.GetInstance<SiteFacade>();
                var facilityFacade = this.Container.GetInstance<FacilityFacade>();
                var facilitiesSites = await facilitySiteFacade.GetFacilitySiteByUserIdAsync(Email);

                FacilitySitePageItems facilitySitePageItems = new FacilitySitePageItems();
                facilitySitePageItems.FacilitySiteItems = ModelConversions.Convert(facilitiesSites);
                facilitySitePageItems.SiteItems = ModelConversions.Convert(siteFacade.GetAll());
                facilitySitePageItems.FacilityItems = ModelConversions.Convert(facilityFacade.GetAll());

                base.LogMessage("GetFacilitySiteByUserIdAsync", DateTime.Now - startTime);

                return new ServiceResponse<FacilitySitePageItems>
                {
                    Item = facilitySitePageItems
                };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<FacilitySitePageItems>
                {
                    HasError = true,
                    Message = ex.Message
                };
            }

        }

        [HttpGet]
        [MyAuthorize]
        public async Task<List<FacilitySiteItems>> Search(int? siteId, int? facilityId)
        {
            DateTime startTime = DateTime.Now;

            var facade = this.Container.GetInstance<FacilitySiteFacade>();

            var results = await facade.SearchAsync(siteId, facilityId);

            base.LogMessage("Search", DateTime.Now - startTime);

            return results.Select(ModelConversions.Convert).ToList();
        }

        /// <summary>
        /// Deletes facility site relation
        /// </summary>
        [HttpPost]
        [MyAuthorize]
        public async Task<ServiceResponse<bool>> DeleteRelationAsync(FacilitySiteModel facilitySiteModel)
        {
            try
            {
                var facilitySiteFacade = this.Container.GetInstance<FacilitySiteFacade>();
                return new ServiceResponse<bool>
                {
                    Item = await facilitySiteFacade.DeleteRelationAsync(facilitySiteModel.FacilitySiteId),
                    Message = FacilitySiteConstant.RelationDeletedSuccessfully
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
