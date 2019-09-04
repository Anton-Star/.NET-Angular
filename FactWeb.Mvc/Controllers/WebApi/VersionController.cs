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
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Hosting;
using System.Web.Http;

namespace FactWeb.Mvc.Controllers.WebApi
{
    public class VersionController : BaseWebApiController<VersionController>
    {
        public VersionController(Container container) : base(container)
        {
        }

        [HttpPost]
        [MyAuthorize]
        [Route("api/Version")]
        public async Task<ServiceResponse> MakeVersionActive(ApplicationVersionItem version)
        {
            var startTime = DateTime.Now;

            try
            {
                var facade = this.Container.GetInstance<RequirementFacade>();

                await facade.MakeVersionActiveAsync(version.Id, base.Email);

                var appFacade = this.Container.GetInstance<ApplicationFacade>();

                ApplicationFacade.ActiveVersions = new Dictionary<int, ApplicationVersion>();

                HostingEnvironment.QueueBackgroundWorkItem(x =>
                {
                    appFacade.GetActiveApplicationVersions();

                    base.LogMessage("Loading Active Application Versions Completed.", new TimeSpan());
                });

                this.SendInvalidation();

                base.LogMessage("Version/MakeVersionActive", DateTime.Now - startTime);

                return new ServiceResponse();
            }
            catch (Exception ex)
            {
                return base.HandleException(ex);
            }
        }

        [HttpGet]
        [MyAuthorize]
        [Route("api/Version")]
        public ApplicationVersionItem Get(Guid version)
        {
            var startTime = DateTime.Now;

            var facade = this.Container.GetInstance<ApplicationFacade>();

            var applicationVersion = facade.GetApplicationVersion(version);

            base.LogMessage("Version/Get", DateTime.Now - startTime);

            return ModelConversions.Convert(applicationVersion, null, false);
        }

        [HttpPut]
        [MyAuthorize]
        [Route("api/Version")]
        public ServiceResponse<ApplicationVersionItem> Add(ApplicationVersionItem model)
        {
            var startTime = DateTime.Now;

            try
            {
                var facade = this.Container.GetInstance<RequirementFacade>();
                var appFacade = this.Container.GetInstance<ApplicationFacade>();

                var version = facade.Add(model.ApplicationType.ApplicationTypeName, model.CopyFromId, model.Title,
                        model.VersionNumber, model.IsActive, base.Email);

                version = appFacade.GetApplicationVersion(version.Id);

                base.LogMessage("Version/Add", DateTime.Now - startTime);

                return new ServiceResponse<ApplicationVersionItem>
                {
                    Item = ModelConversions.Convert(version, null, false)
                };
            }
            catch (Exception ex)
            {
                return base.HandleException<ApplicationVersionItem>(ex);
            }
        }

        [HttpGet]
        [MyAuthorize]
        [Route("api/Version/Active")]
        public List<ApplicationVersionItem> GetActiveVersions()
        {
            var facade = this.Container.GetInstance<ApplicationFacade>();

            var versions = facade.GetActiveApplicationVersions();

            return versions.Select(x=>ModelConversions.Convert(x, null, true)).ToList();
        }

        [HttpGet]
        [MyAuthorize]
        [Route("api/Version/Active/Simple")]
        public List<ApplicationVersionItem> GetSimpleActiveVersions()
        {
            var facade = this.Container.GetInstance<ApplicationFacade>();

            var versions = facade.GetSimpleActiveApplicationVersions();

            return versions.Select(x => ModelConversions.Convert(x, null, true, null, false)).ToList();
        }

        [HttpDelete]
        [Route("api/Version/{id}")]
        [MyAuthorize]
        public HttpResponseMessage Delete(Guid id)
        {
            try
            {
                var facade = this.Container.GetInstance<ApplicationFacade>();
                facade.DeleteVersion(id, base.Email);

                return this.Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return this.Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }



        }

        [HttpGet]
        [Route("api/Version/Cache")]
        [MyAuthorize]
        public HttpResponseMessage RebuildCache(int type, Guid versionId)
        {
            if (!base.IsFactStaff)
            {
                throw new Exception("Not Authorized");
            }

            try
            {
                var facade = this.Container.GetInstance<ApplicationFacade>();
                facade.RemoveFromActiveVersionAndReload(type, versionId);

                return this.Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return this.Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        private void SendInvalidation()
        {
            var hubContext = GlobalHost.ConnectionManager.GetHubContext<CacheHub>();
            hubContext.Clients.All.Invalidated(Constants.CacheStatuses.ActiveVersions);
        }
    }
}
