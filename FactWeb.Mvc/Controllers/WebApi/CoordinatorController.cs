using FactWeb.BusinessFacade;
using FactWeb.Model.InterfaceItems;
using FactWeb.Mvc.Models;
using SimpleInjector;
using System;
using System.Threading.Tasks;
using System.Web.Http;

namespace FactWeb.Mvc.Controllers.WebApi
{
    public class CoordinatorController : BaseWebApiController<CoordinatorController>
    {
        public CoordinatorController(Container container) : base(container)
        {
        }
        
        [HttpGet]
        [MyAuthorize]
        [Route("api/Coordinator")]
        public async Task<CoordinatorViewItem> Get(Guid appliactionUniqueId)
        {
            try
            {
                var start = DateTime.Now;
                var coordinatorFacade = this.Container.GetInstance<CoordinatorFacade>();

                var coordinatorViewItem = await coordinatorFacade.GetCoordinatorView(appliactionUniqueId);

                base.LogMessage("Get", DateTime.Now - start);

                //var coordinatorViewItem = ModelConversions.Convert(coordinatorView);

                return coordinatorViewItem;
            }
            catch (Exception ex)
            {
                base.HandleExceptionForResponse(ex);
                throw;
            }
        }

        [HttpPost]
        [MyAuthorize]
        [Route("api/Coordinator")]
        public ServiceResponse Save(SaveCoordinatorViewModel model)
        {
            try
            {
                if (!base.IsFactStaff)
                {
                    throw new Exception("Not Authorized");
                }

                var coordinatorFacade = this.Container.GetInstance<CoordinatorFacade>();

                coordinatorFacade.SaveCoordinatorViewChanges(model.ComplianceApplicationId, model.AccreditationGoal,
                    model.InspectionScope, model.AccreditedSinceDate, model.TypeDetail, base.Email);

                return new ServiceResponse();
            }
            catch (Exception ex)
            {
                return base.HandleException(ex);
            }
        }

        [HttpPost]
        [MyAuthorize]
        [Route("api/Coordinator/Personnel")]
        public ServiceResponse SavePersonnel(SavePersonnelModel model)
        {
            try
            {
                if (!base.IsFactStaff)
                {
                    throw new Exception("Not Authorized");
                }

                var coordinatorFacade = this.Container.GetInstance<CoordinatorFacade>();

                coordinatorFacade.SavePersonnel(model.OrgId, model.Personnel, base.Email);

                return new ServiceResponse();
            }
            catch (Exception ex)
            {
                return base.HandleException(ex);
            }
        }
    }
}