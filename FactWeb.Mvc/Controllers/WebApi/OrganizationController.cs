using FactWeb.BusinessFacade;
using FactWeb.BusinessLayer;
using FactWeb.Infrastructure;
using FactWeb.Model.InterfaceItems;
using FactWeb.Mvc.Hubs;
using FactWeb.Mvc.Models;
using Microsoft.AspNet.SignalR;
using SimpleInjector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace FactWeb.Mvc.Controllers.WebApi
{
    public class OrganizationController : BaseWebApiController<OrganizationController>
    {
        public OrganizationController(Container container) : base(container)
        {
        }

        [HttpGet]
        [MyAuthorize]
        [Route("api/Organization")]
        public async Task<List<OrganizationItem>> GetAll(string includeFac, string includeAll)
        {
            DateTime startTime = DateTime.Now;
            var organizationFacade = this.Container.GetInstance<OrganizationFacade>();

            var organizations = organizationFacade.GetAll(includeFac == "Y");

            //var orgItems = organizations.Select(x=>ModelConversions.Convert(x)).OrderBy(x => x.OrganizationName).ToList();

            var statuses = new List<string>
            {
                Constants.ApplicationStatus.Applied,
                Constants.ApplicationStatus.InReview,
                Constants.ApplicationStatus.ForReview,
                Constants.ApplicationStatus.RFI,
                Constants.ApplicationStatus.RFIReview,
                Constants.ApplicationStatus.ApplicantResponse,
                Constants.ApplicationStatus.ApplicantResponseReview
            };

            var result = new List<OrganizationItem>();

            foreach (var org in organizations)
            {
                var orgItem = ModelConversions.Convert(org, includeFac == "Y", includeAll == "Y");

                var app = org.Applications.FirstOrDefault(x => statuses.Any(y => y == x.ApplicationStatus.Name) && x.ApplicationType.Name == Constants.ApplicationTypes.Eligibility);

                if (app != null)
                {
                    orgItem.EligibilityApplicationUniqueId = app.UniqueId;   
                }

                var renewalApp = org.Applications.FirstOrDefault(x => statuses.Any(y => y == x.ApplicationStatus.Name) && x.ApplicationType.Name == Constants.ApplicationTypes.Renewal);

                if (renewalApp != null)
                {
                    orgItem.RenewalApplicationUniqueId = renewalApp.UniqueId;
                }

                var complienceApp = org.Applications.FirstOrDefault(x => statuses.Any(y => y == x.ApplicationStatus.Name) && (x.ApplicationType.Name == Constants.ApplicationTypes.CT || x.ApplicationType.Name == Constants.ApplicationTypes.CordBlood || x.ApplicationType.Name == Constants.ApplicationTypes.Common));
                
                if (complienceApp != null)
                {
                    orgItem.ComplianceApplicationUniqueId = complienceApp.ComplianceApplicationId;
                    orgItem.ApplicationUniqueId = complienceApp.UniqueId;
                }


                result.Add(orgItem);
            }

            base.LogMessage("GetAll", DateTime.Now - startTime);

            return result.OrderBy(x => x.OrganizationName).ToList();
        }

        [HttpGet]
        [MyAuthorize]
        [Route("api/Organization/Simple")]
        public List<SimpleOrganization> GetSimpleOrganizations()
        {
            var orgFacade = this.Container.GetInstance<OrganizationFacade>();

            return orgFacade.GetSimpleOrganizations().OrderBy(x=>x.OrganizationName).ToList();
        }

        [HttpGet]
        [MyAuthorize]
        [Route("api/Organization/Flat")]
        public List<OrganizationItem> GetAllFlat()
        {
            DateTime startTime = DateTime.Now;
            var organizationFacade = this.Container.GetInstance<OrganizationFacade>();

            var organizations = organizationFacade.GetAllOrganizationWithApplications();

            var statuses = new List<string>
            {
                Constants.ApplicationStatus.Applied,
                Constants.ApplicationStatus.InReview,
                Constants.ApplicationStatus.ForReview,
                Constants.ApplicationStatus.RFI,
                Constants.ApplicationStatus.RFIReview,
                Constants.ApplicationStatus.ApplicantResponse,
                Constants.ApplicationStatus.ApplicantResponseReview,
                Constants.ApplicationStatus.Approved
            };

            var result = new List<OrganizationItem>();

            foreach (var org in organizations)
            {
                var orgItem = ModelConversions.Convert(org, false);

                var app = org.Applications.FirstOrDefault(x => statuses.Any(y => y == x.ApplicationStatus.Name) && x.ApplicationType.Name == Constants.ApplicationTypes.Eligibility);

                if (app != null)
                {
                    orgItem.EligibilityApplicationUniqueId = app.UniqueId;
                }

                var renewalApp = org.Applications.FirstOrDefault(x => statuses.Any(y => y == x.ApplicationStatus.Name) && x.ApplicationType.Name == Constants.ApplicationTypes.Renewal);

                if (renewalApp != null)
                {
                    orgItem.RenewalApplicationUniqueId = renewalApp.UniqueId;
                }

                var complienceApp = org.Applications.FirstOrDefault(x => statuses.Any(y => y == x.ApplicationStatus.Name) && (x.ApplicationType.Name == Constants.ApplicationTypes.CT || x.ApplicationType.Name == Constants.ApplicationTypes.CordBlood || x.ApplicationType.Name == Constants.ApplicationTypes.Common));

                if (complienceApp != null)
                {
                    orgItem.ComplianceApplicationUniqueId = complienceApp.ComplianceApplicationId;
                    orgItem.ApplicationUniqueId = complienceApp.UniqueId;
                }


                result.Add(orgItem);
            }

            base.LogMessage("GetAllFlat", DateTime.Now - startTime);

            return result.OrderBy(x => x.OrganizationName).ToList();
        }

        [HttpGet]
        [MyAuthorize]
        [Route("api/Organization/{organizationId}")]
        public OrganizationItem GetByID(int organizationId)
        {
            DateTime startTime = DateTime.Now;
            var organizationFacade = this.Container.GetInstance<OrganizationFacade>();
            var organization = organizationFacade.GetByID(organizationId);

            base.LogMessage("GetByID", DateTime.Now - startTime);

            return ModelConversions.Convert(organization);
        }

        [HttpGet]
        [MyAuthorize]
        [Route("api/Organization/Compliance")]
        public string GetNameByCompliance(Guid id)
        {
            var manager = this.Container.GetInstance<ComplianceApplicationManager>();

            var compApp = manager.GetById(id);

            return compApp?.Organization?.Name;
        }

        [HttpGet]
        [MyAuthorize]
        [Route("api/Organization/Name")]
        public OrganizationItem GetByName(string organizationName, string includeFac, string includeAll)
        {
            DateTime startTime = DateTime.Now;
            var organizationFacade = this.Container.GetInstance<OrganizationFacade>();
            var organization = organizationFacade.GetByName(organizationName);

            base.LogMessage("GetByName", DateTime.Now - startTime);

            return ModelConversions.Convert(organization, includeFac == "Y", includeAll == "Y");
        }

        [HttpGet]
        [MyAuthorize]
        [Route("api/Organization/Director")]
        public bool IsDirector(string organizationName)
        {
            var organizationFacade = this.Container.GetInstance<OrganizationFacade>();
            var organization = organizationFacade.GetByName(organizationName);

            if (organization == null) return false;

            return organization.Users.Any(x => x.UserId == base.UserId.GetValueOrDefault() && x.JobFunctionId == Constants.JobFunctionIds.OrganizationDirector);
        }

        [HttpGet]
        [MyAuthorize]
        [Route("api/Organization/Search")]
        public async Task<List<OrganizationItem>> Search(string organizationName, string city, string state)
        {
            DateTime startTime = DateTime.Now;
            var organizationFacade = this.Container.GetInstance<OrganizationFacade>();

            var organizations = await organizationFacade.SearchAsync(organizationName, city, state);

            base.LogMessage("Search", DateTime.Now - startTime);

            return organizations.Select(x=>ModelConversions.Convert(x)).ToList();
        }

        [HttpGet]
        [MyAuthorize]
        [Route("api/Organization/Search")]
        public async Task<List<OrganizationItem>> Search(int? organizationId, int? facilityId)
        {
            DateTime startTime = DateTime.Now;
            var organizationFacade = this.Container.GetInstance<OrganizationFacade>();

            var organizations = await organizationFacade.SearchAsync(organizationId, facilityId);

            base.LogMessage("Search", DateTime.Now - startTime);

            return organizations.Select(x=>ModelConversions.Convert(x)).ToList();
        }

        [HttpGet]
        [MyAuthorize]
        [Route("api/Organization")]
        public async Task<List<OrganizationItem>> GetByFacilityIdAndRelation(int facilityId, bool strongRelation)
        {
            DateTime startTime = DateTime.Now;
            var organizationFacade = this.Container.GetInstance<OrganizationFacade>();

            var organizations = await organizationFacade.GetByFacilityIdAndRelation(facilityId, strongRelation);

            base.LogMessage("GetByFacilityIdAndRelation", DateTime.Now - startTime);

            return organizations.Select(x=>ModelConversions.Convert(x)).ToList();
        }

        /// <summary>
        /// Saves new organization
        /// </summary>
        /// <param name="organizationItem"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/Organization")]
        [MyAuthorize]
        public async Task<ServiceResponse<OrganizationItem>> SaveAsync(OrganizationItem organizationItem)
        {
            DateTime startTime = DateTime.Now;

            try
            {
                var organizationFacade = this.Container.GetInstance<OrganizationFacade>();

                var organization = await organizationFacade.SaveAsync(organizationItem, base.Email, base.IPAddress);

                base.LogMessage("SaveAsync", DateTime.Now - startTime);

                this.SendInvalidation();

                var org = ModelConversions.Convert(organization);
                org.CycleNumber = organizationItem.CycleNumber;
                org.CycleEffectiveDate = organizationItem.CycleEffectiveDate;

                return new ServiceResponse<OrganizationItem>()
                {
                    Item = org
                };
                //return ModelConversions.Convert(organization);
            }
            catch (Exception ex)
            {
                return new ServiceResponse<OrganizationItem>()
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
        /// Updates existing organization
        /// </summary>
        /// <param name="organizationItem"></param>
        /// <returns></returns>
        [HttpPut]
        [MyAuthorize]
        [Route("api/Organization")]
        public async Task<ServiceResponse> UpdateOrganizationAsync(OrganizationItem organizationItem)
        {
            DateTime startTime = DateTime.Now;

            try
            {
                var organizationFacade = this.Container.GetInstance<OrganizationFacade>();

                await organizationFacade.UpdateOrganizationAsync(organizationItem, base.Email, base.IPAddress);

                base.LogMessage("UpdateOrganizationAsync", DateTime.Now - startTime);

                this.SendInvalidation();

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

        /// <summary>
        /// Get all organization types
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [MyAuthorize]
        [Route("api/Organization/Types")]
        public async Task<List<OrganizationTypeItem>> GetOrganizationTypesAsync()
        {
            DateTime startTime = DateTime.Now;

            var organizationFacade = this.Container.GetInstance<OrganizationFacade>();

            var organizationTypes = await organizationFacade.GetOrganizationTypesAsync();

            base.LogMessage("GetOrganizationTypesAsync", DateTime.Now - startTime);

            return organizationTypes.Select(ModelConversions.Convert).ToList();
        }

        /// <summary>
        /// Get all Accredited Services
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [MyAuthorize]
        [Route("api/Organization/AccreditedServices/{organizationId}")]
        public string GetAccreditedServices(int organizationId)
        {
            DateTime startTime = DateTime.Now;

            var organizationFacade = this.Container.GetInstance<OrganizationFacade>();

            var accreditedServices = organizationFacade.GetAccreditedServices(organizationId);

            base.LogMessage("GetAccreditedServices", DateTime.Now - startTime);

            return accreditedServices;
        }

        /// <summary>
        /// Get all Accreditation Status
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [MyAuthorize]
        [Route("api/Organization/AccreditationStatus")]
        public async Task<List<AccreditationStatusItem>> GetAccreditationStatusAsync()
        {
            DateTime startTime = DateTime.Now;

            var organizationFacade = this.Container.GetInstance<OrganizationFacade>();

            var accreditationStatus = await organizationFacade.GetAccreditationStatusAsync();

            base.LogMessage("GetAccreditationStatusAsync", DateTime.Now - startTime);

            return ModelConversions.Convert(accreditationStatus);
        }

        /// <summary>
        /// Get all BAA owner
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [MyAuthorize]
        [Route("api/Organization/BaaOwner")]
        public async Task<List<BAAOwnerItem>> GetBAAOwnerAsync()
        {
            DateTime startTime = DateTime.Now;

            var organizationFacade = this.Container.GetInstance<OrganizationFacade>();

            var baaOwner = await organizationFacade.GetBAAOwnerAsync();

            base.LogMessage("GetBAAOwnerAsync", DateTime.Now - startTime);

            return baaOwner.Select(ModelConversions.Convert).ToList();
        }

        [HttpGet]
        [MyAuthorize]
        [Route("api/Organization/Users")]
        public async Task<List<UserItem>> GetOrgUsers(string name, string includeAll)
        {
            DateTime startTime = DateTime.Now;

            var facade = this.Container.GetInstance<UserFacade>();

            base.LogMessage("Organization/Users", DateTime.Now - startTime);

            var users = await facade.GetAllForOrganizationAsync(name);

            return users.Select(x => ModelConversions.Convert(x, includeAll == "Y", includeAll == "Y")).OrderBy(x => x.FullName).ToList();
        }

        [HttpGet]
        [MyAuthorize]
        [Route("api/Organization/{applicationUniqueId}/Inspectors")]
        public async Task<InspectionItem> GetOrgInspectors(Guid applicationUniqueId)
        {
            DateTime startTime = DateTime.Now;

            var facade = this.Container.GetInstance<InspectionFacade>();

            base.LogMessage("Organization/Inspectors", DateTime.Now - startTime);

            var inspection = await facade.GetInspectionByAppIdAsync(applicationUniqueId);

            return ModelConversions.Convert(inspection);
        }

        [HttpGet]
        [MyAuthorize]
        [Route("api/Organization/{organizationId}/Consultants")]
        public async Task<List<OrganizationConsultantItem>> GetOrgConsultants(int organizationId)
        {
            DateTime startTime = DateTime.Now;

            var facade = this.Container.GetInstance<OrganizationConsultantFacade>();

            base.LogMessage("Organization/Consultants", DateTime.Now - startTime);

            var organizationConsultants = await facade.GetOrganizationConsultantsByOrgIdAsync(organizationId);

            return organizationConsultants.Select(ModelConversions.Convert).ToList();
        }

        [HttpGet]
        [MyAuthorize]
        [Route("api/Organization/{organizationId}/Applications")]
        public List<ApplicationItem> GetOrganizationApplications(int organizationId)
        {
            DateTime startTime = DateTime.Now;
            var organizationFacade = this.Container.GetInstance<OrganizationFacade>();
            var applications = organizationFacade.GetOrganizationApplications(organizationId);

            base.LogMessage("GetOrganizationApplications", DateTime.Now - startTime);

            return applications.Select(x=>ModelConversions.Convert(x)).ToList();
        }

        [HttpGet]
        [MyAuthorize]
        [Route("api/Organization/Sites")]
        public List<SiteItems> GetSites(string name)
        {
            DateTime startTime = DateTime.Now;
            var organizationFacade = this.Container.GetInstance<OrganizationFacade>();
            var sites = organizationFacade.GetOrganizationSites(name);

            base.LogMessage("GetOrganizationApplications", DateTime.Now - startTime);

            return sites;
        }

        [HttpGet]
        [MyAuthorize]
        [Route("api/Organization/BAADocuments/{organizationId}")]
        public List<OrganizationBAADocumentItem> GetBAADocuments(int organizationId)
        {
            DateTime startTime = DateTime.Now;
            var organizationFacade = this.Container.GetInstance<OrganizationFacade>();
            var baaDocuments = organizationFacade.GetOrganizationBAADocuments(organizationId);

            base.LogMessage("GetBAADocuments", DateTime.Now - startTime);

            return baaDocuments.Select(ModelConversions.Convert).ToList();
        }


        [HttpGet]
        [MyAuthorize]
        [Route("api/Organization/Sites")]
        public List<SiteItems> GetSitesByApp(Guid app)
        {
            DateTime startTime = DateTime.Now;
            var organizationFacade = this.Container.GetInstance<OrganizationFacade>();
            var sites = organizationFacade.GetOrganizationSitesByApplication(app);

            base.LogMessage("GetSitesByApp", DateTime.Now - startTime);

            return sites;
        }

        [HttpGet]
        [MyAuthorize]
        [Route("api/Organization/EligibilitySubmitted")]
        public List<OrganizationItem> GetAllWithSubmittedEligibility()
        {
            var start = DateTime.Now;

            var organizationFacade = this.Container.GetInstance<OrganizationFacade>();
            var items = organizationFacade.GetAllWithSubmittedEligibility();

            base.LogMessage("GetAllWithSubmittedEligibility", DateTime.Now - start);

            return items;
        }

        [HttpPost]
        //[MyAuthorize]
        [Route("api/Organization/DocumentLibrary")]
        public async Task<HttpResponseMessage> SetupDocumentLibrary()
        {
            var start = DateTime.Now;

            try
            {
                var facade = this.Container.GetInstance<OrganizationFacade>();
                await facade.SetDocumentLibraryAsync();
                base.LogMessage("SetupDocumentLibrary", DateTime.Now - start);

                return this.Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return this.Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
            }
            

        }

        [HttpPost]
        //[MyAuthorize]
        [Route("api/Organization/DocumentLibraryGroups")]
        public HttpResponseMessage SetupDocumentLibraryGroups()
        {
            try
            {
                var facade = this.Container.GetInstance<OrganizationFacade>();
                facade.SetDocumentLibraryGroupIds();

                return this.Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return this.Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }
    }
}

