using FactWeb.BusinessFacade;
using FactWeb.Infrastructure;
using FactWeb.Model.InterfaceItems;
using FactWeb.Mvc.Hubs;
using FactWeb.Mvc.Models;
using Microsoft.AspNet.SignalR;
using SimpleInjector;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace FactWeb.Mvc.Controllers.WebApi
{
    public class RequirementController : BaseWebApiController<RequirementController>
    {
        public RequirementController(Container container) : base(container)
        {
        }

        [HttpGet]
        [Route("api/Requirement/Export")]
        [MyAuthorize]
        public List<ExportModel> GetExport(Guid versionId)
        {
            if (!base.IsFactStaff)
            {
                throw new Exception("Not Authorized");
            }

            try
            {
                var facade = this.Container.GetInstance<RequirementFacade>();

                var rows = facade.Export(versionId);

                //foreach (var row in rows)
                //{
                //    row.Row = row.Row.Replace("&quot;", "\"");
                //}

                rows.Insert(0, new ExportModel
                {
                    Row = "\"FullStandardName\",\"StandardText\",\"Guidance\",\"QuestionNumber\",\"QuestionText\",\"QuestionType\",\"QuestionNote\",\"OptionText\",\"OptionSkipsTheseQuestions\",\"ExpectedAnswer\",\"AdultAllogeneic\",\"AdultAutologous\",\"PediatricAllogeneic\",\"PediatricAutologous\",\"OffSiteStorage\",\"UnrelatedFixed\",\"RelatedFixed\",\"UnrelatedNonfixed\",\"RelatedNonfixed\",\"PedsAlloAddon\",\"PedsAutoAddon\",\"AdultAutoAddon\",\"AdultAlloAddon\",\"MoreThanMinimalAddon\",\"MinimalAddon\",\"CollectionAddon\",\"ProcessingAddon\",\"UnrelatedAddon\",\"RelatedAddon\",\"NonFixedAddon\",\"FixedAddon\""
                });

                return rows;
            }
            catch (Exception ex)
            {
                base.HandleExceptionForResponse(ex);
                throw;
            }
        }

        [HttpGet]
        [MyAuthorize]
        [Route("api/Requirement")]
        public List<ApplicationVersionItem> Get(string type)
        {
            var startTime = DateTime.Now;

            if (base.RoleName == Constants.Roles.FACTAdministrator || base.RoleName == Constants.Roles.QualityManager)
            {
                var facade = this.Container.GetInstance<RequirementFacade>();

                try
                {
                    var reqs = facade.GetRequirements(type);

                    base.LogMessage(string.Format("Requirement/{0}", type), DateTime.Now - startTime);

                    return reqs;
                }
                catch (Exception ex)
                {
                    base.HandleException(ex);
                    throw;
                }
            }
            else
            {
                return null;
            }
        }

        [HttpGet]
        [MyAuthorize]
        [Route("api/Requirement")]
        public ApplicationVersionItem Get(string type, Guid versionId)
        {
            var startTime = DateTime.Now;

            if (base.RoleName == Constants.Roles.FACTAdministrator || base.RoleName == Constants.Roles.QualityManager)
            {
                var facade = this.Container.GetInstance<RequirementFacade>();

                try
                {
                    var reqs = facade.GetRequirements(versionId, type);

                    base.LogMessage(string.Format("Requirement/{0}", type), DateTime.Now - startTime);

                    return reqs;
                }
                catch (Exception ex)
                {
                    base.HandleException(ex);
                    throw;
                }
            }
            else
            {
                return null;
            }
        }


        [HttpGet]
        [MyAuthorize]
        [Route("api/Requirement/GetByGuid")]
        public ApplicationSectionItem GetByGuid(string guid, string reqId)
        {
            var applicationFacade = this.Container.GetInstance<ApplicationFacade>();

            try
            {
                //following appUniqueId hardcoded
                var appSections = applicationFacade.BuildApplication(base.UserId.GetValueOrDefault(), base.IsFactStaff, Guid.Parse(guid), base.IsFactStaff || base.IsReviewer);
                var section = FindSection(appSections, Guid.Parse(reqId));
                return section;
            }
            catch (Exception ex)
            {
                base.HandleException(ex);
                throw;
            }
        }

        private ApplicationSectionItem FindSection(List<ApplicationSectionItem> appSections, Guid sectionGuid)
        {
            var section = appSections.Where(sec => sec.Id== sectionGuid).SingleOrDefault();
            if (section != null)
                return section;
           
            foreach (var sec in appSections)
            {
                section = FindSection(sec.Children, sectionGuid);
                if (section != null)
                    return section;
            }
            return null;
        }

        [HttpGet]
        [MyAuthorize]
        [Route("api/Requirement/Documents")]
        public List<SectionDocument> GetDocuments(Guid appId, string isComp)
        {
            var startTime = DateTime.Now;

            var facade = this.Container.GetInstance<RequirementFacade>();

            try
            {
                var reqs = facade.GetSectionsWithDocuments(appId, isComp == "Y", base.UserId.GetValueOrDefault(),
                    base.RoleId.GetValueOrDefault());

                base.LogMessage("api/Requirement/Documents", DateTime.Now - startTime);

                return reqs;
            }
            catch (Exception ex)
            {
                base.HandleException(ex);
                throw;
            }
        }

        [HttpPost]
        [MyAuthorize]
        [Route("api/Requirement")]
        public async Task<ServiceResponse<ApplicationSectionItem>> Save(ApplicationSectionItem model)
        {
            var startTime = DateTime.Now;

            if (base.RoleId == (int)Constants.Role.FACTAdministrator || base.RoleId == (int)Constants.Role.PrimaryContact || base.RoleId == (int)Constants.Role.QualityManager)
            {
                try
                {
                    var facade = this.Container.GetInstance<RequirementFacade>();

                    var section = await facade.SaveAsync(model, base.Email);
                    base.LogMessage("Requirement/Save", DateTime.Now - startTime);

                    this.SendInvalidation();

                    return new ServiceResponse<ApplicationSectionItem>
                    {
                        Item = ModelConversions.Convert(section, model.AppUniqueId)
                    };
                }
                catch (Exception ex)
                {
                    return base.HandleException<ApplicationSectionItem>(ex);
                }
            }
            else
            {
                return new ServiceResponse<ApplicationSectionItem>
                {
                    HasError = true,
                    Message = "Not Authorized"
                };
            }
        }

        [HttpDelete]
        [MyAuthorize]
        [Route("api/Requirement/{id}")]
        public ServiceResponse Delete(Guid id)
        {
            var startTime = DateTime.Now;

            if (base.RoleId == (int)Constants.Role.FACTAdministrator || base.RoleId == (int)Constants.Role.PrimaryContact || base.RoleId == (int)Constants.Role.QualityManager)
            {
                try
                {
                    var facade = this.Container.GetInstance<RequirementFacade>();

                    facade.DeleteRequirement(id, base.Email);
                    base.LogMessage("Requirement/Delete", DateTime.Now - startTime);

                    this.SendInvalidation();

                    return new ServiceResponse();
                }
                catch (Exception ex)
                {
                    return base.HandleException(ex);
                }
            }

            else
            {
                return new ServiceResponse
                {
                    HasError = true,
                    Message = "Not Authorized"
                };
            }
        }

        [System.Web.Http.Authorize]
        [HttpPost]
        [Route("api/Requirement/Import")]
        public async Task<ServiceResponse> ProcessImport(string applicationType, string version, string versionNumber)
        {
            if (base.RoleId == (int)Constants.Role.FACTAdministrator || base.RoleId == (int)Constants.Role.PrimaryContact || base.RoleId == (int)Constants.Role.QualityManager)
            {
                var startTime = DateTime.Now;

                try
                {
                    var facade = this.Container.GetInstance<RequirementFacade>();

                    var filesReadToProvider = await this.Request.Content.ReadAsMultipartAsync();


                    var content = filesReadToProvider.Contents.FirstOrDefault();

                    if (content == null) return null;

                    var stream = await content.ReadAsStreamAsync();




                    var contents = new List<string>();
                    using (var reader = new StreamReader(stream))
                    {
                        while (reader.Peek() > 0)
                        {
                            contents.Add(reader.ReadLine());
                        }

                    }

                    var headersRow = contents[0];
                    var headers = headersRow.Split(',').ToList();
                    var rows = contents.GetRange(1, contents.Count - 1);

                    facade.Import(applicationType, version, versionNumber, headers, rows, base.Email);

                    this.SendInvalidation();

                    base.LogMessage("Requirement/Import", DateTime.Now - startTime);

                    return new ServiceResponse();
                }
                catch (Exception ex)
                {
                    return base.HandleException(ex);
                }
            }
            else
            {
                throw new Exception("Not Authorized");
            }
        }

        private void SendInvalidation()
        {
            var hubContext = GlobalHost.ConnectionManager.GetHubContext<CacheHub>();
            hubContext.Clients.All.Invalidated(Constants.CacheStatuses.ActiveVersions);
        }
    }
}
