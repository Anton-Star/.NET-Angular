using FactWeb.Infrastructure;
using FactWeb.Model;
using FactWeb.Model.InterfaceItems;
using FactWeb.RepositoryContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationVersion = FactWeb.Model.ApplicationVersion;

namespace FactWeb.BusinessLayer
{
    public class ApplicationVersionManager : BaseManager<ApplicationVersionManager, IApplicationVersionRepository, ApplicationVersion>
    {
        public ApplicationVersionManager(IApplicationVersionRepository repository) : base(repository)
        {
        }

        /// <summary>
        /// Gets all the application version by their type
        /// </summary>
        /// <param name="applicationTypeId">Id of the application type</param>
        /// <returns>Collection of ApplicationVersion objects</returns>
        public List<ApplicationVersion> GetByType(int applicationTypeId)
        {
            return this.Repository.GetByType(applicationTypeId);
        }

        /// <summary>
        /// Gets all the application version by their type asynchronously
        /// </summary>
        /// <param name="applicationTypeId">Id of the application type</param>
        /// <returns>Collection of ApplicationVersion objects</returns>
        public Task<List<ApplicationVersion>> GetByTypeAsync(int applicationTypeId)
        {
            return this.Repository.GetByTypeAsync(applicationTypeId);
        }
        
        public List<ApplicationVersion> GetByType(List<ApplicationSection> sections, List<ApplicationSectionQuestion> questions, List<ApplicationQuestionNotApplicable> notApplicables, string applicationTypeName)
        {
            var versions = this.Repository.GetByType(applicationTypeName);

            foreach (var version in versions)
            {
                version.ApplicationSections =
                    sections.Where(x => x.ApplicationVersionId == version.Id && x.ParentApplicationSectionId == null)
                        .ToList();

                foreach (var section in version.ApplicationSections)
                {
                    section.Questions = questions.Where(x => x.ApplicationSectionId == section.Id).ToList();

                    this.GetChildren(sections, questions, notApplicables, section);
                }
            }

            return versions;
        }

        public void CheckVersionNumber(Guid? applicationSectionId, string newVersionNumber, string savedBy)
        {
            if (applicationSectionId == null) return;

            var applicationVersion = this.Repository.GetByApplicationSection(applicationSectionId.Value);

            if (applicationVersion.VersionNumber == newVersionNumber) return;

            applicationVersion.VersionNumber = newVersionNumber;
            applicationVersion.UpdatedBy = savedBy;
            applicationVersion.UpdatedDate = DateTime.Now;

            base.Repository.Save(applicationVersion);
        }

        public ApplicationVersion GetByApplicationSection(Guid applicationSectionId)
        {
            return this.Repository.GetByApplicationSection(applicationSectionId);
        }

        public Task<ApplicationVersion> GetByApplicationSectionAsync(Guid applicationSectionId)
        {
            return this.Repository.GetByApplicationSectionAsync(applicationSectionId);
        }

        public List<ExportModel> Export(Guid applicationVersionId)
        {
            return this.Repository.Export(applicationVersionId);
        }

        public async Task CheckVersionNumberAsync(ApplicationVersion applicationVersion, string newVersionNumber, string savedBy)
        {
            if (applicationVersion == null) return;

            if (applicationVersion.VersionNumber == newVersionNumber) return;

            applicationVersion.VersionNumber = newVersionNumber;
            applicationVersion.UpdatedBy = savedBy;
            applicationVersion.UpdatedDate = DateTime.Now;

            await base.Repository.SaveAsync(applicationVersion);
        }

        public ApplicationVersion GetApplicationVersion(Guid versionId)
        {
            var flat = this.Repository.GetFlatApplication(versionId);

            if (flat.Count == 0) return null;

            var first = flat.First();

            var applicationVersion = new ApplicationVersion
            {
                Id = first.ApplicationVersionId,
                Title = first.ApplicationVersionTitle,
                VersionNumber = first.ApplicationVersionNumber,
                IsActive = first.ApplicationSectionIsActive.GetValueOrDefault(),
                ApplicationTypeId = first.ApplicationTypeId.GetValueOrDefault(),
                ApplicationType = new ApplicationType
                {
                    Id = first.ApplicationTypeId.GetValueOrDefault(),
                    Name = first.ApplicationTypeName
                },
                ApplicationSections = new List<ApplicationSection>(),

            };

            var rootItems = flat.Where(x => x.ParentApplicationSectionId == null).Select(x=> new
            {
                x.ApplicationSectionId
            }).Distinct();

            foreach (var row in rootItems.Select(item => flat.First(x => x.ApplicationSectionId == item.ApplicationSectionId)))
            {
                applicationVersion.ApplicationSections.Add(new ApplicationSection()
                {
                    Id = row.ApplicationSectionId,
                    ApplicationTypeId = row.ApplicationTypeId.GetValueOrDefault(),
                    ApplicationType = new ApplicationType
                    {
                        Id = row.ApplicationTypeId.GetValueOrDefault(),
                        Name = row.ApplicationTypeName
                    },
                    Children = new List<ApplicationSection>(),
                    ApplicationSectionScopeTypes = new List<ApplicationSectionScopeType>(),
                    Questions = new List<ApplicationSectionQuestion>(),
                    Name = row.ApplicationSectionName,
                    IsActive = row.ApplicationSectionIsActive.GetValueOrDefault(),
                    HelpText = row.ApplicationSectionHelpText,
                    IsVariance = row.ApplicationSectionIsVariance.GetValueOrDefault(),
                    Order = row.ApplicationSectionOrder,
                    UniqueIdentifier = row.ApplicationSectionUniqueIdentifier
                });
            }

            


            return applicationVersion;
        }

        /// <summary>
        /// Gets all the application version by their type asynchronously
        /// </summary>
        /// <param name="applicationTypeName">Name of the application type</param>
        /// <returns>Collection of ApplicationVersion objects</returns>
        public Task<List<ApplicationVersion>> GetByTypeAsync(string applicationTypeName)
        {
            return this.Repository.GetByTypeAsync(applicationTypeName);
        }

        public List<ApplicationVersionItem> GetAllForApplicationType(List<ApplicationSection> applicationSections, string applicationTypeName, bool onlyIncludeSectionsForActive)
        {
            LogMessage("GetAllForApplicationTypeItems (ApplicationSectionManager)");

            var items = base.Repository.GetByType(applicationTypeName).OrderBy(x => x.CreatedDate).ToList();

            return items.Select(x=>ModelConversions.Convert(x, null, onlyIncludeSectionsForActive, applicationSections)).ToList();
        }

        /// <summary>
        /// Makes a version the current active one
        /// </summary>
        /// <param name="versionId">Id of the version</param>
        /// <param name="updatedBy">Who is doing the update</param>
        public void MakeActive(Guid versionId, string updatedBy)
        {
            var version = this.GetById(versionId);

            if (version == null)
            {
                throw new KeyNotFoundException("Cannot find version");
            }

            var versions = this.GetByType(version.ApplicationTypeId)
                .Where(x=>x.IsActive || x.Id == versionId);

            foreach (var record in versions)
            {
                record.IsActive = record.Id == versionId;
                record.UpdatedBy = updatedBy;
                record.UpdatedDate = DateTime.Now;
                base.Repository.BatchSave(record);
            }

            base.Repository.SaveChanges();
        }

        /// <summary>
        /// Makes a version the current active one asynchronously
        /// </summary>
        /// <param name="versionId">Id of the version</param>
        /// <param name="updatedBy">Who is doing the update</param>
        public async Task MakeActiveAsync(ApplicationVersion version, List<ApplicationVersion> versions, string updatedBy)
        {  
            
            foreach (var record in versions.Where(x => x.IsActive || x.Id == version.Id))
            {
                record.IsActive = record.Id == version.Id;
                record.UpdatedBy = updatedBy;
                record.UpdatedDate = DateTime.Now;
                base.Repository.BatchSave(record);
            }

            await base.Repository.SaveChangesAsync();
            
        }

        /// <summary>
        /// Adds a new Version to the repository
        /// </summary>
        /// <param name="type">Type the version is for</param>
        /// <param name="newSections">Sections to be copied</param>
        /// <param name="title">Title of the version</param>
        /// <param name="number">Number of the version</param>
        /// <param name="isActive">Whether the version should be marked as the active one</param>
        /// <param name="createdBy">Who is creating the record</param>
        /// <returns></returns>
        public ApplicationVersion Add(ApplicationType type, List<ApplicationSection> newSections, string title, string number, bool isActive,
            string createdBy)
        {
            var version = new ApplicationVersion
            {
                Id = Guid.NewGuid(),
                ApplicationTypeId = type.Id,
                Title = title,
                VersionNumber = number,
                IsActive = isActive,
                CreatedDate = DateTime.Now,
                CreatedBy = createdBy,
                ApplicationSections = newSections ?? new List<ApplicationSection>()
            };
            
            base.Repository.Add(version);

            return version;
        }

        /// <summary>
        /// Adds a new Version to the repository asynchronously
        /// </summary>
        /// <param name="type">Type the version is for</param>
        /// <param name="copyFromVersionId">Id of the version that should be copied</param>
        /// <param name="title">Title of the version</param>
        /// <param name="number">Number of the version</param>
        /// <param name="isActive">Whether the version should be marked as the active one</param>
        /// <param name="createdBy">Who is creating the record</param>
        public async Task<ApplicationVersion> AddAsync(ApplicationType type, Guid? copyFromVersionId, string title,
            string number, bool isActive, string createdBy)
        {
            ApplicationVersion copiedVersion = null;
            if (copyFromVersionId.HasValue)
            {
                copiedVersion = this.GetById(copyFromVersionId.Value);
            }

            var version = new ApplicationVersion
            {
                Id = Guid.NewGuid(),
                ApplicationTypeId = type.Id,
                Title = title,
                VersionNumber = number,
                IsActive = isActive,
                CreatedDate = DateTime.Now,
                CreatedBy = createdBy,
                ApplicationSections = new List<ApplicationSection>()
            };


            if (copiedVersion != null)
            {
                var sections = copiedVersion.ApplicationSections.ToList();

                foreach (var section in sections)
                {
                    section.Id = Guid.NewGuid();
                    version.ApplicationSections.Add(section);
                }
            }

            await base.Repository.AddAsync(version);

            return version;
        }
        /// <summary>
        /// Gets all the active application version asynchronously
        /// </summary>
        /// <returns>Collection of ApplicationVersion objects</returns>
        public Task<List<ApplicationVersion>> GetActiveVersionsAsync()
        {
            return base.Repository.GetActiveAsync();
        }

        public List<ApplicationVersion> GetActiveVersions()
        {
            return base.Repository.GetActive();
        }

        /// <summary>
        /// Gets all the active application version
        /// </summary>
        /// <returns>Collection of ApplicationVersion objects</returns>
        public List<ApplicationVersion> GetActiveVersions(List<ApplicationSection> sections, List<ApplicationSectionQuestion> questions, List<ApplicationQuestionNotApplicable> notApplicables)
        {
            var versions = base.Repository.GetActive();

            foreach (var version in versions)
            {
                version.ApplicationSections =
                    sections.Where(x => x.ApplicationVersionId == version.Id && x.ParentApplicationSectionId == null)
                        .ToList();

                foreach (var section in version.ApplicationSections)
                {
                    this.GetChildren(sections, questions, notApplicables, section, true);
                }
            }

            return versions;
        }

        public ApplicationVersion GetVersion(Guid versionId, List<ApplicationSection> sections, List<ApplicationSectionQuestion> questions, List<ApplicationQuestionNotApplicable> notApplicables)
        {
            var version = base.Repository.GetVersion(versionId);

            version.ApplicationSections =
                    sections.Where(x => x.ApplicationVersionId == version.Id && x.ParentApplicationSectionId == null)
                        .ToList();

            foreach (var section in version.ApplicationSections)
            {
                this.GetChildren(sections, questions, notApplicables, section, true);
            }

            return version;
        }

        private void GetChildren(List<ApplicationSection> sections, List<ApplicationSectionQuestion> questions, List<ApplicationQuestionNotApplicable> notApplicables, ApplicationSection section, bool includeQuestions = false)
        {
            section.Children = sections.Where(x => x.ParentApplicationSectionId == section.Id).ToList();

            foreach (var child in section.Children)
            {
                if (includeQuestions)
                {
                    child.Questions = questions.Where(x => x.ApplicationSectionId == child.Id).ToList();

                    foreach (var question in child.Questions)
                    {
                        question.ApplicationSection = child;
                        question.ApplicationQuestionNotApplicables =
                            notApplicables.Where(x => x.ApplicationSectionQuestionId == question.Id).ToList();
                    }
                }
                

                this.GetChildren(sections, questions, notApplicables, child, includeQuestions);
            }
        }
    }
}
