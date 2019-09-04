using FactWeb.BusinessFacade;
using FactWeb.Infrastructure;
using FactWeb.Model;
using FactWeb.Model.InterfaceItems;
using FactWeb.Mvc.Hubs;
using FactWeb.Mvc.Models;
using Microsoft.AspNet.SignalR;
using SimpleInjector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace FactWeb.Mvc.Controllers.WebApi
{
    public class SiteController : BaseWebApiController<SiteController>
    {
        public SiteController(Container container) : base(container)
        {
        }

        [HttpGet]
        [MyAuthorize]
        public List<SiteItems> GetAll()
        {
            DateTime startTime = DateTime.Now;
            var siteFacade = this.Container.GetInstance<SiteFacade>();

            var sites = siteFacade.GetAllSites();

            base.LogMessage("GetAll", DateTime.Now - startTime);

            return sites.Select(x=>ModelConversions.Convert(x, true, true)).ToList();
        }

        [HttpGet]
        [Route("api/Site/Flat")]
        [MyAuthorize]
        public List<FlatSite> GetFlatSites()
        {
            var startTime = DateTime.Now;
            var siteFacade = this.Container.GetInstance<SiteFacade>();

            var sites = siteFacade.GetFlatSites();

            base.LogMessage("GetFlatSites", DateTime.Now - startTime);

            return sites;
        }
        
        [HttpGet]
        [MyAuthorize]
        [Route("api/Site/SitesByOrganizationId")]
        public List<SiteItems> GetSitesByOrganizationId(long organizationId)
        {
            DateTime startTime = DateTime.Now;

            var siteFacade = this.Container.GetInstance<SiteFacade>();
            var sites = siteFacade.GetSitesByOrganizationId(organizationId);

            base.LogMessage("GetSitesByOrganizationId", DateTime.Now - startTime);

            return ModelConversions.Convert(sites);
        }

        [HttpGet]
        [MyAuthorize]
        [Route("api/Site/Address")]
        public async Task<SiteItems> GetAddress(long organizationId)
        {
            DateTime startTime = DateTime.Now;

            var siteFacade = this.Container.GetInstance<SiteFacade>();
            var primarySite = await siteFacade.GetPrimarySiteAsync(organizationId);

            base.LogMessage("GetAddress", DateTime.Now - startTime);

            return ModelConversions.Convert(primarySite, true, true);
        }

        [HttpGet]
        [Route("api/Site")]
        [MyAuthorize]
        public SiteItems GetSiteByName(string name)
        {
            var startTime = DateTime.Now;
            var siteFacade = this.Container.GetInstance<SiteFacade>();

            var site = siteFacade.GetByName(name);

            base.LogMessage("GetSiteById", DateTime.Now - startTime);

            return  ModelConversions.Convert(site, true, true);
        }

        [HttpGet]
        [Route("api/Site/Compliance")]
        [MyAuthorize]
        public List<SiteItems> GetSiteByCompliance(Guid compAppId)
        {
            var startTime = DateTime.Now;
            var facade = this.Container.GetInstance<SiteFacade>();

            var sites = facade.GetSitesByCompliance(compAppId);

            base.LogMessage("GetSiteByCompliance", DateTime.Now - startTime);

            return sites.Select(x=>ModelConversions.Convert(x, true, true)).ToList();
        }

        [HttpGet]
        [MyAuthorize]
        public async Task<List<SiteItems>> Search(string siteName)
        {
            DateTime startTime = DateTime.Now;
            var siteFacade = this.Container.GetInstance<SiteFacade>();

            var sites = await siteFacade.SearchAsync(siteName);

            base.LogMessage("Search", DateTime.Now - startTime);

            return sites.Select(x => ModelConversions.Convert(x, true, true)).ToList();
        }

        [HttpGet]
        [MyAuthorize]
        public async Task<List<SiteItems>> Search(int? siteId)
        {
            DateTime startTime = DateTime.Now;
            var siteFacade = this.Container.GetInstance<SiteFacade>();
            var sites = await siteFacade.SearchAsync(siteId);

            base.LogMessage("Search", DateTime.Now - startTime);

            return sites.Select(x => ModelConversions.Convert(x, true, true)).ToList();
        }

        /// <summary>
        /// Saves new site
        /// </summary>
        /// <param name="siteItem"></param>
        /// <returns></returns>
        [HttpPost]
        [MyAuthorize]
        [Route("api/Site")]
        public async Task<ServiceResponse<SiteItems>> SaveAsync(SiteItems siteItem)
        {
            DateTime startTime = DateTime.Now;

            try
            {
                var siteFacade = this.Container.GetInstance<SiteFacade>();

                var site = await siteFacade.SaveAsync(siteItem, Email);

                base.LogMessage("SaveAsync", DateTime.Now - startTime);

                this.SendInvalidation();

                return new ServiceResponse<SiteItems>()
                {
                    Item = ModelConversions.Convert(site, false, false)
                };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<SiteItems>()
                {
                    HasError = true,
                    Message = ex.Message
                };
            }
        }

        private void SendInvalidation()
        {
            var hubContext = GlobalHost.ConnectionManager.GetHubContext<CacheHub>();
            hubContext.Clients.All.Invalidated(Constants.CacheStatuses.Organizations);
            hubContext.Clients.All.Invalidated(Constants.CacheStatuses.Facilities);
            hubContext.Clients.All.Invalidated(Constants.CacheStatuses.Sites);
        }

        /// <summary>
        /// Get all states list
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [MyAuthorize]
        public async Task<List<StateItem>> GetStatesListAsync()
        {
            DateTime startTime = DateTime.Now;

            var siteFacade = this.Container.GetInstance<SiteFacade>();
            var statesList = await siteFacade.GetStatesListAsync();

            base.LogMessage("GetStatesListAsync", DateTime.Now - startTime);

            return statesList.Select(ModelConversions.Convert).ToList();
        }

        /// <summary>
        /// Get all countries list
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [MyAuthorize]
        public async Task<List<CountryItem>> GetCountriesListAsync()
        {
            DateTime startTime = DateTime.Now;

            var siteFacade = this.Container.GetInstance<SiteFacade>();
            var countriesList = await siteFacade.GetCountriesListAsync();

            base.LogMessage("GetClinicalTypesAsync", DateTime.Now - startTime);

            return countriesList.Select(ModelConversions.Convert).ToList();
        }

        /// <summary>
        /// Get all address types
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [MyAuthorize]
        [Route("api/Site/AddressTypes")]
        public async Task<List<AddressTypeItem>> GetAddressTypesListAsync()
        {
            DateTime startTime = DateTime.Now;

            var siteFacade = this.Container.GetInstance<SiteFacade>();
            var addressTypesList = await siteFacade.GetAddressTypesListAsync();

            base.LogMessage("GetAddressTypesListAsync", DateTime.Now - startTime);

            return addressTypesList.Select(ModelConversions.Convert).ToList();
        }

        /// <summary>
        /// Get all clinical types
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [MyAuthorize]
        public async Task<List<ClinicalTypeItem>> GetClinicalTypesAsync()
        {
            DateTime startTime = DateTime.Now;

            var siteFacade = this.Container.GetInstance<SiteFacade>();
            var clinicalTypes = await siteFacade.GetClinicalTypesAsync();

            base.LogMessage("GetClinicalTypesAsync", DateTime.Now - startTime);

            return clinicalTypes.Select(ModelConversions.Convert).ToList();
        }

        /// <summary>
        /// Get all processing types
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [MyAuthorize]
        public async Task<List<ProcessingTypeItem>> GetProcessingTypesAsync()
        {
            DateTime startTime = DateTime.Now;

            var siteFacade = this.Container.GetInstance<SiteFacade>();
            var processingTypes = await siteFacade.GetProcessingTypesAsync();

            base.LogMessage("GetProcessingTypesAsync", DateTime.Now - startTime);

            return processingTypes.Select(ModelConversions.Convert).ToList();
        }

        /// <summary>
        /// Get all Collection Product types
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [MyAuthorize]
        public async Task<List<CollectionProductTypeItem>> GetCollectionProductTypesAsync()
        {
            DateTime startTime = DateTime.Now;

            var siteFacade = this.Container.GetInstance<SiteFacade>();
            var collectionProductTypes = await siteFacade.GetCollectionProductTypesAsync();

            base.LogMessage("GetCollectionProductTypesAsync", DateTime.Now - startTime);

            return collectionProductTypes.Select(ModelConversions.Convert).ToList();
        }

        /// <summary>
        /// Get all CB Collection types
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [MyAuthorize]
        public async Task<List<CBCollectionTypeItem>> GetCBCollectionTypesAsync()
        {
            DateTime startTime = DateTime.Now;

            var siteFacade = this.Container.GetInstance<SiteFacade>();
            var cbCollectionTypes = await siteFacade.GetCBCollectionTypesAsync();

            base.LogMessage("GetCBCollectionTypesAsync", DateTime.Now - startTime);

            return cbCollectionTypes.Select(ModelConversions.Convert).ToList();
        }

        /// <summary>
        /// Get all CB unit types
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [MyAuthorize]
        public async Task<List<CBUnitTypeItem>> GetCBUnitTypesAsync()
        {
            DateTime startTime = DateTime.Now;

            var siteFacade = this.Container.GetInstance<SiteFacade>();
            var cbUnitTypes = await siteFacade.GetCBUnitTypesAsync();

            base.LogMessage("GetCBUnitTypesAsync", DateTime.Now - startTime);

            return cbUnitTypes.Select(ModelConversions.Convert).ToList();
        }

        /// <summary>
        /// Get all Clinical Population types
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [MyAuthorize]
        public async Task<List<ClinicalPopulationTypeItem>> GetClinicalPopulationTypesAsync()
        {
            DateTime startTime = DateTime.Now;

            var siteFacade = this.Container.GetInstance<SiteFacade>();
            var clinicalPopulationTypes = await siteFacade.GetClinicalPopulationTypesAsync();

            base.LogMessage("GetClinicalPopulationTypesAsync", DateTime.Now - startTime);

            return clinicalPopulationTypes.Select(ModelConversions.Convert).ToList();
        }

        /// <summary>
        /// Get all transplant types
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [MyAuthorize]
        public async Task<List<TransplantTypeItem>> GetTransplantTypesAsync()
        {
            DateTime startTime = DateTime.Now;

            var siteFacade = this.Container.GetInstance<SiteFacade>();
            var transplantTypes = await siteFacade.GetTransplantTypesAsync();

            base.LogMessage("GetTransplantTypesAsync", DateTime.Now - startTime);

            return transplantTypes.Select(ModelConversions.Convert).ToList();
        }

        [HttpGet]
        [MyAuthorize]
        [Route("api/Site/CBCategory")]
        public async Task<List<CBCategoryItem>> GetAllCBCategories()
        {
            DateTime startTime = DateTime.Now;

            var siteFacade = this.Container.GetInstance<SiteFacade>();
            var cbCategories = await siteFacade.GetAllCbCategoriesAsync();

            base.LogMessage("GetAllCBCategories", DateTime.Now - startTime);

            return cbCategories.Select(ModelConversions.Convert).ToList();
        }

        [HttpPost]
        [MyAuthorize]
        [Route("api/Site/CordBloodTotal")]
        public async Task<ServiceResponse<SiteCordBloodTransplantTotalItem>> SaveSiteCordBloodTransplantTotal(
            SiteCordBloodTransplantTotalItem model)
        {
            DateTime startTime = DateTime.Now;

            var siteFacade = this.Container.GetInstance<SiteFacade>();

            try
            {
                var item = await siteFacade.SaveSiteCordBloodTransplantTotalAsync(model, base.Email);
                base.LogMessage("SaveSiteCordBloodTransplantTotal", DateTime.Now - startTime);

                if (!model.Id.HasValue)
                {
                    item.CbCategory = new CBCategory
                    {
                        Id = 0,
                        Name = model.CbCategory.Name
                    };
                    item.CbUnitType = new CBUnitType
                    {
                        Id = 0,
                        Name = model.CbUnitType.Name
                    };
                }

                return new ServiceResponse<SiteCordBloodTransplantTotalItem>
                {
                    Item = ModelConversions.Convert(item)
                };
            }
            catch (Exception ex)
            {
                return base.HandleException<SiteCordBloodTransplantTotalItem>(ex);
            }
        }

        [HttpPost]
        [MyAuthorize]
        [Route("api/Site/TransplantTotal")]
        public async Task<ServiceResponse<SiteTransplantTotalItem>> SaveSiteTransplantTotal(SiteTransplantTotalItem model)
        {
            DateTime startTime = DateTime.Now;

            var siteFacade = this.Container.GetInstance<SiteFacade>();

            try
            {
                var item = await siteFacade.SaveSiteTransplantTotalAsync(model, base.Email);
                base.LogMessage("SaveSiteTransplantTotal", DateTime.Now - startTime);

                if (!model.Id.HasValue)
                {
                    item.TransplantCellType = new TransplantCellType
                    {
                        Name = model.TransplantCellType.Name
                    };
                    item.ClinicalPopulationType = new ClinicalPopulationType
                    {
                        Id = 0,
                        Name = model.ClinicalPopulationType.Name
                    };
                    item.TransplantType = new TransplantType
                    {
                        Id = 0,
                        Name = model.TransplantType.Name
                    };
                }

                this.SendInvalidation();

                return new ServiceResponse<SiteTransplantTotalItem>
                {
                    Item = ModelConversions.Convert(item)
                };
            }
            catch (Exception ex)
            {
                return base.HandleException<SiteTransplantTotalItem>(ex);
            }
        }

        [HttpPost]
        [MyAuthorize]
        [Route("api/Site/CollectionTotal")]
        public async Task<ServiceResponse<SiteCollectionTotalItem>> SaveSiteCollectionTotal(SiteCollectionTotalItem model)
        {
            DateTime startTime = DateTime.Now;

            var siteFacade = this.Container.GetInstance<SiteFacade>();

            try
            {
                var item = await siteFacade.SaveSiteCollectionTotalAsync(model, base.Email);
                base.LogMessage("SaveSiteCollectionTotal", DateTime.Now - startTime);

                if (!model.Id.HasValue)
                {
                    item.CollectionType = new CollectionType
                    {
                        Name = model.CollectionType.Name
                    };
                    item.ClinicalPopulationType = new ClinicalPopulationType
                    {
                        Name = model.ClinicalPopulationType.Name
                    };
                }

                this.SendInvalidation();

                return new ServiceResponse<SiteCollectionTotalItem>
                {
                    Item = ModelConversions.Convert(item)
                };
            }
            catch (Exception ex)
            {
                return base.HandleException<SiteCollectionTotalItem>(ex);
            }
        }

        [HttpPost]
        [MyAuthorize]
        [Route("api/Site/ProcessingTotal")]
        public async Task<ServiceResponse<SiteProcessingTotalItem>> SaveSiteProcessingTotal(SiteProcessingTotalItem model)
        {
            DateTime startTime = DateTime.Now;

            var siteFacade = this.Container.GetInstance<SiteFacade>();

            try
            {
                var items = await siteFacade.SaveSiteProcessingTotalAsync(model, base.Email);
                base.LogMessage("SaveSiteProcessingTotal", DateTime.Now - startTime);

                this.SendInvalidation();

                return new ServiceResponse<SiteProcessingTotalItem>
                {
                    Item = items
                };
            }
            catch (Exception ex)
            {
                return base.HandleException<SiteProcessingTotalItem>(ex);
            }
        }

        [HttpPost]
        [MyAuthorize]
        [Route("api/Site/ProcessingMethodologyTotal")]
        public async Task<ServiceResponse<SiteProcessingMethodologyTotalItem>> SaveProcessingMethodologyTotal(SiteProcessingMethodologyTotalItem model)
        {
            DateTime startTime = DateTime.Now;

            var siteFacade = this.Container.GetInstance<SiteFacade>();

            try
            {
                var item = await siteFacade.SaveSiteProcessingMethodologyTotalAsync(model, base.Email);
                base.LogMessage("SaveProcessingMethodologyTotal", DateTime.Now - startTime);

                if (!model.Id.HasValue)
                {
                    item.ProcessingType = new ProcessingType
                    {
                        Name = model.ProcessingType.Name
                    };
                }

                this.SendInvalidation();

                return new ServiceResponse<SiteProcessingMethodologyTotalItem>
                {
                    Item = ModelConversions.Convert(item)
                };
            }
            catch (Exception ex)
            {
                return base.HandleException<SiteProcessingMethodologyTotalItem>(ex);
            }
        }

        [HttpDelete]
        [MyAuthorize]
        [Route("api/Site/ProcessingMethodologyTotal/{id}")]
        public ServiceResponse DeleteProcessingMethodologyTotal(Guid id)
        {
            if (!base.IsFactStaff)
            {
                return new ServiceResponse
                {
                    HasError = true,
                    Message = "Not Authorized"
                };
            }

            try
            {
                var facade = this.Container.GetInstance<SiteFacade>();

                facade.DeleteSiteTotal(id, "MO");

                return new ServiceResponse();
            }
            catch (Exception ex)
            {
                return base.HandleException(ex);
            }
        }

        [HttpDelete]
        [MyAuthorize]
        [Route("api/Site/ProcessingTotal/{id}")]
        public ServiceResponse DeleteProcessingTotal(Guid id)
        {
            if (!base.IsFactStaff)
            {
                return new ServiceResponse
                {
                    HasError = true,
                    Message = "Not Authorized"
                };
            }

            try
            {
                var facade = this.Container.GetInstance<SiteFacade>();

                facade.DeleteSiteTotal(id, "PO");

                return new ServiceResponse();
            }
            catch (Exception ex)
            {
                return base.HandleException(ex);
            }
        }

        [HttpDelete]
        [MyAuthorize]
        [Route("api/Site/CollectionTotal/{id}")]
        public ServiceResponse DeleteCollectionTotal(Guid id)
        {
            if (!base.IsFactStaff)
            {
                return new ServiceResponse
                {
                    HasError = true,
                    Message = "Not Authorized"
                };
            }

            try
            {
                var facade = this.Container.GetInstance<SiteFacade>();

                facade.DeleteSiteTotal(id, "CO");

                return new ServiceResponse();
            }
            catch (Exception ex)
            {
                return base.HandleException(ex);
            }
        }

        //
        [HttpDelete]
        [MyAuthorize]
        [Route("api/Site/TransplantTotal/{id}")]
        public ServiceResponse DeleteTransplantTotal(Guid id)
        {
            if (!base.IsFactStaff)
            {
                return new ServiceResponse
                {
                    HasError = true,
                    Message = "Not Authorized"
                };
            }

            try
            {
                var facade = this.Container.GetInstance<SiteFacade>();

                facade.DeleteSiteTotal(id, "TO");

                return new ServiceResponse();
            }
            catch (Exception ex)
            {
                return base.HandleException(ex);
            }
        }

        //
        [HttpDelete]
        [MyAuthorize]
        [Route("api/Site/CordBloodTotal/{id}")]
        public ServiceResponse DeleteCordBloodTotal(Guid id)
        {
            if (!base.IsFactStaff)
            {
                return new ServiceResponse
                {
                    HasError = true,
                    Message = "Not Authorized"
                };
            }

            try
            {
                var facade = this.Container.GetInstance<SiteFacade>();

                facade.DeleteSiteTotal(id, "CB");

                return new ServiceResponse();
            }
            catch (Exception ex)
            {
                return base.HandleException(ex);
            }
        }
    }
}
