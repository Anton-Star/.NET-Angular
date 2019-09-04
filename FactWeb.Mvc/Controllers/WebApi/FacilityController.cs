using FactWeb.BusinessFacade;
using FactWeb.Infrastructure;
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
    public class FacilityController : BaseWebApiController<FacilityController>
    {
        public FacilityController(Container container) : base(container)
        {
        }

        [HttpGet]
        [MyAuthorize]
        public List<FacilityItems> GetAll()
        {
            var facade = this.Container.GetInstance<FacilityFacade>();

            var facilities = facade.GetAll();

            var result = facilities.Select(x=>ModelConversions.Convert(x)).OrderBy(x => x.FacilityName).ToList();
            return result;
        }

        [HttpGet]
        [MyAuthorize]
        [Route("api/Facility/AllActive")]
        public List<FacilityItems> GetAllActiveAsync()
        {
            var facade = this.Container.GetInstance<FacilityFacade>();

            var facilities = facade.GetAllActive();

            return facilities.Select(x => ModelConversions.Convert(x)).OrderBy(x => x.FacilityName).ToList();
        }

        [HttpGet]
        [Route("api/Facility/Flat")]
        [MyAuthorize]
        public List<FacilityItems> GetAllActiveFlatAsync()
        {
            var facade = this.Container.GetInstance<FacilityFacade>();

            var facilities = facade.GetAllActive();

            return facilities.Select(x => ModelConversions.Convert(x, false)).OrderBy(x => x.FacilityName).ToList();
        }

        [HttpGet]
        [MyAuthorize]
        [Route("api/Facility/{facilityId}")]
        public FacilityItems GetByID(int facilityId)
        {
            DateTime startTime = DateTime.Now;
            var facilityFacade = this.Container.GetInstance<FacilityFacade>();
            var facility = facilityFacade.GetByID(facilityId);

            base.LogMessage("GetByID", DateTime.Now - startTime);

            return ModelConversions.Convert(facility);
        }

        [HttpGet]
        [MyAuthorize]
        [Route("api/FacilityWithChild/{facilityId}")]
        public FacilityItems GetByIDWithChild(int facilityId)
        {
            DateTime startTime = DateTime.Now;
            var facilityFacade = this.Container.GetInstance<FacilityFacade>();
            var facility = facilityFacade.GetByID(facilityId);

            base.LogMessage("GetByIDWithChild", DateTime.Now - startTime);

            return ModelConversions.ConvertIncludeChild(facility);
        }
        

        [HttpGet]
        [Route("api/Facility/MasterServiceTypes")]
        public async Task<List<MasterServiceTypeItem>> GetMasterServiceTypes()
        {
            var facade = this.Container.GetInstance<FacilityFacade>();

            var masterServiceTypes = await facade.GetMasterServiceTypesAsync();

            return masterServiceTypes.Select(ModelConversions.Convert).ToList();
        }

        [HttpGet]
        [MyAuthorize]
        [Route("api/Facility/ServiceTypes")]
        public async Task<List<ServiceTypeItem>> GetServiceTypes()
        {
            var facade = this.Container.GetInstance<FacilityFacade>();

            var serviceTypes = await facade.GetServiceTypesAsync();

            return serviceTypes.Select(ModelConversions.Convert).ToList();
        }

        [HttpGet]
        [MyAuthorize]
        [Route("api/Facility/FacilityAccredidations")]
        public async Task<List<FacilityAccreditationItem>> GetFacilityAccredidations()
        {
            try
            {
                var facade = this.Container.GetInstance<FacilityFacade>();

                var facilitiesAccredidations = await facade.GetFacilityAccredidationsAsync();

                return facilitiesAccredidations.Select(ModelConversions.Convert).ToList();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpGet]
        [MyAuthorize]
        [Route("api/Facility/CBCollectionSiteTypes")]
        public async Task<string> GetCBCollectionSiteTypes(int facilityId)
        {
            var facade = this.Container.GetInstance<FacilityFacade>();

            return await facade.GetCBCollectionSiteTypes(facilityId);
        }

        [HttpGet]
        [MyAuthorize]
        [Route("api/Facility/Cibmtr")]
        public List<CibmtrItem> GetAllCibmtrForOrg(string orgName)
        {
            if (!base.IsFactStaff && base.RoleId.GetValueOrDefault() != (int)Constants.Role.Inspector)
            {
                return null;
            }

            var facade = this.Container.GetInstance<FacilityFacade>();
            var items = facade.GetAllForOrg(orgName);

            return items;
        }

        [HttpGet]
        [MyAuthorize]
        [Route("api/Facility/Cibmtr")]
        public List<CibmtrItem> GetAllCibmtr(string facilityName)
        {
            if (!base.IsFactStaff && base.RoleId.GetValueOrDefault() != (int)Constants.Role.Inspector)
            {
                return null;
            }

            var facade = this.Container.GetInstance<FacilityFacade>();
            var items = facade.GetAllCibmtrForFacility(facilityName);

            return items.Select(ModelConversions.Convert).ToList();
        }

        [HttpPost]
        [MyAuthorize]
        [Route("api/Facility/Cibmtr")]
        public ServiceResponse<Guid> SaveCibmtr(CibmtrItem model)
        {
            if (!base.IsFactStaff)
            {
                return new ServiceResponse<Guid>
                {
                    HasError = true,
                    Message = "Not Authorized"
                };
            }

            try
            {
                var facade = this.Container.GetInstance<FacilityFacade>();
                var recorrd = facade.SaveCibmtr(model, base.Email);

                return new ServiceResponse<Guid>
                {
                    Item = recorrd.Id
                };
            }
            catch (Exception ex)
            {
                return base.HandleException<Guid>(ex);
            }

            
        }

        [HttpPost]
        [MyAuthorize]
        [Route("api/Facility/Cibmtr/Outcomes")]
        public ServiceResponse SaveCibmtrOutcomes(List<CibmtrOutcomeAnalysis> model)
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
                var facade = this.Container.GetInstance<FacilityFacade>();

                foreach (var outcome in model)
                {
                    facade.SaveCibmtrOutcome(outcome, base.Email);
                }
                

                return new ServiceResponse();
            }
            catch (Exception ex)
            {
                return base.HandleException(ex);
            }


        }

        [HttpPost]
        [MyAuthorize]
        [Route("api/Facility/Cibmtr/Outcome")]
        public ServiceResponse<Guid> SaveCibmtrOutcome(CibmtrOutcomeAnalysis model)
        {
            if (!base.IsFactStaff)
            {
                return new ServiceResponse<Guid>
                {
                    HasError = true,
                    Message = "Not Authorized"
                };
            }

            try
            {
                var facade = this.Container.GetInstance<FacilityFacade>();
                var recorrd = facade.SaveCibmtrOutcome(model, base.Email);

                return new ServiceResponse<Guid>
                {
                    Item = recorrd.Id
                };
            }
            catch (Exception ex)
            {
                return base.HandleException<Guid>(ex);
            }


        }

        [HttpPut]
        [MyAuthorize]
        [Route("api/Facility/Cibmtr")]
        public ServiceResponse UpdateCibmtrs(UpdateFacilityCibmtrOutcomeModel model)
        {
            try
            {
                var facade = this.Container.GetInstance<FacilityFacade>();

                foreach (var outcome in model.CibmtrOutcomeAnalyses)
                {
                    facade.SaveCibmtrOutcome(outcome, base.Email);
                }

                foreach (var mgmt in model.CibmtrDataMgmts)
                {
                    facade.SaveCibmtrData(mgmt, base.Email);
                }

                return new ServiceResponse();
            }
            catch (Exception ex)
            {
                return base.HandleException(ex);
            }


        }

        [HttpPost]
        [MyAuthorize]
        [Route("api/Facility/Cibmtr/Data")]
        public ServiceResponse<Guid> SaveCibmtrData(CibmtrDataMgmt model)
        {
            if (!base.IsFactStaff)
            {
                return new ServiceResponse<Guid>
                {
                    HasError = true,
                    Message = "Not Authorized"
                };
            }

            try
            {
                var facade = this.Container.GetInstance<FacilityFacade>();
                var recorrd = facade.SaveCibmtrData(model, base.Email);

                return new ServiceResponse<Guid>
                {
                    Item = recorrd.Id
                };
            }
            catch (Exception ex)
            {
                return base.HandleException<Guid>(ex);
            }


        }

        /// <summary>
        /// Add or update facility
        /// </summary>
        /// <param name="facilityItem"></param>
        /// <returns></returns>
        [HttpPost]
        [MyAuthorize]
        [Route("api/Facility")]
        public async Task<ServiceResponse<FacilityItems>> SaveAsync(FacilityItems facilityItem)
        {
            DateTime startTime = DateTime.Now;

            try
            {
                var facilityFacade = this.Container.GetInstance<FacilityFacade>();

                var facility = await facilityFacade.SaveAsync(facilityItem, Email);

                base.LogMessage("SaveAsync", DateTime.Now - startTime);

                this.SendInvalidation();

                return new ServiceResponse<FacilityItems>()
                {
                    Item = ModelConversions.Convert(facility)
                };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<FacilityItems>()
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
        /// Delete facility
        /// </summary>
        /// <param name="facilityId"></param>
        /// <returns></returns>

        [HttpDelete]
        [MyAuthorize]
        [Route("api/Facility")]
        public async Task<ServiceResponse<FacilityItems>> DeleteAsync(int facilityId)
        {
            DateTime startTime = DateTime.Now;

            try
            {
                var facilityFacade = this.Container.GetInstance<FacilityFacade>();

                var facility = await facilityFacade.DeleteAsync(facilityId, this.Email);

                base.LogMessage("Delete", DateTime.Now - startTime);

                this.SendInvalidation();

                return new ServiceResponse<FacilityItems>()
                {
                    Item = ModelConversions.Convert(facility)
                };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<FacilityItems>()
                {
                    HasError = true,
                    Message = ex.Message
                };
            }
        }

    }
}
