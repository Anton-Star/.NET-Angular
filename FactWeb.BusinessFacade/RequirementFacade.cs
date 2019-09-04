using FactWeb.BusinessLayer;
using FactWeb.Infrastructure;
using FactWeb.Model;
using FactWeb.Model.InterfaceItems;
using SimpleInjector;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Linq;
using System.Threading.Tasks;
using ApplicationManager = FactWeb.BusinessLayer.ApplicationManager;

namespace FactWeb.BusinessFacade
{
    public class RequirementFacade
    {
        private readonly Container container;

        public RequirementFacade(Container container)
        {
            this.container = container;
        }
        public ApplicationSectionItem GetRequirementById(Guid reqGuid, Guid appUniqueId)
        {
            var sectionManager = this.container.GetInstance<ApplicationSectionManager>();
            var applicationManager = this.container.GetInstance<ApplicationManager>();

            var app = applicationManager.GetByUniqueId(appUniqueId, true);
            var section = sectionManager.GetRequirementById(reqGuid, app.Id);

            List<ApplicationResponse> lstResponses = new List<ApplicationResponse>(app.ApplicationResponses);
            foreach (var question in section.Questions)
            {
                question.ApplicationResponses = lstResponses.FindAll(q => q.ApplicationSectionQuestionId == question.Id);
            }

            var sectionItem = ModelConversions.Convert(section, null);
            return sectionItem;
        }

        public List<ApplicationVersionItem> GetRequirements(string applicationTypeName)
        {
            var versionManager = this.container.GetInstance<ApplicationVersionManager>();
            var applicationTypeManager = this.container.GetInstance<ApplicationTypeManager>();
            var applicationSectionManager = this.container.GetInstance<ApplicationSectionManager>();

            var applicationType = applicationTypeManager.GetByName(applicationTypeName);
            var version = applicationType.ApplicationVersions.FirstOrDefault(x => x.IsActive);

            if (applicationType == null)
            {
                throw new ObjectNotFoundException(string.Format("Cannot find application type {0}", applicationTypeName));
            }

            List<ApplicationSection> sections = null;
            if (ApplicationFacade.ActiveVersions.ContainsKey(applicationType.Id))
            {
                var ver = ApplicationFacade.ActiveVersions[applicationType.Id];
                sections = ver.ApplicationSections.ToList();
            }
            else
            {
                sections = applicationSectionManager.GetFlatForApplicationType(applicationType.Id, version?.Id);
            }

            return versionManager.GetAllForApplicationType(sections, applicationTypeName, true); 
        }

        public ApplicationVersionItem GetRequirements(Guid versionId, string applicationTypeName)
        {
            var versionManager = this.container.GetInstance<ApplicationVersionManager>();
            var applicationTypeManager = this.container.GetInstance<ApplicationTypeManager>();
            var applicationSectionManager = this.container.GetInstance<ApplicationSectionManager>();

            var applicationType = applicationTypeManager.GetByName(applicationTypeName);

            if (applicationType == null)
            {
                throw new ObjectNotFoundException(string.Format("Cannot find application type {0}", applicationTypeName));
            }

            List<ApplicationSection> sections = null;
            sections = applicationSectionManager.GetFlatForApplicationType(applicationType.Id, versionId);

            var versions = versionManager.GetAllForApplicationType(sections, applicationTypeName, false);

            return versions.SingleOrDefault(x => x.Id == versionId);
        }

        public List<ExportModel> Export(Guid applicationVersionId)
        {
            var manager = this.container.GetInstance<ApplicationVersionManager>();

            return manager.Export(applicationVersionId);
        }

        public ApplicationVersion Import(string applicatonTypeName, string versionName, string versionNumber, List<string> headers, List<string> rows, string importedBy)
        {
            var applicationSectionManager = this.container.GetInstance<ApplicationSectionManager>();
            var applicationTypeManager = this.container.GetInstance<ApplicationTypeManager>();
            var scopeTypeManager = this.container.GetInstance<ScopeTypeManager>();
            var versionManager = this.container.GetInstance<ApplicationVersionManager>();

            var applicationType = applicationTypeManager.GetByName(applicatonTypeName);
            var scopeTypes = scopeTypeManager.GetAllActive();

            var version = new ApplicationVersion
            {
                Id = Guid.NewGuid(),
                ApplicationTypeId = applicationType.Id,
                Title = versionName,
                IsActive = false,
                VersionNumber = versionNumber,
                CreatedDate = DateTime.Now,
                CreatedBy = importedBy
            };
            
            version = applicationSectionManager.Import(scopeTypes, version, applicationType, headers, rows, importedBy);

            versionManager.Add(version);

            return version;
        }
        

        public List<SectionDocument> GetSectionsWithDocuments(Guid appId, bool isCompliance, Guid userId, int roleId)
        {
            var applicationSectionManager = this.container.GetInstance<ApplicationSectionManager>();
          
            return applicationSectionManager.GetDocuments(appId, isCompliance, roleId);
        }

        public async Task<List<SectionDocument>> GetSectionsWithDocumentsAsync(int organizationId)
        {
            var applicationSectionManager = this.container.GetInstance<ApplicationSectionManager>();

            return await applicationSectionManager.GetDocumentsAsync(organizationId);
        }

        public async Task<ApplicationSection> SaveAsync(ApplicationSectionItem item, string updatedBy)
        {
            var applicationSectionManager = this.container.GetInstance<ApplicationSectionManager>();
            var applicationTypeManager = this.container.GetInstance<ApplicationTypeManager>();
            var applicationSectionScopeType = this.container.GetInstance<ApplicationSectionScopeTypeManager>();
            var applicationVersionManager = this.container.GetInstance<ApplicationVersionManager>();
            
            var applicationType = await applicationTypeManager.GetByNameAsync(item.ApplicationTypeName);

            if (applicationType == null)
            {
                throw new ObjectNotFoundException("Can't find Application Type");
            }

            var version = applicationVersionManager.GetById(item.VersionId.GetValueOrDefault());

            var applicationSection = await applicationSectionManager.AddOrUpdateAsync(applicationType, item, updatedBy);

            await applicationSectionScopeType.UpdateApplicationSectionScopeTypeAsync(item,  updatedBy, applicationSection.Id);

            if (item.Id != null) return applicationSection;

            await applicationVersionManager.CheckVersionNumberAsync(version, item.Version, updatedBy);
            
            return applicationSection;
        }

        public void DeleteRequirement(Guid id, string updatedBy)
        {
            var applicationSectionManager = this.container.GetInstance<ApplicationSectionManager>();

            applicationSectionManager.Remove(id, updatedBy);
            
        }

        public async Task MakeVersionActiveAsync(Guid versionId, string updatedBy)
        {
            var applicationVersionManager = this.container.GetInstance<ApplicationVersionManager>();

            var newVersion = applicationVersionManager.GetById(versionId);

            if (newVersion == null)            
                throw new KeyNotFoundException("Cannot find version");            

            var versions = applicationVersionManager.GetByType(newVersion.ApplicationTypeId);

            await applicationVersionManager.MakeActiveAsync(newVersion, versions, updatedBy);
        }

        public ApplicationVersion Add(string applicationTypeName, Guid? copyFromVersionId, string title,
            string number, bool isActive, string createdBy)
        {
            var applicationTypeManager = this.container.GetInstance<ApplicationTypeManager>();
            var applicationVersionManager = this.container.GetInstance<ApplicationVersionManager>();

            var applicationType = applicationTypeManager.GetByName(applicationTypeName);

            if (applicationType == null)
            {
                throw new KeyNotFoundException("Cannot find Application Type");
            }

            List<ApplicationSection> sections = null;

            if (copyFromVersionId.HasValue)
            {
                sections = this.GetCopiedSections(copyFromVersionId.Value, createdBy);
            }

            return
                applicationVersionManager.Add(applicationType, sections, title, number, isActive,
                        createdBy);
        }

        private List<ApplicationSection> GetCopiedSections(Guid versionId, string updatedBy)
        {
            var result = new List<ApplicationSection>();

            var applicationSectionManager = this.container.GetInstance<ApplicationSectionManager>();
            var sections = applicationSectionManager.GetAllForVersion(versionId);

            var questions = new Dictionary<Guid, Guid>();

            foreach (var sect in sections.Where(x => x.Questions != null && x.Questions.Count > 0))
            {
                foreach (var question in sect.Questions)
                {
                    if (questions.ContainsKey(question.Id)) continue;

                    questions.Add(question.Id, Guid.NewGuid());
                }
            }

            foreach (var section in sections.Where(x=>x.ParentApplicationSectionId == null))
            {
                var parent = this.CreateNewSection(questions, section, null, updatedBy);

                result.Add(parent);

                var children = sections.Where(x => x.ParentApplicationSectionId == section.Id).ToList();

                result.AddRange(this.CopyChildrenSections(questions, sections, children, parent.Id, updatedBy));
            }

            return result;
        }

        private List<ApplicationSection> CopyChildrenSections(Dictionary<Guid, Guid> questions, List<ApplicationSection> allSections, List<ApplicationSection> sections, Guid? parentId,
            string updatedBy)
        {
            var result = new List<ApplicationSection>();

            foreach (var section in sections)
            {
                var newSection = this.CreateNewSection(questions, section, parentId, updatedBy);

                result.Add(newSection);

                var children = allSections.Where(x => x.ParentApplicationSectionId == section.Id).ToList();

                result.AddRange(this.CopyChildrenSections(questions, allSections, children, newSection.Id, updatedBy));
            }

            return result;
        }

        private ApplicationSection CreateNewSection(Dictionary<Guid, Guid> questions, ApplicationSection section, Guid? parentId, string updatedBy)
        {
            return new ApplicationSection
            {
                Id = Guid.NewGuid(),
                ApplicationTypeId = section.ApplicationTypeId,
                ParentApplicationSectionId = parentId,
                PartNumber = section.PartNumber,
                Name = section.Name,
                IsActive = section.IsActive,
                CreatedBy = updatedBy,
                CreatedDate = DateTime.Now,
                HelpText = section.HelpText,
                IsVariance = section.IsVariance,
                Version = section.Version,
                Order = section.Order,
                UniqueIdentifier = section.UniqueIdentifier,
                Questions = section.Questions.Select(x=> new ApplicationSectionQuestion
                {
                    Id = questions[x.Id],
                    QuestionTypeId = x.QuestionTypeId,
                    Text = x.Text,
                    Description = x.Description,
                    IsActive = x.IsActive,
                    Order = x.Order,
                    CreatedBy = updatedBy,
                    CreatedDate = DateTime.Now,
                    ComplianceNumber = x.ComplianceNumber,
                    QuestionTypesFlag = x.QuestionTypesFlag,
                    ScopeTypes = x.ScopeTypes.Select(y=>new ApplicationSectionQuestionScopeType
                        {
                            Id = Guid.NewGuid(),
                            ScopeTypeId = y.ScopeTypeId,
                            CreatedBy = updatedBy,
                            CreatedDate = DateTime.Now
                        })
                    .ToList(),
                    Answers = x.Answers.Select(y=> new ApplicationSectionQuestionAnswer
                    {
                        Id = Guid.NewGuid(),
                        Text = y.Text,
                        IsActive = y.IsActive,
                        Order = y.Order,
                        CreatedBy = updatedBy,
                        CreatedDate = DateTime.Now,
                        IsExpectedAnswer = y.IsExpectedAnswer,
                        Displays = y.Displays.Where(zz=>questions.ContainsKey(zz.HidesQuestionId)).Select(z=> new ApplicationSectionQuestionAnswerDisplay
                        {
                            Id = Guid.NewGuid(),
                            HidesQuestionId = questions[z.HidesQuestionId],
                            CreatedBy = updatedBy,
                            CreatedDate = DateTime.Now
                        })
                        .ToList(),
                    })
                    .ToList()
                })
                .ToList(),
                ApplicationSectionScopeTypes = section.ApplicationSectionScopeTypes.Select(x=> new ApplicationSectionScopeType
                {
                    CreatedDate = DateTime.Now,
                    CreatedBy = updatedBy,
                    Id = Guid.NewGuid(),
                    IsActual = x.IsActual,
                    IsDefault = x.IsDefault,
                    ScopeTypeId = x.ScopeTypeId
                })
                .ToList()
            };
        }
    }
}















